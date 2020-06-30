module Review.Types

open Shared

type RatingModel = { MaxRating: int; HoverRating: int; SelectedRating: int; IsReadOnly: bool }
type Model =
    {
        FilmName: string
        Review: Review
        RatingModel: RatingModel
        Rating: Rating.Model
    }

type Msg =
    // | RatingMsg of Rating.Msg
    | UserFieldChanged of Review
    | CommentFieldChanged of Review
    | SubmitReview of Review
    | CancelReview
