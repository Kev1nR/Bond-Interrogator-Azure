module Rating

open Elmish
open Fable.FontAwesome
open Fable.React
open Fable.React.Props
open Thoth.Fetch
open Fulma

type Model = {HoverRating: int; SelectedRating: int}

type Msg = 
    | HoverRating of int
    | SelectedRating of int

let init () =
    {HoverRating = 0; SelectedRating = 0} 

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | HoverRating rating -> 
        printfn "Got a HoverRating message with value %d" rating
        currentModel, Cmd.none
    | SelectedRating rating -> 
        printfn "Got a SelectedRating message with value %d" rating
        currentModel, Cmd.none

let view isReadOnly maxRating (model : Model) (dispatch : Msg -> unit) =
    div []
        [
            str (sprintf "in Rating view with current model: %A" model)
        ]



    