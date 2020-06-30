module Review.View

open Fable.React
open Fable.React.Props
open Fulma
open Review.Types
open Shared

let view (model : Review) (dispatch : Review.Types.Msg -> unit) =

    Modal.modal [ Modal.IsActive true ]
        [ Modal.background [ Props [ OnClick (fun _ -> dispatch CancelReview) ] ] [ ]
          Modal.Card.card [ ]
            [ Modal.Card.head [ ]
                [ Modal.Card.title [ ]
                    [ str "model.FilmName" ]
                  Delete.delete [ Delete.OnClick (fun _ -> dispatch CancelReview) ] [ ] ]
              Modal.Card.body [ ]
                [
                    div []
                        [
                            // Rating.view true 5 model.Rating (fun msg -> dispatch (RatingMsg (msg)))

                            Input.input
                                [
                                    Input.Placeholder "Reviewers name"
                                    Input.OnChange (fun ev -> dispatch (UserFieldChanged ({ model with Who = ev.Value})))
                                ]

                            Textarea.textarea
                                [
                                    Textarea.Placeholder "Review text"
                                    Textarea.OnChange (fun ev -> dispatch (CommentFieldChanged ({ model with Comment = ev.Value})))
                                ][]
                        ]
                ]
              Modal.Card.foot [ ]
                [ Button.button [ Button.Color IsSuccess; Button.OnClick (fun _ -> dispatch (SubmitReview model))  ]
                    [ str "Save changes" ]
                  Button.button [ ]
                    [ str "Cancel" ] ] ] ]
