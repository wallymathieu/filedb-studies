module WebFs.GraphQL
open System
open SomeBasicFileStoreApp
open FSharp.Data.GraphQL
open FSharp.Data.GraphQL.Types
open FSharp.Data.GraphQL.Execution

let CustomerType =
  Define.Object<Customer>(
    name = "Customer",
    description = "A customer",
    isTypeOf = (fun o -> o :? Customer),
    fieldsFn = fun () ->
    [
        Define.Field("id", Int, "The id of the customer.", fun _ h -> Customer.id h)
        Define.Field("FirstName", String, "First name.", fun _ h -> h.FirstName)
        Define.Field("LastName", String, "Last name.",fun _ h -> h.LastName)
    ])
let ProductType =
  Define.Object<Product>(
    name = "Product",
    description = "A product",
    isTypeOf = (fun o -> o :? Product),
    fieldsFn = fun () ->
    [
        Define.Field("id", Int, "The id of the product.", fun _ h -> Product.id h)
        Define.Field("Name", String, "Name.", fun _ h ->Product.name h)
        Define.Field("Cost", Float, "Cost.",fun _ h -> float <| Product.cost h)
    ])
let OrderType =
  Define.Object<Order>(
    name = "Order",
    description = "An order",
    isTypeOf = (fun o -> o :? Order),
    fieldsFn = fun () ->
    [
        Define.Field("id", Int, "The id of the product.", fun _ h -> Order.id h)
        Define.Field("products", ListOf ProductType, "Products.",
                    fun _ h -> List.toSeq h.Products)
        Define.Field("customer", CustomerType, "Customer.", fun _ h -> h.Customer)
    ])
let query (r:IRepository)=
  Define.Object(
      "Query", [
        Define.Field(
            "order", Nullable OrderType, "Gets order",
            [ Define.Input("id", String) ],
            fun ctx () -> ctx.Arg("id") |> r.GetOrder)
        Define.Field(
            "product", Nullable ProductType, "Gets product",
            [ Define.Input("id", String) ],
            fun ctx () -> ctx.Arg("id") |> r.GetProduct)
        Define.Field(
            "customer", Nullable CustomerType, "Gets product",
            [ Define.Input("id", String) ],
            fun ctx () -> ctx.Arg("id") |> r.GetCustomer)
      ])

let mutation (handle:(Command->bool)) (now:unit->DateTime)=
  Define.Object(
    "Mutation", [
        Define.Field(
            "addCustomer", Boolean, "Add customer. Returns true if successful",
            [ Define.Input("id", String); Define.Input("firstname", String); Define.Input("lastname", String)],
            fun ctx () -> 
                handle <| AddCustomerCommand ((ctx.Arg "id"), 0, (ctx.Arg "firstname"), (ctx.Arg "lastname")))
        Define.Field(
            "addProduct", Boolean, "Add product. Returns true if successful",
            [ Define.Input("id", String); Define.Input("name", String); Define.Input("cost", Float) ],
            fun ctx () ->
                let cost=ctx.Arg "cost" |> decimal
                handle <| AddProductCommand ((ctx.Arg "id"), 0, (ctx.Arg "name"), cost))
        Define.Field(
            "addOrder", Boolean, "Add order. Returns true if successful",
            [ Define.Input("id", String); Define.Input("customer", Int); ],
            fun ctx () ->
                handle <| AddOrderCommand ((ctx.Arg "id"), 0, (ctx.Arg "customer"), now()))
        Define.Field(
            "addProductToOrder", Boolean, "Add product. Returns true if successful",
            [ Define.Input("orderId", Int); Define.Input("productId", Int) ],
            fun ctx () ->
                handle <| AddProductToOrder ((ctx.Arg "orderId"), (ctx.Arg "productId")))
    ])
let schema r handle now=
    let mutation=mutation handle now 
    Schema(query = query r, mutation=mutation, config = { SchemaConfig.Default with Types = [ OrderType; ProductType; CustomerType ]})
