namespace CoreFsTests
open System
open System.Collections.Generic
open System.Threading

open SomeBasicFileStoreApp
open GetCommands
open SomeBasicFileStoreApp.Core.Domain
open SomeBasicFileStoreApp.Core.Commands
module Helpers=

    let inline tee fn x = x |> fn |> ignore; x


    let unwrap commands =
        commands
            |> Array.map WithSeqenceNumber.getCommand 
            |> Array.toList 


    type FakeAppendToFile ()=
        let batches = new List<Command list>();

        interface IAppendBatch with
            member this.Batch(commands)=
                batches.Add(commands|>List.ofSeq)
                Thread.Sleep(100)
            member this.ReadAll()=
                Thread.Sleep(100)
                batches |> List.concat |> Seq.ofList

        member this.Batches()=
            batches.ToArray()


    type ObjectContainer()=
        let _fakeAppendToFile = new FakeAppendToFile()
        let _repository = new Repository()
        let _persistToFile = new PersistCommands(_fakeAppendToFile)

        let handlers (): HandleCommand list=
            [ 
               yield HandleCommand(fun c-> Commands.handle _repository c)
               if (_persistToFile.Started()) then
                yield HandleCommand(fun c-> _persistToFile.Handle(c) )
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
                hs |> List.iter (fun h-> h.Invoke(command))

            cs |> List.iter handle

        member this.BatchesPersisted()=
            _fakeAppendToFile.Batches()

        interface IDisposable with
            member this.Dispose()=
                _persistToFile.Stop()
