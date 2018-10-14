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
                let notExisting = id<=0 || repository.GetCustomer id |> Option.isNone
                if notExisting then
                    let id' = if id<=0 then repository.NextCustomerId() else id
                    repository.Save({
                                     Id=id'
                                     FirstName=firstName
                                     LastName=lastName
                                     Version=version
                                    })
                    true
                else
                    false
            | AddOrderCommand(id=id; version=version; customer=customer; orderDate=orderDate)->
                let notExisting = id<=0 || repository.GetOrder id |> Option.isNone 
                match repository.GetCustomer customer, notExisting with
                | Some customer, true->
                  let id' = if id<=0 then repository.NextOrderId() else id
                  repository.Save({
                                    Id=id'
                                    OrderDate=orderDate
                                    Version=version
                                    Customer=customer 
                                    Products=List.empty
                                   })
                  true
                | _,_ -> false
            | AddProductCommand(id=id; version=version; name=name; cost=cost)->
                let notExisting = id<=0 || repository.GetProduct id |> Option.isNone
                if notExisting then 
                    let id' = if id<=0 then repository.NextOrderId() else id
                    repository.Save({
                                     Id=id'
                                     Version=version
                                     Cost=cost
                                     Name=name
                                    })
                    true
                else
                    false
            | AddProductToOrder(orderId=orderId; productId=productId)->
                let order = repository.GetOrder(orderId)
                let product = repository.GetProduct(productId)
                match order,product with
                | Some order, Some product ->
                    repository.Save({order with Products= product :: order.Products})
                    true
                | _, _ -> false
            | Empty -> false


