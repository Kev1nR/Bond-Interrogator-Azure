module SharedViews

open Fable.React
open Fable.React.Props
open Fulma

let modalView isActive title content actionButtons cancelAction =
    Modal.modal [ Modal.IsActive isActive ]
        [ Modal.background [ Props [ OnClick (fun _ -> cancelAction) ] ] [ ]
          Modal.Card.card [ ]
            [ Modal.Card.head [ ]
                [ Modal.Card.title [ ]
                    [ str title ]
                  Delete.delete [ Delete.OnClick (fun _ -> cancelAction) ] [ ] ]
              Modal.Card.body [ ]
                [
                    content
                    // div []
                    //     [
                    //         Rating.view true 5 model.Rating (fun msg -> dispatch (RatingMsg (msg)))

                    //         Input.input
                    //             [
                    //                 Input.Placeholder "Reviewers name"
                    //                 Input.OnChange (fun ev -> dispatch (UserFieldChanged ev.Value))
                    //             ]

                    //         Textarea.textarea
                    //             [
                    //                 Textarea.Placeholder "Review text"
                    //                 Textarea.OnChange (fun ev -> dispatch (CommentFieldChanged ev.Value))
                    //             ][]
                    //     ]
                ]
              Modal.Card.foot [ ]
                [ 
                    actionButtons
                //   Button.button [ Button.Color IsSuccess; Button.OnClick (fun _ -> dispatch (SubmitReview model.Review))  ]
                //     [ str "Save changes" ]
                //   Button.button [ ]
                //     [ str "Cancel" ] 
            ] ] ]
