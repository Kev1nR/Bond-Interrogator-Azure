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
                            p [] [
                                    for i in 1..5 do
                                        yield span 
                                            [ 
                                                Style [ Cursor "pointer"]
                                                OnMouseOver (fun _ -> printfn "OnMouseOver message for %d" i)
                                                OnClick (fun _ -> printfn "OnClick message for %d" i)]
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
                                ][]
                        ] ]
              Modal.Card.foot [ ]
                [ Button.button [ Button.Color IsSuccess; Button.OnClick (fun _ -> dispatch (SubmitReview model.Review))  ]
                    [ str "Save changes" ]
                  Button.button [ ]
                    [ str "Cancel" ] ] ] ]
