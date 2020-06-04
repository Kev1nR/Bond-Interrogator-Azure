module Child.Types

open Shared

type RatingModel = { MaxRating: int; HoverRating: int; SelectedRating: int; IsReadOnly: bool }
type Model =
    {
        FilmName: string
        Review: Review
        RatingModel: RatingModel
    }

type Msg =
    | HoverRating of int
    | SelectedRating of int
    | UserFieldChanged of string
    | CommentFieldChanged of string
    | SubmitReview of Review
    | CancelReview
