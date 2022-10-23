module Hw6Client.Program 
open System
open System.Net.Http
open System.Threading.Tasks

let getAsync (client:HttpClient) (url:string) = 
    async {
        let! response = client.GetAsync(url) |> Async.AwaitTask
        response.EnsureSuccessStatusCode () |> ignore
        let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        return content
    }

 
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]  
[<EntryPoint>]
let main args =
    while (true) do 
        let args =  Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries)
        use httpClient = new HttpClient()
        let url = $"http://localhost:5000/calculate?value1={args[0]}&operation={args[1]}&value2={args[2]}";
        printfn $"Returned result: {getAsync httpClient url |> Async.RunSynchronously}"

    0