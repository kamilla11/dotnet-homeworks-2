open System
open Hw5
open Parser
open Calculator

[<EntryPoint>]
let main args =
    let result = parseCalcArguments args

    match result with
    | Ok options -> printfn $"{options |||> calculate}"
    | Error message -> printfn $"{message}"

    0 