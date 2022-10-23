module Hw6Client.Program 
open System
open System.Net.Http
open System.Threading.Tasks

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let Path = "http://localhost:5000/"

let getAsync (client:HttpClient) (url:string) = 
    async {
        let! response = client.GetAsync(url) |> Async.AwaitTask
        response.EnsureSuccessStatusCode () |> ignore
        let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        return content
    }

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let runOnServer (url:string)=
    Task.Delay(3000) |> ignore
    async {
        use httpClient = new HttpClient()
        let! result = 
            getAsync httpClient url
        printfn "Returned result: %s" result
    }
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
[<EntryPoint>]
let main args =
    while (true) do 
        let args =  Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries)
        let url = $"{Path}calculate?value1={args[0]}&operation={args[1]}&value2={args[2]}";
        runOnServer url
        |> Async.RunSynchronously

    0