module Review.View

open Fable.React
open Fable.React.Props
open Fulma
open Review.Types
open Shared

let view title (model : Model) (dispatch : Review.Types.Msg -> unit) =

    Modal.modal [ Modal.IsActive true ]
        [ Modal.background [ Props [ OnClick (fun _ -> dispatch CancelReview) ] ] [ ]
          Modal.Card.card [ ]
            [ Modal.Card.head [ ]
                [ Modal.Card.title [ ]
                    [ str title ]
                  Delete.delete [ Delete.OnClick (fun _ -> dispatch CancelReview) ] [ ] ]
              Modal.Card.body [ ]
                [
                    div []
                        [
                            Rating.fiveStarRater model.RatingModel (fun msg -> dispatch (RatingMsg (msg)))

                            Input.input
                                [
                                    Input.Placeholder "Reviewers name"
                                    Input.OnChange (fun ev -> dispatch (ContentChanged ({ model.Review with Who = ev.Value})))
                                ]

                            Textarea.textarea
                                [
                                    Textarea.Placeholder "Review text"
                                    Textarea.OnChange (fun ev -> dispatch (ContentChanged ({ model.Review with Comment = ev.Value})))
                                ][]
                        ]
                ]
              Modal.Card.foot [ ]
                [ Button.button [ Button.Color IsSuccess; Button.OnClick (fun _ -> dispatch (SubmitReview model.Review))  ]
                    [ str "Save changes" ]
                  Button.button [ Button.OnClick (fun _ -> dispatch (CancelReview))]
                    [ str "Cancel" ] ] ] ]
