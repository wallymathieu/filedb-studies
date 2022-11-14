namespace SomeBasicFileStoreApp.Core;

public record Customer(int Id, Names Name, int Version)
{
    public static Customer Create(int id, string firstName, string lastName, int version)
    {
        return new Customer(
            Id: id,
            Name: new Names(firstName,lastName),
            Version: version);
    }
}

public record Names(string First, string Last);