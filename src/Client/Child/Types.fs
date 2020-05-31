module Child.Types

open Shared
type Model =
    {
        FilmName: string
        Review: Review
    }

type Msg =
    | SimpleMessage
    | ValueMessage of int
