module Review.Types

open Shared

type Msg =
    | UserFieldChanged of Review
    | CommentFieldChanged of Review
    | SubmitReview of Review
    | CancelReview
