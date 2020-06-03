module Child.View

open Elmish
open Fable.FontAwesome
open Fable.React
open Fable.React.Props
open Thoth.Fetch
open Fulma
open Child.Types

let view (model : Child.Types.Model) (dispatch : Child.Types.Msg -> unit) =

    Modal.modal [ Modal.IsActive true ]
        [ Modal.background [ Props [ OnClick (fun _ -> dispatch CancelReview) ] ] [ ]
          Modal.Card.card [ ]
            [ Modal.Card.head [ ]
                [ Modal.Card.title [ ]
                    [ str model.FilmName ]
                  Delete.delete [ Delete.OnClick (fun _ -> dispatch CancelReview) ] [ ] ]
              Modal.Card.body [ ]
                [
                    div []
                        [
                            // Container.container []
                            //     [
                            //          Span.span [ OnMouseOver (fun _ -> printfn "mouse over") ] []
                            //     ]

                            p [] [
                                    // OnMouseOver (fun ev -> dispatch (UserFieldChanged "x")) ] [ str "click" ]
                                    for i in 1..5 do
                                        yield a [ OnClick (fun _ -> printfn "sd" )]
                                            [
                                                Icon.icon [ Icon.Option.Modifiers [ Modifier.TextColor IsWarning ] ]
                                                    [ Fa.i [ (if i <= 3 then Fa.Solid.Star else Fa.Regular.Star) ] [ ] ]
                                            ]

                                    yield str (sprintf " from %d reviews" 10)
                                 ]
                            Input.input
                                [
                                    Input.Placeholder "Reviewers name"
                                    Input.OnChange (fun ev -> dispatch (UserFieldChanged ev.Value))
                                ]

                            Textarea.textarea
                                [
                                    Textarea.Placeholder "Review text"
                                    Textarea.OnChange (fun ev -> dispatch (CommentFieldChanged ev.Value))
                                ]
                                []
                            Button.button [ Button.OnClick (fun _ -> dispatch (HoverRating 3)) ] [ str "Dispatch HoverRating" ]
                            Button.button [ Button.OnClick (fun _ -> dispatch (SelectedRating 5)) ] [ str "Dispatch SelectedRating with 5" ]

                        ] ]
              Modal.Card.foot [ ]
                [ Button.button [ Button.Color IsSuccess; Button.OnClick (fun _ -> dispatch (SubmitReview model.Review))  ]
                    [ str "Save changes" ]
                  Button.button [ ]
                    [ str "Cancel" ] ] ] ]
