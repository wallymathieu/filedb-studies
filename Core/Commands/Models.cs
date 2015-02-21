using ProtoBuf.Meta;

namespace SomeBasicFileStoreApp.Core.Commands
{
	public class Model
	{
		static Model()
		{
			var model = RuntimeTypeModel.Default;
			model[typeof(Customer)]
				.Add(1, "Id")
				.Add(2, "Firstname")
				.Add(3, "Lastname")
				.Add(4, "Orders")
				.Add(5, "Version")
				;

			model[typeof(Product)]
				.Add(1, "Id")
				.Add(2, "Cost")
				.Add(3, "Name")
				.Add(4, "Version")
				;

			model[typeof(Order)]
				.Add(1, "Id")
				.Add(2, "Customer")
				.Add(3, "OrderDate")
				.Add(4, "Products")
				.Add(5, "Version")
				;
		}

	}
}
