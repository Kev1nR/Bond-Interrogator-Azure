module Review.State

open Review.Types
open Shared
open Elmish

let update (msg : Msg) (currentModel : Review) : Review * Cmd<Msg> =
    match msg with
    | UserFieldChanged review | CommentFieldChanged review ->
        printfn "Changing model to %A" review
        review, Cmd.none
    | SubmitReview review ->
        printfn "New review is %A" review
        review, Cmd.none
    | CancelReview -> currentModel, Cmd.none
