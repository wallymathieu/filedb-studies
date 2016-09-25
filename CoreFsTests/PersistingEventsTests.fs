namespace CoreFsTests

open NUnit.Framework
open System.IO
open System
open System.Collections.Generic

open SomeBasicFileStoreApp
open Helpers
open GetCommands

[<TestFixture>]
type PersistingEventsTests() = 
    let dbs = new List<string>()

    [<TearDown>]
    member this.TearDown()=
        dbs |> Seq.iter (fun db-> File.WriteAllText(db, String.Empty) )

    [<Test>]
    member this.Read_items_persisted_in_single_batch()=
        let commands = getCommands()
        let _persist = JsonAppendToFile("Json_CustomerDataTests_1.db" |> tee(fun db -> dbs.Add(db))) :> IAppendBatch
        _persist.Batch(commands |> unwrap);
        Assert.That(_persist.ReadAll() |> Seq.length, Is.EqualTo(commands.Length));


    [<Test>]
    member this.Read_items()=
        let commands = getCommands()
        let _persist = JsonAppendToFile("Json_CustomerDataTests_2.db" |> tee(fun db -> dbs.Add(db))) :> IAppendBatch
        _persist.Batch(commands |> unwrap);
        let allTypes ls=
            ls |> Seq.map( fun c -> c.GetType())
        Assert.That(_persist.ReadAll() |> allTypes , Is.EquivalentTo(commands |> unwrap |> allTypes));

