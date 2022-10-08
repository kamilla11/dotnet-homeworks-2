module Hw4.Parser

open System
open System.Collections.Generic
open Hw4.Calculator

type CalcOptions =
    { arg1: float
      arg2: float
      operation: CalculatorOperation }

let isArgLengthSupported (args: string []) =
    let maxArgLength = 3
    args.Length = maxArgLength

let parseOperation (arg: string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> raise (ArgumentException "Incorrect operation!")

let parseArgument (value: string) =
    match Double.TryParse(value) with
    | true, arg -> arg
    | _ -> raise (ArgumentException "Incorrect argument!")

let parseCalcArguments (args: string []) =

    if not (isArgLengthSupported (args)) then
        raise (ArgumentException "Incorrect data. Length more than 3.")

    let val1 = parseArgument args[0]
    let val2 = parseArgument args[2]
    let operation = parseOperation args[1]

    { arg1 = val1
      arg2 = val2
      operation = operation }