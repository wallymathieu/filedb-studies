// Learn more about F# at http://fsharp.org

open System
open Suave
open SomeBasicFileStoreApp
open WebFs

type CmdArgs = 
  { IP : System.Net.IPAddress
    Port : Sockets.Port
    Json : string option
  }

open FSharpPlus

[<EntryPoint>]
let main argv =
  let args = 
    let (|Port|_|) : _-> UInt16 option = tryParse
    let (|IPAddress|_|) :_->System.Net.IPAddress option= tryParse
    
    //default bind to 127.0.0.1:8083
    let defaultArgs = 
      { IP = System.Net.IPAddress.Loopback
        Port = 8083us
        Json = None
      }
    
    let rec parseArgs b args = 
      match args with
      | [] -> b
      | "--ip" :: IPAddress ip :: xs -> parseArgs { b with IP = ip } xs
      | "--port" :: Port p :: xs -> parseArgs { b with Port = p } xs
      | "--json" :: file :: xs -> parseArgs { b with Json = Some file } xs
      | invalidArgs -> 
        printfn "error: invalid arguments %A" invalidArgs
        printfn "Usage:"
        printfn "    --ip ADDRESS   ip address (Default: %O)" defaultArgs.IP
        printfn "    --port PORT    port (Default: %i)" defaultArgs.Port
        exit 1

    argv
    |> List.ofArray
    |> parseArgs defaultArgs
    
  let r = Repository() :> IRepository
  let now = fun ()->DateTime.UtcNow
  let createJsonAppend path =
    let appender = JsonAppendToFile(path)
    let persister = CommandPersister(appender)
    fun c -> persister.Handle c 
  let handlers = [ Some <| fun c-> Command.run r c
                   Option.map createJsonAppend args.Json ] 
                 |> List.choose id
  
  let handle = fun c->
    let hs = handlers
    let handle command = 
        hs |> List.fold (fun b h-> b && h(command)) true
    handle c
 
  let schema = GraphQL.schema r handle now
  Web.startWebServer { defaultConfig with bindings = [ HttpBinding.create HTTP args.IP args.Port ] }  schema
  0 // return an integer exit code
