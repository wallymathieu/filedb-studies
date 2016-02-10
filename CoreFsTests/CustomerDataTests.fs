namespace CoreFsTests

open NUnit.Framework
open System.IO
open System
open System.Collections.Generic

open SomeBasicFileStoreApp
open Helpers
open GetCommands

[<TestFixture>]
type CustomerDataTests()=
    let _container = new ObjectContainer()
    let _repository = _container.GetRepository();

    [<TestFixtureSetUp>]
    member this.TestFixtureSetup()=
        //_container.Boot();
        let commands = getCommands()
        _container.Handle(commands |> unwrap)

    [<TestFixtureTearDown>]
    member this.TestFixtureTearDown()=
        (_container :> IDisposable).Dispose()

    [<Test>]
    member this.CanGetCustomerById()=
        Assert.IsNotNull(_repository.GetCustomer(1))

    [<Test>]
    member this.CanGetProductById()=
        Assert.IsNotNull(_repository.GetProduct(1))

    [<Test>]
    member this.OrderContainsProduct()=
        let order = _repository.GetOrder(1)
        Assert.True(order.Products |> List.tryFind( fun p -> p.Id = 1) |> Option.isSome)

    //[<Test>]
    //member this.OrderHasACustomer()=
    //    Assert.IsNotNullOrEmpty(_repository.GetTheCustomerOrder(1).Firstname)