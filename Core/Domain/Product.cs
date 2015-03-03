namespace SomeBasicFileStoreApp.Core
{
	public class Product 
    {
        public Product(int id, float cost, string name, int version)
        {
            Id = id;
            Cost = cost;
            Name = name;
            Version = version;           
        }

		public virtual int Id { get; private set; }

		public virtual float Cost { get; private set; }

		public virtual string Name { get; private set; }

		public virtual int Version { get; private set; }
	}
}
