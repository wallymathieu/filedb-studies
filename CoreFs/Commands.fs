namespace SomeBasicFileStoreApp.Core.Commands
open SomeBasicFileStoreApp.Core.Domain
open System

type Command = 
    | Empty
    | AddCustomerCommand of id:CustomerId * version:int * firstName:string * lastName:string
    | AddOrderCommand of id:OrderId * version:int * customer:CustomerId * orderDate:DateTime
    | AddProductCommand of id:ProductId * version:int * name:string * cost:float
    | AddProductToOrder of orderId:OrderId * productId:ProductId


module Commands=
    let handle (repository:IRepository) command=
        match command with
            | AddCustomerCommand(id=id ;version=version; firstName=firstName; lastName=lastName) -> 
                repository.Save(Entity.Customer({
                                                Id=id
                                                FirstName=firstName
                                                LastName=lastName
                                                Version=version
                                                }))
            | AddOrderCommand(id=id; version=version; customer=customer; orderDate=orderDate)-> 
                repository.Save(Entity.Order({
                                               Id=id
                                               OrderDate=orderDate
                                               Version=version
                                               Customer= customer
                                               Products=List.empty
                                             }))
            | AddProductCommand(id=id; version=version; name=name; cost=cost)-> 
                repository.Save(Entity.Product({
                                                Id=id
                                                Version=version
                                                Cost=cost
                                                Name=name
                                               }))
            | AddProductToOrder(orderId=orderId; productId=productId)->
                let order = repository.GetOrder(orderId)
                let product = repository.GetProduct(productId)
                repository.Save(Entity.Order({order with Products= product :: order.Products}))
            | Empty -> ()


type HandleCommand = delegate of Command -> unit

