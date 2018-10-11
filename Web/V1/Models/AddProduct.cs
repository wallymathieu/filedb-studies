using SomeBasicFileStoreApp.Core.Commands;

namespace Web.V1.Models
{
    public class AddProduct
    {
        public Command ToCommand()
        {
            return new AddProductCommand
            {
                Id = Id,
                Name= Name,
                Cost=Cost
            };
        }

        public int Id { get; set; }
        public float Cost { get; set; }
        public string Name { get; set; }

    }
}