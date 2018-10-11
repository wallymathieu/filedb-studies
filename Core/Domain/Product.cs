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

        public virtual int Id { get; }

        public virtual float Cost { get; }

        public virtual string Name { get; }

        public virtual int Version { get; }
    }
}
