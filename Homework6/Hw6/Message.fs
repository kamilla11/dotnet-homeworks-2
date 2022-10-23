module Hw6.Message

open System

type MessageType =
    | SuccessfulExecution = 0
    | WrongArgLength = 1
    | WrongArgFormat = 2
    | WrongArgFormatOperation = 3
    | DivideByZero = 4
    
type Message = {
    Type: MessageType
    MessageString: string
}
   