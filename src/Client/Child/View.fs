module Child.View

open Elmish
open Fable.FontAwesome
open Fable.React
open Fable.React.Props
open Thoth.Fetch
open Fulma
open Child.Types

let view (model : Child.Types.Model) (dispatch : Child.Types.Msg -> unit) =
    div []
        [
            str (sprintf "Child view : %A" model)
            Button.button [ Button.OnClick (fun _ -> dispatch SimpleMessage) ] [ str "Dispatch SimpleMessage" ]
            Button.button [ Button.OnClick (fun _ -> dispatch (ValueMessage 5)) ] [ str "Dispatch ValueMessage with 5" ]
        ]