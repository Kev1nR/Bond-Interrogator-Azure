open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Saturn
open Shared

open Microsoft.WindowsAzure.Storage
open Microsoft.Azure.Cosmos.Table

let tryGetEnv = System.Environment.GetEnvironmentVariable >> function null | "" -> None | x -> Some x

let publicPath = tryGetEnv "public_path" |> Option.defaultValue "../Client/public" |> Path.GetFullPath
let storageConnectionString = tryGetEnv "CONNECT_STR"
let storageAccount = tryGetEnv "CONNECT_STR" |> Option.defaultValue "UseDevelopmentStorage=true" |> CloudStorageAccount.Parse

// Create the table client.
let tableClient = storageAccount.CreateCloudTableClient()

let table = tableClient.GetTableReference("BondFilm")

let query =
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

let getReviews bondFilmSequenceId =
    tableClient.GetTableReference("Review")
               .ExecuteQuery(
                   TableQuery().Where(sprintf "PartitionKey eq '%d'" bondFilmSequenceId))
    |> Seq.map (fun e -> { SequenceId = bondFilmSequenceId; Rating = e.Properties.["Rating"].Int32Value.Value; Who = e.RowKey;
                           Comment = e.Properties.["Comment"].StringValue; PostedDate = e.Properties.["PostedDate"].DateTime.Value})

let getImgURI filmId character = AzureServices.getBondMediaCharacterURI (string filmId) character

let buildMovieList (bondDataFn : DynamicTableEntity seq) bondGirlsFn bondFoesFn reviewsFn =
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
                    let reviews = reviewsFn sequenceId |> Seq.toList

                    {SequenceId = sequenceId; Title = title; Synopsis = synopsis;
                     Bond = Some {Name="James Bond"; Actor=bond; ImageURI = (getImgURI sequenceId "James Bond") };
                     M = m |> Option.map (fun actor -> {Name="M"; Actor=actor; ImageURI = (getImgURI sequenceId "M") });
                     Q = q |> Option.map (fun actor -> {Name="Q"; Actor=actor; ImageURI = (getImgURI sequenceId "Q") });
                     TheEnemy = theEnemy; TheGirls = theGirls
                     Reviews = reviews})
            |> Seq.toList (* <- NOTE this is important the encoder doesn't like IEnumerable need to convert to List *)

let postReview review =
    table.ExecuteQuery(
        TableQuery().Where(sprintf "PartitionKey eq '%d'" review.SequenceId))

    //TableOperation.InsertOrReplace()




let port =
    "SERVER_PORT"
    |> tryGetEnv |> Option.map uint16 |> Option.defaultValue 8086us

let webApp = router {
    get "/api/films" (fun next ctx ->
        task {
            let movieList = buildMovieList (table.ExecuteQuery(query)) getGirls getEnemies getReviews
            
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
    post "/api/add-review" (fun next ctx ->
        task {
            use strm = new StreamReader(ctx.Request.Body)
            let! conts = strm.ReadToEndAsync()
            printfn "Body is %A " conts
            let review = { SequenceId = 1; Rating = 5; Who = "Kevin"; Comment = "Really good"; PostedDate = System.DateTime.Now }
            return! json review next ctx
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
