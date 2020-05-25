module Child

type Model =
    {
        Name: string
        Value: int
    }

type Msg =
    | SimpleMessage
    | ValueMessage of int
