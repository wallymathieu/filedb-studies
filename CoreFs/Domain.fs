namespace SomeBasicFileStoreApp.Core.Domain
open System
open Saithe
open Newtonsoft.Json
open System.ComponentModel
[<TypeConverter(typeof<ValueTypeConverter<CustomerId>>)>]
[<JsonConverter(typeof<ValueTypeJsonConverter<CustomerId>>)>]
type CustomerId=
    struct
        val Value:int
    end
    with 
        new(value:int)={ Value=value }

[<TypeConverter(typeof<ValueTypeConverter<ProductId>>)>]
[<JsonConverter(typeof<ValueTypeJsonConverter<ProductId>>)>]
type ProductId=
    struct
        val Value:int
    end
    with 
        new(value:int)={ Value=value }

[<TypeConverter(typeof<ValueTypeConverter<OrderId>>)>]
[<JsonConverter(typeof<ValueTypeJsonConverter<OrderId>>)>]
type OrderId=
    struct
        val Value:int
    end
    with 
        new(value:int)={ Value=value }

type Customer = {Id:CustomerId; FirstName:string ; LastName:string; Version:int}

type Product = {Id:ProductId; Cost:float; Name: string; Version: int}

type Order = {Id:OrderId; Customer:CustomerId; OrderDate:DateTime; Products: Product list; Version: int}
    with 
        static member Create(id:OrderId, customer:CustomerId, orderDate:DateTime, products:Product array, version:int)=
           {Id=id; Customer=customer; OrderDate=orderDate; Products= products |> Array.toList; Version=version}

type Entity=
    | Customer of Customer
    | Product of Product
    | Order of Order

open System.Collections.Generic
type IRepository=
    abstract member GetCustomer: CustomerId->Customer
    abstract member GetProduct: ProductId->Product
    abstract member GetOrder: OrderId->Order

    abstract member Save: Entity->unit

    abstract member QueryOverCustomers : unit->IEnumerable<Customer>
    abstract member QueryOverProducts : unit->IEnumerable<Product>
    abstract member GetTheCustomerOrder: OrderId->Customer

open System.Runtime.CompilerServices
[<Extension>]
type Repositories =   
    [<Extension>]
    static member Save(s:IRepository, customer: Customer) = s.Save(Entity.Customer(customer))
    [<Extension>]
    static member Save(s:IRepository, order: Order) = s.Save(Entity.Order(order))
    [<Extension>]
    static member Save(s:IRepository, product: Product) = s.Save(Entity.Product(product))

open System.Collections.Concurrent

type Repository()=
    let _customers = new ConcurrentDictionary<CustomerId, Customer>()
    let _products = new ConcurrentDictionary<ProductId, Product>()
    let _orders = new ConcurrentDictionary<OrderId, Order>()

    interface IRepository with
        member this.GetCustomer id=
            _customers.[id]
        member this.GetProduct id=
            _products.[id]
        member this.GetOrder id=
            _orders.[id]
        member this.Save entity=
            match entity with
                | Customer c -> _customers.[c.Id] <- c
                | Order o -> _orders.[o.Id] <- o
                | Product p -> _products.[p.Id] <- p
        member this.QueryOverCustomers ()= _customers.Values :> IEnumerable<Customer>
        member this.QueryOverProducts ()= _products.Values :> IEnumerable<Product>
        member this.GetTheCustomerOrder v= _customers.[_orders.[v].Customer]

