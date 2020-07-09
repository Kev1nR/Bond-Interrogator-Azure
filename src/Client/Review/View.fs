module Review.View

open Fable.React
open Fable.React.Props
open Fulma
open Review.Types
<<<<<<< HEAD

let view (model : Review.Types.Model) (dispatch : Review.Types.Msg -> unit) =
=======
open Shared

let view title (model : Model) (dispatch : Review.Types.Msg -> unit) =
>>>>>>> Scene3-Take1

    Modal.modal [ Modal.IsActive true ]
        [ Modal.background [ Props [ OnClick (fun _ -> dispatch CancelReview) ] ] [ ]
          Modal.Card.card [ ]
            [ Modal.Card.head [ ]
                [ Modal.Card.title [ ]
<<<<<<< HEAD
                    [ str model.FilmName ]
=======
                    [ str title ]
>>>>>>> Scene3-Take1
                  Delete.delete [ Delete.OnClick (fun _ -> dispatch CancelReview) ] [ ] ]
              Modal.Card.body [ ]
                [
                    div []
                        [
<<<<<<< HEAD
                            Rating.view true 5 model.Rating (fun msg -> dispatch (RatingMsg (msg)))
=======
                            Rating.fiveStarRater model.RatingModel (fun msg -> dispatch (RatingMsg (msg)))
>>>>>>> Scene3-Take1

                            Input.input
                                [
                                    Input.Placeholder "Reviewers name"
<<<<<<< HEAD
                                    Input.OnChange (fun ev -> dispatch (UserFieldChanged ev.Value))
=======
                                    Input.OnChange (fun ev -> dispatch (ContentChanged ({ model.Review with Who = ev.Value})))
>>>>>>> Scene3-Take1
                                ]

                            Textarea.textarea
                                [
                                    Textarea.Placeholder "Review text"
<<<<<<< HEAD
                                    Textarea.OnChange (fun ev -> dispatch (CommentFieldChanged ev.Value))
=======
                                    Textarea.OnChange (fun ev -> dispatch (ContentChanged ({ model.Review with Comment = ev.Value})))
>>>>>>> Scene3-Take1
                                ][]
                        ]
                ]
              Modal.Card.foot [ ]
                [ Button.button [ Button.Color IsSuccess; Button.OnClick (fun _ -> dispatch (SubmitReview model.Review))  ]
                    [ str "Save changes" ]
<<<<<<< HEAD
                  Button.button [ ]
=======
                  Button.button [ Button.OnClick (fun _ -> dispatch (CancelReview))]
>>>>>>> Scene3-Take1
                    [ str "Cancel" ] ] ] ]
