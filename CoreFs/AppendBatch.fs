namespace SomeBasicFileStoreApp
open System
open System.Threading.Tasks
open FSharpx.Task

type IAppendBatch=
    abstract member Batch: Command list->Task<unit>
    abstract member ReadAll: unit-> Task<Command list>

open System.IO

type JsonAppendToFile(fileName)=
    interface IAppendBatch with

        member this.Batch cs= task {
            use fs = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read)
            use w = new StreamWriter(fs)
            w.WriteLine(JsonConvertCommands.serialize(cs |> List.toArray))
            do! fs.FlushAsync().ContinueWith<unit>( fun _ -> () )
        }
        member this.ReadAll ()=
            use fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            use r = new StreamReader(fs)
            seq{
                let line=ref ""
                let readLine ()=
                    line := r.ReadLine()
                    !line

                while (null <> readLine()) do
                    yield JsonConvertCommands.deserialize<Command array>(!line)
            }
            |> Seq.concat
            |> Seq.toList
            |> Task.FromResult
