module Client.Components

open Elmish
open Fable.FontAwesome
open Fable.React
open Fable.React.Props
open Thoth.Fetch
open Fulma

open Shared
open Fable.Core.JS

type ReviewMsg =
    | Rating of int

let ratingComponent reviews =
    let rateSum = reviews |> Seq.fold (fun acc r -> acc + r.Rating) 0
    let rateCount = reviews |> List.length
    let comp = if rateCount = 0
               then
                    p [] [ str "Be the first to review this film"]
               else
                    let aveRating = rateSum / rateCount
                    p [] [
                            for i in 1..5 do
                                yield Icon.icon [ Icon.Option.Modifiers [ Modifier.TextColor IsWarning ] ] 
                                        [ Fa.i [ (if i <= aveRating then Fa.Solid.Star else Fa.Regular.Star) ] [ ] ]

                            yield str (sprintf " from %d reviews" rateCount)
                         ]
    comp

let update (msg : ReviewMsg) (currentModel : Review) : Review * Cmd<ReviewMsg> =
    match msg with
    | Rating r -> printfn "Rating is %d" r
    currentModel, Cmd.none


