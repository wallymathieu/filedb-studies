using SomeBasicFileStoreApp.Core;

namespace Web.V1.Models
{
    public class ProductModel
    {
        public static ProductModel Map(Product arg)
        {
            return new ProductModel()
            {
                Name = arg.Name,
                Id = arg.Id,
                Cost = arg.Cost
            };
        }
        public int Id { get; set; }

        public float Cost { get; set; }

        public string Name { get; set; }

    }
}