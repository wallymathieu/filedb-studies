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

        public int Id { get; }

        public float Cost { get; }

        public string Name { get; }

        public int Version { get; }
    }
}
