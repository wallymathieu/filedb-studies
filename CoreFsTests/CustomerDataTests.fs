namespace CoreFsTests

open Xunit
open System.IO
open System
open System.Collections.Generic

open SomeBasicFileStoreApp
open Helpers
open GetCommands

type CustomerDataTests()=
    let _container = new ObjectContainer()
    let _repository = _container.GetRepository();

    do
        //_container.Boot();
        let commands = getCommands()
        _container.Handle(commands |> unwrap)

    [<Fact>]
    member this.CanGetCustomerById()=
        Assert.NotNull(_repository.GetCustomer(1))

    [<Fact>]
    member this.CanGetProductById()=
        Assert.NotNull(_repository.GetProduct(1))

    [<Fact>]
    member this.OrderContainsProduct()=
        let order = _repository.GetOrder(1)
        Assert.True(order.Products |> List.tryFind( fun p -> p.Id = 1) |> Option.isSome)

    //[<Test>]
    //member this.OrderHasACustomer()=
    //    Assert.IsNotNullOrEmpty(_repository.GetTheCustomerOrder(1).Firstname)