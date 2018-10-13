using System;
using System.Linq;
using SomeBasicFileStoreApp.Core;

namespace Web.V1.Models
{
    public class OrderModel
    {
        public static OrderModel Map(Order arg)
        {
            return new OrderModel
            {
                Id=arg.Id,
                Customer =CustomerModel.Map(arg.Customer),
                Products = arg.Products.Select(ProductModel.Map).ToArray(),
                OrderDate = arg.OrderDate
            };
        }
        public int Id { get; set; }
        
        public CustomerModel Customer { get; set; }

        public DateTime OrderDate { get;set; }

        public ProductModel[] Products { get; set;}
    }
}