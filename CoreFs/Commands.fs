namespace SomeBasicFileStoreApp
open System
open FSharpPlus

type Command = 
    | Empty
    | AddCustomerCommand of id:int * version:int * firstName:string * lastName:string
    | AddOrderCommand of id:int * version:int * customer:int * orderDate:DateTime
    | AddProductCommand of id:int * version:int * name:string * cost:decimal
    | AddProductToOrder of orderId:int * productId:int

module Command=
    let run (repository:IRepository) command=
        match command with
            | AddCustomerCommand(id=id ;version=version; firstName=firstName; lastName=lastName) -> 
                repository.Save(Entity.Customer({
                                                Id=id
                                                FirstName=firstName
                                                LastName=lastName
                                                Version=version
                                                }))
                true
            | AddOrderCommand(id=id; version=version; customer=customer; orderDate=orderDate)-> 
                match repository.GetCustomer(customer) with
                | Some customer->                  
                  repository.Save(Entity.Order({
                                             Id=id
                                             OrderDate=orderDate
                                             Version=version
                                             Customer=customer 
                                             Products=List.empty
                                           }))
                  true
                | None -> false
            | AddProductCommand(id=id; version=version; name=name; cost=cost)-> 
                repository.Save(Entity.Product({
                                                Id=id
                                                Version=version
                                                Cost=cost
                                                Name=name
                                               }))
                true
            | AddProductToOrder(orderId=orderId; productId=productId)->
                let order = repository.GetOrder(orderId)
                let product = repository.GetProduct(productId)
                match order,product with
                | Some order, Some product ->
                    repository.Save(Entity.Order({order with Products= product :: order.Products}))
                    true
                | _, _ -> false
            | Empty -> false


