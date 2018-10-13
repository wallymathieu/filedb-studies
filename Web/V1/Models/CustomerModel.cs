using SomeBasicFileStoreApp.Core;

namespace Web.V1.Models
{
    public class CustomerModel
    {
        public static CustomerModel Map(Customer arg)
        {
            return new CustomerModel
            {
                Firstname=arg.Name.First,
                Lastname=arg.Name.Last,
                Id=arg.Id,
            };
        }
        public int Id { get; set; }
        public string Lastname { get; set; }

        public string Firstname { get; set; }
    }
}