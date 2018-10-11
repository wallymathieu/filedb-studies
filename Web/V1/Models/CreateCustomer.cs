using SomeBasicFileStoreApp.Core.Commands;

namespace Web.V1.Models
{
    public class CreateCustomer
    {
        public Command ToCommand()
        {
            return new AddCustomerCommand()
            {
                Id = Id,
                Firstname = Firstname ,
                Lastname =Lastname 
            };
        }

        public string Lastname { get; set; }

        public string Firstname { get; set; }

        public int Id { get; set; }
    }
}