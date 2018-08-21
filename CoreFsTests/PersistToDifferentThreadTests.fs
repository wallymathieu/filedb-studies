namespace CoreFsTests

open Xunit
open System.IO
open System
open System.Collections.Generic

open SomeBasicFileStoreApp
open Helpers
open GetCommands

type PersistToDifferentThreadTests()=
    let _container = new ObjectContainer()
    let _repository = _container.GetRepository();
    let mutable _commandsSent = null

    do
        _container.Boot();
        _commandsSent <- getCommands()
        _container.Handle(_commandsSent |> unwrap)
        (_container :> IDisposable).Dispose()

    [<Fact>]
    member this.Will_send_all_commands_to_the_different_thread()=
        let _batches = _container.BatchesPersisted();

        Assert.Equal(List.concat( _batches) |> List.length, _commandsSent |> Array.length)
