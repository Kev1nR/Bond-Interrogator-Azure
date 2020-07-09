module Rating

open Elmish
open Fable.FontAwesome
open Fable.React
open Fable.React.Props
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
        { currentModel with HoverRating = rating }, Cmd.none
    | SelectedRating rating ->
        printfn "Got a SelectedRating message with value %d" rating
        { currentModel with SelectedRating = rating }, Cmd.none

let view isReadOnly maxRating (model : Model) (dispatch : Msg -> unit) =
    div [ Style [ Float "left" ] ]
        [
            for i in 1..maxRating do
                yield span
                    [
                        Style [ Cursor "pointer"]
                        OnMouseOver (fun _ -> dispatch (HoverRating i))
                        OnMouseLeave (fun _ -> dispatch (HoverRating 0))
                        OnClick (fun _ -> dispatch (SelectedRating i))
                    ]
                    [
                        Icon.icon [ Icon.Option.Modifiers [ Modifier.TextColor IsWarning ] ]
                            [ Fa.i [ (if i <= System.Math.Max (model.SelectedRating, model.HoverRating)
                                      then Fa.Solid.Star
                                      else Fa.Regular.Star) ] [ ] ]
                ]
        ]

let fiveStarRater = view false 5
