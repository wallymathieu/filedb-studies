namespace CoreFsTests
open System.IO
open SomeBasicFileStoreApp
open FSharp.Data
type TestData = XmlProvider<"../Tests/TestData/TestData.xml", Global=false>

module GetCommands=
    type WithSeqenceNumber={SequenceNumber:int64; Command:Command}
        with static member getCommand v = v.Command


    let getCommands ()=
        let sequence = ref 0L
        let sequence_next ()=
            sequence.Value <- sequence.Value+1L
            sequence.Value
        let wrap c : WithSeqenceNumber=
            { SequenceNumber=sequence_next(); Command= c}

        let toAddCustomer (o : TestData.Customer)=
            wrap(AddCustomerCommand(id=o.Id,version=o.Version,firstName=o.Firstname,lastName=o.Lastname))

        let toAddOrder (o : TestData.Order)=
            wrap(AddOrderCommand(id=o.Id,version=o.Version,customer=o.Customer, orderDate=o.OrderDate.Date))

        let toProduct (o : TestData.Product)=
            wrap( AddProductCommand(id=o.Id,version=o.Version,name=o.Name,cost=o.Cost))

        let toOrderProduct(o : TestData.OrderProduct)=
            wrap(AddProductToOrder(productId=o.Product, orderId=o.Order))

        use f = File.Open("TestData.xml", FileMode.Open, FileAccess.Read, FileShare.Read)
        let db = TestData.Load(f)

        [|
            yield! db.Customers |> Array.map toAddCustomer 
            yield! db.Orders  |> Array.map toAddOrder
            yield! db.Products |> Array.map toProduct
            yield! db.OrderProducts |> Array.map toOrderProduct
        |]

