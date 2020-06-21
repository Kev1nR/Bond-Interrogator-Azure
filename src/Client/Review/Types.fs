module Review.Types

open Shared

type RatingModel = { MaxRating: int; HoverRating: int; SelectedRating: int; IsReadOnly: bool }
type Model =
    {
        FilmName: string
        Review: Review
        Rating: Rating.Model
    }

type Msg =
    | RatingMsg of Rating.Msg
    | UserFieldChanged of string
    | CommentFieldChanged of string
    | SubmitReview of Review
    | CancelReview
