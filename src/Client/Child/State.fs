module Child.State

open Child.Types
open Shared
open Elmish

let init () : Model * Cmd<Msg> =
    let initialModel = {Name = "Review1"; Value = 0 }
    initialModel, Cmd.none

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | SimpleMessage -> 
        printfn "No change just notifying that %s has value %d" currentModel.Name currentModel.Value
        currentModel, Cmd.none
    | ValueMessage v -> 
        printfn "Changing %s to have value %d" currentModel.Name v
        { currentModel with Value = v }, Cmd.none
