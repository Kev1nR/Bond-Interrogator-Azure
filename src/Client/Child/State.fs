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
            RatingModel = { MaxRating = 5; HoverRating = 0; SelectedRating = 0; IsReadOnly = false }
        }

    initialModel

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | HoverRating rate ->
        let newRating = { currentModel.RatingModel with HoverRating = rate }
        let nextModel = { currentModel with RatingModel = newRating }
        printfn "No change just notifying that %s hovering over %d" currentModel.FilmName rate
        nextModel, Cmd.none
    | SelectedRating rate ->
        let newRating = { currentModel.RatingModel with SelectedRating = rate }
        let newReview = { currentModel.Review with Rating = rate }
        let nextModel = { currentModel with Review = newReview; RatingModel = newRating }
        printfn "Changing model to %A" nextModel
        nextModel, Cmd.none
    | UserFieldChanged user ->
        let newReview = { currentModel.Review with Who = user }
        let nextModel = { currentModel with Review = newReview }
        printfn "Changing model to %A" nextModel
        nextModel, Cmd.none
    | CommentFieldChanged comment ->
        let newReview = { currentModel.Review with Comment = comment }
        let nextModel = { currentModel with Review = newReview }
        printfn "Changing model to %A" nextModel
        nextModel, Cmd.none
    | SubmitReview review ->
        let nextModel = { currentModel with Review = review }
        printfn "New review is %A" nextModel
        nextModel, Cmd.none
    | CancelReview -> currentModel, Cmd.none
