module Hw6Client.Program 
open System
open System.Net.Http

let getAsync (client:HttpClient) (url:string) = 
    async {
        let! response = client.GetAsync(url) |> Async.AwaitTask
        response.EnsureSuccessStatusCode () |> ignore
        let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        return content
    }

let convertOperation (op: string) =
    match op with
    | "+" -> "Plus"
    | "-" -> "Minus"
    | "/" -> "Divide"
    | "*" -> "Multiply"
    | _ -> "Default"
 
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]  
[<EntryPoint>]
let main args =
        while true do 
            try 
               let args =  Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries)
               if not(args.Length = 3) then raise (ArgumentException("Wrong input."))
               let operation = convertOperation args[1]
               use httpClient = new HttpClient()
               let url = $"http://localhost:5000/calculate?value1={args[0]}&operation={operation}&value2={args[2]}";
               printfn $"Returned result: {getAsync httpClient url |> Async.RunSynchronously}"
            with
              | ex ->  printfn $"{ex.Message} Please try again." 
            
        0
    
