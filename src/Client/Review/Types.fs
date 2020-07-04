module Review.Types

open Shared

//type RatingModel = { MaxRating: int; HoverRating: int; SelectedRating: int; IsReadOnly: bool }
type Model =
    {
        Review: Review
        RatingModel: Rating.Model
    }

type Msg =
    | RatingMsg of Rating.Msg
    | ContentChanged of Review
    | SubmitReview of Review
    | CancelReview
