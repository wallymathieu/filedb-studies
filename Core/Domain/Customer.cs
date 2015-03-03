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

		public virtual int Id { get; private set; }
        public virtual string Firstname { get; private set; }

        public virtual string Lastname { get; private set; }
		
        public virtual int Version { get; private set; }
	}
}
