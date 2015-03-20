using ProtoBuf.Meta;

namespace SomeBasicFileStoreApp.Core.Infrastructure.ProtoBuf
{
    public class Model
    {
        static Model()
        {
            var model = RuntimeTypeModel.Default;
            model[typeof(Customer)]
                .Add(1, "Id")
                .Add(2, "Version")
                .Add(3, "Firstname")
                .Add(4, "Lastname")
                ;

            model[typeof(Product)]
                .Add(1, "Id")
                .Add(2, "Version")
                .Add(3, "Cost")
                .Add(4, "Name")
                ;

            model[typeof(Order)]
                .Add(1, "Id")
                .Add(2, "Version")
                .Add(3, "Customer")
                .Add(4, "OrderDate")
                .Add(5, "Products")
                ;
        }

    }
}
