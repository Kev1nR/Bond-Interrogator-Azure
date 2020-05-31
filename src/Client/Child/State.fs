module Child.State

open Child.Types
open Shared
open Elmish

let init film =
    let initialModel =
        {
            FilmName = film.Title
            Review =
                {
                    SequenceId = film.SequenceId
                    Rating = 0
                    Who = "Kevin"
                    Comment = "Great film"
                    PostedDate = System.DateTime.Now
                }
        }
    //{Name = "Review1"; Value = 0 }
    initialModel

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | HoverRating rate ->
        printfn "No change just notifying that %s hovering over %d" currentModel.FilmName rate
        currentModel, Cmd.none
    | SelectedRating rate ->
        let newReview = { currentModel.Review with Rating = rate }
        let nextModel = { currentModel with Review = newReview }
        printfn "Changing model to %A" nextModel
        nextModel, Cmd.none
    | SubmitReview review ->
        let nextModel = { currentModel with Review = review }
        printfn "New review is %A" nextModel
        nextModel, Cmd.none
    | CancelReview -> currentModel, Cmd.none
