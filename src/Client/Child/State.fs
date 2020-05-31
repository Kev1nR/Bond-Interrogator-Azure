module Child.State

open Child.Types
open Shared
open Elmish

let init film =
    let initialModel =
        {
            FilmName = film.Title
            Review =
                {
                    SequenceId = film.SequenceId
                    Rating = 0
                    Who = "Kevin"
                    Comment = "Great film"
                    PostedDate = System.DateTime.Now
                }
        }
    //{Name = "Review1"; Value = 0 }
    initialModel

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | SimpleMessage ->
        printfn "No change just notifying that %s has value %d" currentModel.FilmName currentModel.Review.Rating
        currentModel, Cmd.none
    | ValueMessage v ->
        let newReview = { currentModel.Review with Rating = v }
        let nextModel = { currentModel with Review = newReview }
        printfn "Changing model to %A" nextModel

        nextModel, Cmd.none
