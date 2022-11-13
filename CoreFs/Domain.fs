namespace SomeBasicFileStoreApp
open System
open FSharpPlus
open CoreFs.Threading

type Customer = {Id:int; FirstName:string ; LastName:string; Version:int}
with 
    static member id (c:Customer)=c.Id

type Product = {Id:int; Cost:decimal; Name: string; Version: int}
with 
    static member id (c:Product)=c.Id
    static member name (c:Product)=c.Name
    static member cost (c:Product)=c.Cost

type Order = {Id:int; Customer:Customer; OrderDate:DateTime; Products: Product list; Version: int}
with 
    static member id (c:Order)=c.Id

type IRepository=
    abstract member GetCustomer: int->Customer option
    abstract member GetProduct: int->Product option
    abstract member GetOrder: int->Order option

    abstract member GetCustomers: unit -> Customer list
    abstract member GetOrders: unit -> Order list
    abstract member GetProducts: unit -> Product list

    abstract member Save: Customer->unit
    abstract member Save: Product->unit
    abstract member Save: Order->unit

    abstract member NextCustomerId: unit -> int
    abstract member NextOrderId: unit -> int
    abstract member NextProductId: unit -> int
open System.Collections.Concurrent

type Repository()=
    let customers = new ConcurrentDictionary<int, Customer>()
    let customerId = ThreadSafeMax 0
    let products = new ConcurrentDictionary<int, Product>()
    let productId = ThreadSafeMax 0
    let orders = ConcurrentDictionary<int, Order>()
    let orderId = ThreadSafeMax 0
    
    interface IRepository with
        member this.GetCustomer id = Dict.tryGetValue id customers
        member this.GetProduct id = Dict.tryGetValue id products
        member this.GetOrder id = Dict.tryGetValue id orders
        member this.GetCustomers () = customers.Values |> Seq.toList
        member this.GetProducts ()= products.Values |> Seq.toList
        member this.GetOrders ()= orders.Values |> Seq.toList
        member this.NextCustomerId ()= 1+customerId.Value
        member this.NextProductId ()= 1+productId.Value
        member this.NextOrderId ()= 1+orderId.Value
        member this.Save (c:Customer) = customers[customerId.Observe c.Id] <- c
        member this.Save (o:Order) = orders[orderId.Observe o.Id] <- o
        member this.Save (p:Product) = products[productId.Observe p.Id] <- p

 