using SomeBasicFileStoreApp.Core.Commands;

namespace Web.V1.Models
{
    public class AddProductToOrder
    {
        public Command ToCommand(int orderId)
        {
            return new AddProductToOrderCommand()
            {
                OrderId = orderId,
                ProductId =ProductId  
            };
        }

        public int ProductId { get; set; }
    }
}