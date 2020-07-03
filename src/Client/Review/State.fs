module Review.State

open Rating
open Review.Types
open Shared
open Elmish

// let init film =
//     let initialModel =
//         {
//             FilmName = film.Title
//             Review =
//                 {
//                     SequenceId = film.SequenceId
//                     Rating = 0
//                     Who = ""
//                     Comment = ""
//                     PostedDate = System.DateTime.Now
//                 }
//             RatingModel = { MaxRating = 5; HoverRating = 0; SelectedRating = 0; IsReadOnly = false }
//             Rating = Rating.init()
//         }

//     initialModel

let update (msg : Msg) (currentModel : Review) : Review * Cmd<Msg> =
    match msg with
    // | RatingMsg msg ->
    //     printfn "Got a RatingMsg of %A " msg
    //     let newRating, _ = Rating.update msg currentModel
    //     { currentModel with Rating = newRating }, Cmd.none
    | ContentChanged review ->
        printfn "Changing model to %A" review
        review, Cmd.none
    | SubmitReview review ->
        printfn "New review is %A" review
        review, Cmd.none
    | CancelReview -> currentModel, Cmd.none
