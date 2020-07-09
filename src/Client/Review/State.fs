module Review.State

open Rating
open Review.Types
open Shared
open Elmish

let init film =
    let newReview = {
                        SequenceId = film.SequenceId
                        Rating = 0
                        Who = ""
                        Comment = ""
                        PostedDate = System.DateTime.Now
                    }

    let newRating = Rating.init()

    { Review = newReview; RatingModel = newRating }, Cmd.none

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | RatingMsg msg ->
        printfn "Got a RatingMsg of %A " msg
        let newRating, _ = Rating.update msg currentModel.RatingModel
        let newReview = { currentModel.Review with Rating = newRating.SelectedRating }
        { currentModel with Review = newReview; RatingModel = newRating }, Cmd.none
    | ContentChanged review ->
        printfn "Changing model to %A" review
        { currentModel with Review = review }, Cmd.none
    | SubmitReview review ->
        printfn "New review is %A" review
        { currentModel with Review = review }, Cmd.none
    | CancelReview -> currentModel, Cmd.none
