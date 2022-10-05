module Hw4.Parser

open System
open Hw4.Calculator

type CalcOptions =
    { arg1: float
      arg2: float
      operation: CalculatorOperation }

let isArgLengthSupported (args: string []) = args.Length = 3

let parseOperation (arg: string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> raise (ArgumentException("Incorrect operation!"))

let parseCalcArguments (args: string []) =
    if not (isArgLengthSupported (args)) then
        raise (ArgumentException("Incorrect data. Length more than 3."))

    let couldParse, val1 =
        System.Double.TryParse(args[0])

    if not (couldParse) then
        raise (ArgumentException("Incorrect first argument!"))

    let couldParse, val2 =
        System.Double.TryParse(args[2])

    if not (couldParse) then
        raise (ArgumentException("Incorrect second argument!"))

    let val3 = parseOperation args[1]

    { arg1 = val1
      arg2 = val2
      operation = val3 }