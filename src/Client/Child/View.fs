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
                    [ str "Modal title" ]
                  Delete.delete [ Delete.OnClick (fun _ -> dispatch CancelReview) ] [ ] ]
              Modal.Card.body [ ]
                [
                    div []
                        [
                            str (sprintf "Child view : %A" model)
                            Button.button [ Button.OnClick (fun _ -> dispatch (HoverRating 3)) ] [ str "Dispatch HoverRating" ]
                            Button.button [ Button.OnClick (fun _ -> dispatch (SelectedRating 5)) ] [ str "Dispatch SelectedRating with 5" ]
                            Button.button [ Button.OnClick (fun _ -> dispatch (SubmitReview model.Review)) ] [ str "Dispatch Submit review" ]
                        ] ]
              Modal.Card.foot [ ]
                [ Button.button [ Button.Color IsSuccess ]
                    [ str "Save changes" ]
                  Button.button [ ]
                    [ str "Cancel" ] ] ] ]
