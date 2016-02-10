namespace SomeBasicFileStoreApp
open System

type Customer = {Id:int; FirstName:string ; LastName:string; Version:int}

type Product = {Id:int; Cost:decimal; Name: string; Version: int}

type Order = {Id:int; Customer:Customer; OrderDate:DateTime; Products: Product list; Version: int}

type Entity=
    | Customer of Customer
    | Product of Product
    | Order of Order

type IRepository=
    abstract member GetCustomer: int->Customer
    abstract member GetProduct: int->Product
    abstract member GetOrder: int->Order

    abstract member Save: Entity->unit
    (*public interface IRepository
    {
        IEnumerable<Customer> QueryOverCustomers();
        IEnumerable<Product> QueryOverProducts();
        Customer GetTheCustomerOrder(int v);
    }
    *)

open System.Collections.Concurrent

type Repository()=
    let _customers = new ConcurrentDictionary<int, Customer>()
    let _products = new ConcurrentDictionary<int, Product>()
    let _orders = new ConcurrentDictionary<int, Order>()

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

 