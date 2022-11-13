namespace CoreFsTests
open System
open System.Collections.Generic
open System.Threading.Tasks

open SomeBasicFileStoreApp
open GetCommands

module Helpers=

    let inline tee fn x = x |> fn |> ignore; x

    let unwrap commands =
        commands
            |> Array.map WithSeqenceNumber.getCommand 
            |> Array.toList 

    type FakeAppendToFile ()=
        let batches = List<Command list>();

        interface IAppendBatch with
            member this.Batch(commands)=
                Task.Delay(100)
                    .ContinueWith<unit>(fun r-> batches.Add(commands))
            member this.ReadAll()=
                Task.Delay(100)
                    .ContinueWith(fun r-> batches |> List.concat)

        member this.Batches()=
            batches.ToArray()


    type ObjectContainer()=
        let _fakeAppendToFile = FakeAppendToFile()
        let _repository = Repository()
        let _persistToFile = CommandPersister(_fakeAppendToFile)

        let handlers (): (Command->bool) list=
            [ 
               yield (fun c-> Command.run _repository c)
               if _persistToFile.Started() then
                yield _persistToFile.Handle
               else
                ()
            ]
        
        member this.Boot()=
            _persistToFile.Start()

        member this.GetRepository (): IRepository=
            _repository :> IRepository

        member this.Handle cs=
            let hs = handlers()
            let handle command = 
                hs |> List.fold (fun b h-> b && h(command)) true
            cs |> List.fold (fun b h -> b && handle h) true

        member this.BatchesPersisted()=
            _fakeAppendToFile.Batches()

        interface IDisposable with
            member this.Dispose()=
                _persistToFile.Stop()
