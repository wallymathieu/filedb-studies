namespace SomeBasicFileStoreApp
open System.Threading.Tasks
open FSharp.Control.Tasks.V2

type IAppendBatch=
    abstract member Batch: Command list->Task<unit>
    abstract member ReadAll: unit-> Task<Command list>

open System.IO
open Json.Commands
type JsonAppendToFile(fileName)=
    interface IAppendBatch with

        member this.Batch cs= task {
            use fs = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read)
            use w = new StreamWriter(fs)
            w.WriteLine(serialize(cs |> List.toArray))
            do! fs.FlushAsync().ContinueWith<unit>( fun _ -> () )
        }
        member this.ReadAll ()=
            use fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            use r = new StreamReader(fs)
            seq{
                let line=ref ""
                let readLine ()=
                    line.Value <- r.ReadLine()
                    line.Value

                while (null <> readLine()) do
                    yield deserialize(line.Value)
            }
            |> Seq.concat
            |> Seq.toList
            |> Task.FromResult
