module Client.Components

open Elmish
open Fable.FontAwesome
open Fable.React
open Fable.React.Props
open Thoth.Fetch
open Fulma

open Shared
open Fable.Core.JS


let ratingComponent reviews synopsis =
    let rateSum = reviews |> Seq.fold (fun acc r -> acc + r.Rating) 0
    let rateCount = reviews |> List.length
    let comp = if rateCount = 0
               then
                        p [] [ str "Be the first to review this film"]
               else
                    let aveRating = rateSum / rateCount
                    p [] [
                            for i in 1..aveRating do
                                yield Icon.icon [ Icon.Option.Modifiers [ Modifier.TextColor Color.IsWarning ] ] [ Fa.i [ Fa.Solid.Star ] [ ] ]

                            for i in (aveRating + 1)..5 do
                                yield Icon.icon [ ] [ Fa.i [ Fa.Regular.Star ] [] ]

                            yield str (sprintf " from %d reviews" rateCount)
                         ]
    comp