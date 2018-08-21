namespace CoreFsTests

open Xunit
open System.IO
open System
open System.Collections.Generic

open SomeBasicFileStoreApp
open Helpers
open GetCommands

type PersistingEventsTests() = 
    let dbs = new List<string>()

    interface IDisposable with
        member this.Dispose()=
            dbs |> Seq.iter (fun db-> File.WriteAllText(db, String.Empty) )

    [<Fact>]
    member this.Read_items_persisted_in_single_batch()=
        let commands = getCommands()
        let _persist = JsonAppendToFile("Json_CustomerDataTests_1.db" |> tee(fun db -> dbs.Add(db))) :> IAppendBatch
        _persist.Batch(commands |> unwrap);
        Assert.Equal(_persist.ReadAll() |> List.length, commands.Length);


    [<Fact>]
    member this.Read_items()=
        let commands = getCommands()
        let _persist = JsonAppendToFile("Json_CustomerDataTests_2.db" |> tee(fun db -> dbs.Add(db))) :> IAppendBatch
        _persist.Batch(commands |> unwrap);
        let allTypes ls=
            ls |> List.map( fun c -> c.GetType())
        Assert.StrictEqual(_persist.ReadAll() |> allTypes , commands |> unwrap |> allTypes)

