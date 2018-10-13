using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
    public class Customer
    {
        public Customer(int id, string firstName, string lastName, int version)
        {
            Id = id;
            Name=new Names(firstName,lastName);
            Version = version;
        }

        public int Id { get; }
        
        public int Version { get; }
        
        public Names Name { get; }
    }

    public class Names
    {
        public Names(string first, string last)
        {
            First = first;
            Last = last;
        }

        /// <summary>
        /// First name
        /// </summary>
        public string First { get; }
        /// <summary>
        /// Last name
        /// </summary>
        public string Last { get; }
    }
}
