open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Saturn
open Thoth.Json
open Shared

open Microsoft.WindowsAzure.Storage
open Microsoft.Azure.Cosmos.Table

let tryGetEnv = System.Environment.GetEnvironmentVariable >> function null | "" -> None | x -> Some x

let publicPath = tryGetEnv "public_path" |> Option.defaultValue "../Client/public" |> Path.GetFullPath
let storageConnectionString = tryGetEnv "CONNECT_STR"
let storageAccount = tryGetEnv "CONNECT_STR" |> Option.defaultValue "UseDevelopmentStorage=true" |> CloudStorageAccount.Parse

// Create the table client.
let tableClient = storageAccount.CreateCloudTableClient()

let bondFilmTable = tableClient.GetTableReference("BondFilm")

let getBondfilms =
    TableQuery().Where(
        TableQuery.GenerateFilterCondition(
            "PartitionKey", QueryComparisons.Equal, "BondFilm"))

let getEnemies bondFilmSequenceId =
    tableClient.GetTableReference("TheEnemy")
               .ExecuteQuery(
                   TableQuery().Where(sprintf "PartitionKey eq '%d'" bondFilmSequenceId))
    |> Seq.map (fun e -> { Name = e.Properties.["Character"].StringValue; Actor = e.Properties.["Actor"].StringValue; ImageURI = None })

let getGirls bondFilmSequenceId =
    tableClient.GetTableReference("TheGirls")
               .ExecuteQuery(
                   TableQuery().Where(sprintf "PartitionKey eq '%d'" bondFilmSequenceId))
    |> Seq.map (fun e -> { Name = e.Properties.["Character"].StringValue; Actor = e.Properties.["Actor"].StringValue; ImageURI = None })


let getImgURI filmId character = AzureServices.getBondMediaCharacterURI (string filmId) character

let getBondFilm (filmId : int) bondGirlsFn bondFoesFn =
    let tq =
        TableQuery().Where(
            TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "BondFilm"),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, filmId.ToString())))

    let bondData = bondFilmTable.ExecuteQuery(tq) |> Seq.head

    let sequenceId = int bondData.RowKey
    let title = if bondData.Properties.ContainsKey("Title") then bondData.Properties.["Title"].StringValue else ""
    let synopsis = if bondData.Properties.ContainsKey("Synopsis") then bondData.Properties.["Synopsis"].StringValue else ""
    let bond = if bondData.Properties.ContainsKey("Bond") then bondData.Properties.["Bond"].StringValue else ""
    let m = if bondData.Properties.ContainsKey("M") then Some (bondData.Properties.["M"].StringValue) else None
    let q = if bondData.Properties.ContainsKey("Q") then Some (bondData.Properties.["Q"].StringValue) else None
    let theEnemy = bondFoesFn sequenceId |> Seq.toList
    let theGirls = bondGirlsFn sequenceId |> Seq.toList

    {SequenceId = sequenceId; Title = title; Synopsis = synopsis;
     Bond = Some {Name="James Bond"; Actor=bond; ImageURI = (getImgURI sequenceId "James Bond") };
     M = m |> Option.map (fun actor -> {Name="M"; Actor=actor; ImageURI = (getImgURI sequenceId "M") });
     Q = q |> Option.map (fun actor -> {Name="Q"; Actor=actor; ImageURI = (getImgURI sequenceId "Q") });
     TheEnemy = theEnemy; TheGirls = theGirls}

let buildMovieList (bondDataFn : DynamicTableEntity seq) bondGirlsFn bondFoesFn =
    bondDataFn
    |> Seq.map (fun f ->
                    let sequenceId = int f.RowKey
                    let title = if f.Properties.ContainsKey("Title") then f.Properties.["Title"].StringValue else ""
                    let synopsis = if f.Properties.ContainsKey("Synopsis") then f.Properties.["Synopsis"].StringValue else ""
                    let bond = if f.Properties.ContainsKey("Bond") then f.Properties.["Bond"].StringValue else ""
                    let m = if f.Properties.ContainsKey("M") then Some (f.Properties.["M"].StringValue) else None
                    let q = if f.Properties.ContainsKey("Q") then Some (f.Properties.["Q"].StringValue) else None
                    let theEnemy = bondFoesFn sequenceId |> Seq.toList
                    let theGirls = bondGirlsFn sequenceId |> Seq.toList

                    {SequenceId = sequenceId; Title = title; Synopsis = synopsis;
                     Bond = Some {Name="James Bond"; Actor=bond; ImageURI = (getImgURI sequenceId "James Bond") };
                     M = m |> Option.map (fun actor -> {Name="M"; Actor=actor; ImageURI = (getImgURI sequenceId "M") });
                     Q = q |> Option.map (fun actor -> {Name="Q"; Actor=actor; ImageURI = (getImgURI sequenceId "Q") });
                     TheEnemy = theEnemy; TheGirls = theGirls})
            |> Seq.toList (* <- NOTE this is important the encoder doesn't like IEnumerable need to convert to List *)


let port =
    "SERVER_PORT"
    |> tryGetEnv |> Option.map uint16 |> Option.defaultValue 8086us

let webApp = router {
    get "/api/films" (fun next ctx ->
        task {
            let movieList = buildMovieList (bondFilmTable.ExecuteQuery(getBondfilms)) getGirls getEnemies
            return! json movieList next ctx
        })
    getf "/api/list-media/%s" (fun filmId next ctx ->
        task {
            return! json (AzureServices.listBondMedia filmId) next ctx
        })
    getf "/api/media-item-character/%s/%s" (fun (filmId, character) next ctx ->
        task {
            return! json (AzureServices.getBondMediaCharacterURI filmId character) next ctx
        })
    }

let configureAzure (services:IServiceCollection) =
    tryGetEnv "APPINSIGHTS_INSTRUMENTATIONKEY"
    |> Option.map services.AddApplicationInsightsTelemetry
    |> Option.defaultValue services

let app = application {
    url ("http://0.0.0.0:" + port.ToString() + "/")
    use_router webApp
    memory_cache
    use_static publicPath
    use_json_serializer(Thoth.Json.Giraffe.ThothSerializer())
    service_config configureAzure
    use_gzip
}

run app
