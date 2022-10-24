module Hw6.App
open Hw6
open Parser
open MaybeBuilder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Values

let calculatorHandler: HttpHandler =
    fun next ctx ->
        let result = maybe {
            let! values = ctx.TryBindQueryString<Values>()
            let! calculation = parseCalcArguments [|values.value1; values.operation; values.value2|]
            return calculation
        }    
        
        match result with
        | Ok ok -> (setStatusCode 200 >=> text (ok.ToString())) next ctx
        | Error error ->
            if error.Equals("DivideByZero") then
                (setStatusCode 200 >=> text error) next ctx
            else
                (setStatusCode 400 >=> text error) next ctx


    
let webApp =
    choose [
        GET >=> choose [
             route "/" >=> text "Use //calculate?value1=<Val1>&operation=<Operation>&value2=<Val2>"
             route "/calculate" >=> calculatorHandler
        ]
        setStatusCode 404 >=> text "Not Found" 
    ]
    
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseGiraffe webApp


[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run
        ()
    
    0