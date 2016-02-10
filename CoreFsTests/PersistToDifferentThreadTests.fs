namespace CoreFsTests

open NUnit.Framework
open System.IO
open System
open System.Collections.Generic

open SomeBasicFileStoreApp
open Helpers
open GetCommands

[<TestFixture>]
type PersistToDifferentThreadTests()=
    let _container = new ObjectContainer()
    let _repository = _container.GetRepository();
    let mutable _commandsSent = null

    [<TestFixtureSetUp>]
    member this.TestFixtureSetup()=
        _container.Boot();
        _commandsSent <- getCommands()
        _container.Handle(_commandsSent |> unwrap)
        (_container :> IDisposable).Dispose()

    [<Test>]
    member this.Will_send_all_commands_to_the_different_thread()=
        let _batches = _container.BatchesPersisted();

        Assert.That(List.concat( _batches) |> List.length, Is.EqualTo(_commandsSent |> Array.length))
