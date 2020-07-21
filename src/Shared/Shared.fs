namespace Shared

type Character = { Name: string; Actor: string; ImageURI: string option }

type RatingSummary = {
    AverageRating: int
    NumReviews: int
}

type Review = {
    SequenceId: int
    Rating: int
    Who: string
    Comment: string
    PostedDate: System.DateTime
}

type FilmReviews = {
    RatingSummary: RatingSummary
    Reviews: Review list
}

type BondFilm = {
    SequenceId: int
    Title: string
    Synopsis: string
    Bond: Character option
    M: Character option
    Q: Character option
    TheEnemy: Character list
    TheGirls: Character list
    Reviews: FilmReviews
}