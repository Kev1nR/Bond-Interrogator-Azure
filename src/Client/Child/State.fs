module Child.State

open Child.Types
open Shared
open Elmish

let init () : Model =
    let initialModel = {Name = "Review1"; Value = 0 }
    initialModel

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | SimpleMessage ->
        printfn "No change just notifying that %s has value %d" currentModel.Name currentModel.Value
        currentModel, Cmd.none
    | ValueMessage v ->
        let newValue = currentModel.Value + v
        let nextModel = { currentModel with Value = newValue }
        printfn "Changing model to %A" nextModel
        nextModel, Cmd.none
