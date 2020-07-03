module App

open Elmish
open Fable.FontAwesome
open Fable.React
open Fable.React.Props
open Thoth.Fetch
open Fulma

open Shared
open Fable.Core.JS

type ServerState = Idle | Loading | ServerError of string
type ModalContent = Review | Character

// The model holds data that you want to keep track of while the application is running
// in this case, we are keeping track of selection of Bond films from the dropdown selector
// The initial value will be requested from server
type Model =
    {
      ValidationError : string option
      ServerState : ServerState
      BondFilm : BondFilm option
      BondFilmList : BondFilm list option
      CurrentFilm : int option
      IsBurgerOpen : bool
      ShowModal: ModalContent option
      Review : Review option
    }

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
type Msg =
| BondFilmListLoaded of BondFilm list
| BondFilmSelected of BondFilm
| ToggleBurger
| CloseModal
| AddReview of BondFilm
| ReviewMsgHandler of Review.Types.Msg * Review //.Types.Model

let initialFilms () = Fetch.fetchAs<BondFilm list> "/api/films"
let inline pushReview review : Promise<BondFilm> = Fetch.post ("/api/add-review", review)

// defines the initial state and initial command (= side-effect) of the application
let init () : Model * Cmd<Msg> =
    let initialModel = {
        ValidationError = None;
        ServerState = Loading;
        BondFilm = None;
        BondFilmList = None;
        CurrentFilm = None;
        IsBurgerOpen = false;
        ShowModal = None
        Review = None}
    let loadBondFilmsCmd =
        Cmd.OfPromise.perform initialFilms () BondFilmListLoaded
    initialModel, loadBondFilmsCmd

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match currentModel.BondFilmList, msg with
    | _, BondFilmSelected b ->
        printf "BondFilmSelected msg"
        let nextModel = { currentModel with BondFilm = Some b; CurrentFilm = Some b.SequenceId }
        nextModel, Cmd.none
    | _, BondFilmListLoaded films ->
        let nextModel = { ValidationError = None
                          ServerState = Loading
                          BondFilm = None
                          BondFilmList = Some films
                          CurrentFilm = None
                          IsBurgerOpen = false
                          ShowModal = None
                          Review = None
                        }
        nextModel, Cmd.none
    | _, ToggleBurger -> { currentModel with IsBurgerOpen = not currentModel.IsBurgerOpen }, Cmd.none
    | _, CloseModal -> { currentModel with ShowModal = None }, Cmd.none
    | _, AddReview f ->
        let newReview, _ = Review.State.init f
        let nextModel = { currentModel with ShowModal = Some Review; Review = Some newReview }
        nextModel, Cmd.none
    | _, ReviewMsgHandler (childMsg, childModel) ->
            match childMsg with
            | Review.Types.SubmitReview r ->
                printf "Got a SubmitReview message with value %A" r
                let nextChildModel, _ = Review.State.update childMsg childModel
                printfn "Review model after submit %A" nextChildModel
                { currentModel with ShowModal = None }, Cmd.none
            | Review.Types.CancelReview ->
                printf "Got a CancelReview message"
                { currentModel with ShowModal = None }, Cmd.none
            | Review.Types.ContentChanged r ->
                printf "Got Content changed ReviewMsg %A" childMsg
                let nextChildModel, _ = Review.State.update childMsg r
                // { currentModel with ReviewModel = Some nextChildModel }, Cmd.none
                { currentModel with Review = Some nextChildModel }, Cmd.none

let safeComponents =
    let components =
        span [ ]
           [ a [ Href "https://github.com/SAFE-Stack/SAFE-template" ]
               [ str "SAFE  "
                 str Version.template ]
             str ", "
             a [ Href "https://saturnframework.github.io" ] [ str "Saturn" ]
             str ", "
             a [ Href "http://fable.io" ] [ str "Fable" ]
             str ", "
             a [ Href "https://elmish.github.io" ] [ str "Elmish" ]
             str ", "
             a [ Href "https://fulma.github.io/Fulma" ] [ str "Fulma" ]
             str ", "
             a [ Href "https://bulmatemplates.github.io/bulma-templates/" ] [ str "Bulma\u00A0Templates" ]

           ]

    span [ ]
        [ str "Version "
          strong [ ] [ str Version.app ]
          str " powered by: "
          components ]

let navBrand isBurgerOpen dispatch =
    Navbar.Brand.div [ ]
        [ Navbar.Item.a
            [ Navbar.Item.Props [] ]
            [ img [ Src "007_tranparent.png"
                    Alt "Logo" ] ]
          Navbar.burger [ Modifiers [ ]
                          CustomClass (if isBurgerOpen then "is-active" else "")
                          Props [
                            OnClick (fun _ -> dispatch ToggleBurger) ] ]
                        [ span [ ] [ ]
                          span [ ] [ ]
                          span [ ] [ ] ] ]

let navMenu isBurgerOpen =
    Navbar.menu [ Navbar.Menu.IsActive isBurgerOpen ]
        [ Navbar.End.div [ ]
            [ Navbar.Item.a [ ]
                [
                  a [   Href "https://utterlyuseless.home.blog/bond-interrogator/" ] [ str "Documentation"]
                ]
              Navbar.Item.div [ ]
                [ Button.a
                    [ Button.Color IsWhite
                      Button.IsOutlined
                      Button.Size IsSmall
                      Button.Props [ Href "https://github.com/Kev1nR/Bond-Interrogator-Azure" ] ]
                    [ Icon.icon [ ]
                        [ Fa.i [Fa.Brand.Github; Fa.FixedWidth] [] ]
                      span [ ] [ str "View Source" ] ] ] ] ]

let dropDownList (model : Model) (dispatch : Msg -> unit) =
    Box.box' [ CustomClass "cta" ]
      [ Level.level [ ]
          [ Level.item [ ]
              [
                Dropdown.dropdown [ Dropdown.IsHoverable; ]
                  [ div [ ]
                      [ Button.button [  ]
                          [ span [ ]
                              [
                                match model.BondFilm with
                                | Some film -> yield str film.Title
                                | _ -> yield str "Select film"
                              ]
                            Icon.icon [ Icon.Size IsSmall ] [ Fa.i [ Fa.Solid.AngleDown ] [ ] ]
                          ] ]
                    Dropdown.menu [ ]
                      [ Dropdown.content [  ]
                          [
                              match model.BondFilmList with
                              | Some films ->
                                  for m in films do
                                    yield Dropdown.Item.a
                                      [
                                          Dropdown.Item.IsActive (if model.CurrentFilm.IsSome then (m.SequenceId = model.CurrentFilm.Value) else false)
                                          Dropdown.Item.Props [ OnClick ( fun _ -> dispatch (BondFilmSelected m)) ]
                                      ] [str m.Title ]
                              | _ -> yield Dropdown.Item.a [ ] [str "<Empty>" ] ] ] ] ] ] ]

let characterCard filmId character =
  let imgURI = character.ImageURI |> Option.defaultValue ""
  let heading = character.Name
  let body = sprintf "Played by %s.%s is ..." character.Actor character.Name
  Column.column [ Column.Width (Screen.All, Column.Is4) ]
    [ Card.card [ ]
        [
          Card.content [ ]
            [ Content.content [ ]
                [
                    Media.media []
                        [
                            Media.left [ ]
                                [
                                    Image.image [ Image.Is64x64 ] [ img [ Src imgURI ] ]
                                ]
                            Media.content []
                                [
                                   h4 [ ] [ str heading ]
                                ]
                        ]
                    Level.level []
                        [
                            p [ ] [ str body ]
                        ]
                ] ] ] ]

let characters (model : Model) =
    let filmId, characterList = model.BondFilm |> Option.fold (fun bfCs bf -> bf.SequenceId, [ bf.Bond; bf.M; bf.Q ]) (0,[])

    let ccs = characterList
              |> Seq.choose id
              |> Seq.map (characterCard filmId)

    if ccs |> Seq.isEmpty
    then
      Columns.columns [ Columns.CustomClass "features" ] []
    else
      Columns.columns [ Columns.CustomClass "features" ]
        [
          for cc in ccs do
            yield cc
        ]

let filmInfo (model : Model) dispatch =
    Column.column
      [ Column.CustomClass "intro"
        Column.Width (Screen.All, Column.Is8)
        Column.Offset (Screen.All, Column.Is2) ]
      [ h2 [  ClassName "title" ]
          [
            yield (model.BondFilm |> Option.fold (fun _ b -> str b.Title) (str "\"Do you expect me to talk?\""))
            yield Container.container []
                    [
                        match model.BondFilm with
                        | Some bf ->
                           yield  Button.button [ Button.OnClick (fun _ -> dispatch (AddReview bf) ) ] [ str "Add review" ]
                        | None -> yield p [] []
                    ]
          ]
        p [ ClassName "subtitle"]
          [
            let subtitleContent =
                model.BondFilm
                |> Option.fold (fun _ b ->
                    div [] [
                        //Client.Components.ratingComponent b.Reviews
                        br []
                        p [] [ str b.Synopsis ] ])
                   (div [] [ str "\"No Mr. Bond, I expect you to choose a film!\"" ])

            yield subtitleContent
          ]
        characters model
      ]


let footerContainer =
    Container.container [ ]
        [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
            [ p [ ]
                [ safeComponents ]
              p [ ]
                [ a [ Href "https://github.com/SAFE-Stack/SAFE-template" ]
                    [ Icon.icon [ ]
                        [ Fa.i [Fa.Brand.Github; Fa.FixedWidth] [] ] ] ] ] ]

let view (model : Model) (dispatch : Msg -> unit) =
    div [ ]
        [ Hero.hero
            [ Hero.Color IsPrimary
              Hero.IsHalfHeight
              Hero.IsBold ]
            [ Hero.head [ ]
                [ Navbar.navbar [ ]
                    [ Container.container [ ]
                        [ navBrand (model.IsBurgerOpen) dispatch
                          navMenu (model.IsBurgerOpen)] ] ]
              Hero.body [ ]
                [ Container.container [ Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ Heading.p [ ]
                        [ str "The Bond Interrogator" ]
                      Heading.p [ Heading.IsSubtitle ]
                          [ str "A SPECTRE agent's guide to the Bond film catalogue" ] ] ] ]
          dropDownList model dispatch

          Container.container []
            [
              match model.ShowModal with
              | Some Review ->
                    printfn "Show modal for review"
                    let reviewContent =
                        match model.BondFilm, model.Review with
                        | Some bf, Some r ->
                            let title = bf.Title
                            Review.View.view title r (fun msg -> dispatch (ReviewMsgHandler (msg, r)))
                        | _ -> (div [][])

                    yield reviewContent
              | Some Character ->
                  printfn "Show modal for character"
                  yield (str "This will be character content")
              | None ->
                  printfn "Don't show modal"
            ]

          Container.container [ ]
            [ filmInfo model dispatch ]

          footer [ ClassName "footer" ]
            [ footerContainer ] ]