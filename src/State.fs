module App.State

open Elmish
open Elmish.Browser.Navigation
open Types
open Fable.Import


let urlUpdate (result: Option<Navigation.Page>) model =
    match result with
    | None ->

        Browser.console.error("Error parsing url: " + Browser.window.location.href)
        model, Navigation.newUrl (Navigation.toHash model.CurrentPage)

    | Some page ->
        let model = { model with CurrentPage = page }
        match page with
        | Navigation.Question questionPage ->
            let (subModel, subCmd) = Question.Dispatcher.State.init questionPage
            { model with QuestionDispatcher = subModel }, Cmd.map QuestionDispatcherMsg subCmd

let init result =
    urlUpdate result Model.Empty

let update msg model =
    match msg with
    | QuestionDispatcherMsg msg ->
        let (subModel, subCmd) = Question.Dispatcher.State.update msg model.QuestionDispatcher
        { model with QuestionDispatcher = subModel }, Cmd.map QuestionDispatcherMsg subCmd
