open Hw4
open Hw4.Parser
open Hw4.Calculator

[<EntryPoint>]
let main args =
    let options = parseCalcArguments args
    printfn $"{calculate options.arg1 options.operation options.arg2}"
    0