namespace CoreFsTests
open System
open System.IO
open System.Collections.Generic
open FSharp.Data
type TestData = XmlProvider<"../Tests/TestData.xml", Global=false>
open SomeBasicFileStoreApp.Core.Commands
open SomeBasicFileStoreApp.Core.Domain

module GetCommands=
    type WithSeqenceNumber={SequenceNumber:int64; Command:Command}
        with static member getCommand v = v.Command

    [<CompiledName("Get")>]
    let getCommands ()=
        let sequence = ref 0L
        let sequence_next ()=
            sequence:=!sequence+1L
            !sequence
        let wrap c : WithSeqenceNumber=
            { SequenceNumber=sequence_next(); Command= c}

        let toAddCustomer (o : TestData.Customer)=
            wrap(AddCustomerCommand(id=CustomerId(o.Id),version=o.Version,firstName=o.Firstname,lastName=o.Lastname))

        let toAddOrder (o : TestData.Order)=
            wrap(AddOrderCommand(id=OrderId(o.Id),version=o.Version,customer=CustomerId(o.Customer), orderDate=o.OrderDate))

        let toProduct (o : TestData.Product)=
            wrap( AddProductCommand(id=ProductId(o.Id),version=o.Version,name=o.Name,cost=float(o.Cost)))

        let toOrderProduct(o : TestData.OrderProduct)=
            wrap(AddProductToOrder(productId=ProductId(o.Product), orderId=OrderId(o.Order)))

        use f = File.Open("TestData.xml", FileMode.Open, FileAccess.Read, FileShare.Read)
        let db = TestData.Load(f)

        [|
            yield! db.Customers |> Array.map toAddCustomer 
            yield! db.Orders  |> Array.map toAddOrder
            yield! db.Products |> Array.map toProduct
            yield! db.OrderProducts |> Array.map toOrderProduct
        |]

