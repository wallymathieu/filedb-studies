using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
	public class Customer 
    {
		public Customer()
		{
        }
		public virtual int Id { get; set; }
		public virtual string Firstname { get; set; }

		public virtual string Lastname { get; set; }
		
		public virtual int Version { get; set; }
	}
}
