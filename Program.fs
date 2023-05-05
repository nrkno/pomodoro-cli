open System
open System.IO
open System.Text.Json

let path = "endOfPomodoro.json"

let setEndOfPomodoro (endOfPomodoro: DateTime) =
    let endOfPomodoroString = JsonSerializer.Serialize endOfPomodoro
    let endOfPomodoroWithNewLine = endOfPomodoroString + "\n"
    File.WriteAllText (path, endOfPomodoroWithNewLine)

let start () =
    DateTime.Now.AddMinutes 25
    |> setEndOfPomodoro

let break' () = setEndOfPomodoro DateTime.Now

let leftOfString (symbol: string) (leftOf: TimeSpan): string =
    leftOf.TotalMinutes
    |> ceil
    |> int
    |> sprintf "%s %d\n" symbol

let leftOfPomodoroString (leftOfPomodoro: TimeSpan): string = leftOfString "🍅" leftOfPomodoro
let leftOfBreakString (leftOfBreak: TimeSpan): string = leftOfString "☕" leftOfBreak
let isTimeLeft (left: TimeSpan): bool = left > TimeSpan 0

let endOfPomodoroString (now: DateTime) (endOfPomodoro: DateTime): string =
    let endOfBreak = endOfPomodoro.AddMinutes 5
    let leftOfBreak = endOfBreak - now

    if isTimeLeft leftOfBreak
    then leftOfBreakString leftOfBreak
    else ""

let left (): string =
    let endOfPomodoro =
        File.ReadAllText path
        |> JsonSerializer.Deserialize<DateTime>

    let now = DateTime.Now
    let leftOfPomodoro = endOfPomodoro - now

    if isTimeLeft leftOfPomodoro
    then leftOfPomodoroString leftOfPomodoro
    else endOfPomodoroString now endOfPomodoro

let startCommand () =
    start ()
    0

let leftCommand () =
    printf $"{left ()}"
    0

let breakCommand () =
    break' ()
    0

let displayHelp () =
    printfn "pom - a pomodoro command line interface"
    printfn ""
    printfn "Usage: pom <command>"
    printfn ""
    printfn "Commands:"
    printfn "    start    Start a 25 minute pomodoro, followed by a 5 minute break"
    printfn ""
    printfn "    left     Get minutes left of pomodoro. Example: \"🍅 19\"."
    printfn "             When the pomodoro is over, get minutes left of break. Example: \"☕ 3\"."
    printfn "             When the break is over, get an empty string."
    printfn ""
    printfn "    break    Start a 5 minute break"
    printfn "    help     Show help"

let helpCommand () =
    displayHelp ()
    0

let invalidCommand () =
    printfn "Invalid command"
    printfn ""
    displayHelp ()
    1

[<EntryPoint>]
let main (args: string array): int =
    let command =
        args
        |> Array.toList
        |> List.tryHead
        |> Option.defaultValue "help"

    match command with
    | "start" -> startCommand ()
    | "left" -> leftCommand ()
    | "break" -> breakCommand ()
    | "help" | "--help" | "-h" -> helpCommand ()
    | _ -> invalidCommand ()
