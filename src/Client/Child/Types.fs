module Child.Types

open Shared
type Model =
    {
        FilmName: string
        Review: Review
    }

type Msg =
    | HoverRating of int
    | SelectedRating of int
    | UserFieldChanged of string
    | CommentFieldChanged of string
    | SubmitReview of Review
    | CancelReview
