module WebFs.Web
open System
open Newtonsoft.Json.Serialization
open Suave
open Suave.Operators
open Newtonsoft.Json
open FSharp.Data.GraphQL.Execution
open FSharp.Data.GraphQL
let settings = JsonSerializerSettings(ContractResolver= CamelCasePropertyNamesContractResolver())
let json o = JsonConvert.SerializeObject(o, settings)


let tryParse (data:byte[]) =
    let raw = Text.Encoding.UTF8.GetString data
    if raw <> null && raw <> ""
    then
        let map = JsonConvert.DeserializeObject<Map<string,obj>>(raw)
        Map.tryFind "query" map
    else None


let setCorsHeaders =
    Writers.setHeader  "Access-Control-Allow-Origin" "*"
    >=> Writers.setHeader "Access-Control-Allow-Headers" "content-type"

let handleRequest schema : WebPart =
    fun http ->
        async {
            match tryParse http.request.rawForm with
            | Some query ->
                // at the moment parser is not parsing new lines correctly, so we need to get rid of them
                let q = (query :?> string).Trim().Replace("\r\n", " ")
                let! result = Executor(schema).AsyncExecute(q)
                let serialized = json result
                return! http |> Successful.OK serialized
            | None ->
                let! schemaResult = Executor(schema).AsyncExecute(Introspection.IntrospectionQuery)
                return! http |> Successful.OK (json schemaResult)
        }
let startWebServer config schema=
  startWebServer config (setCorsHeaders >=> (handleRequest schema) >=> Writers.setMimeType "application/json")
