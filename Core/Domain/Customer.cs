using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
    public class Customer
    {
        public Customer(int id, string firstName, string lastName, int version)
        {
            Id = id;
            Firstname = firstName;
            Lastname = lastName;
            Version = version;
        }

        public virtual int Id { get; }
        public virtual string Firstname { get; }

        public virtual string Lastname { get; }

        public virtual int Version { get; }
    }
}
