module ReviewViewer.View

open Fable.React
open Fable.React.Props
open Fulma
open Review.Types
open Shared

let view title (model : Model) (dispatch : Review.Types.Msg -> unit) =
    Card.card []
        [
            str "this is my review"
        ]

// Modal.Card.body [ ]
//                 [
//                     div []
//                         [
//                             Rating.fiveStarRater model.RatingModel (fun msg -> dispatch (RatingMsg (msg)))

//                             Input.input
//                                 [
//                                     Input.Placeholder "Reviewers name"
//                                     Input.OnChange (fun ev -> dispatch (ContentChanged ({ model.Review with Who = ev.Value})))
//                                 ]

//                             Textarea.textarea
//                                 [
//                                     Textarea.Placeholder "Review text"
//                                     Textarea.OnChange (fun ev -> dispatch (ContentChanged ({ model.Review with Comment = ev.Value})))
//                                 ][]
//                         ]
//                 ]