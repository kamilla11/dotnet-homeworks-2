open Hw4

[<EntryPoint>]
let main args =
    let calc = Parser.parseCalcArguments args
    let value1 = calc.arg1
    let operation = calc.operation
    let value2 = calc.arg2

    let result =
        Calculator.calculate value1 operation value2

    printfn $"{result}"
    0
