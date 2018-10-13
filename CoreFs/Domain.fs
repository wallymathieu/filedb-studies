namespace SomeBasicFileStoreApp
open System
open FSharpPlus

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

type Entity=
    | Customer of Customer
    | Product of Product
    | Order of Order

type IRepository=
    abstract member GetCustomer: int->Customer option
    abstract member GetProduct: int->Product option
    abstract member GetOrder: int->Order option
    abstract member GetCustomers: unit -> Customer list
    abstract member GetOrders: unit -> Order list
    abstract member GetProducts: unit -> Product list
    abstract member Save: Entity->unit

open System.Collections.Concurrent

type Repository()=
    let _customers = new ConcurrentDictionary<int, Customer>()
    let _products = new ConcurrentDictionary<int, Product>()
    let _orders = new ConcurrentDictionary<int, Order>()

    interface IRepository with
        member this.GetCustomer id = Dict.tryGetValue id _customers
        member this.GetProduct id = Dict.tryGetValue id _products
        member this.GetOrder id = Dict.tryGetValue id _orders
        member this.GetCustomers () = _customers.Values |> Seq.toList
        member this.GetProducts ()= _products.Values |> Seq.toList
        member this.GetOrders ()= _orders.Values |> Seq.toList
        member this.Save entity=
            match entity with
                | Customer c -> _customers.[c.Id] <- c
                | Order o -> _orders.[o.Id] <- o
                | Product p -> _products.[p.Id] <- p

 