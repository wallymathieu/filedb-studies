using System;
using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core;

public record Order(int Id, Customer Customer, DateTime OrderDate, IEnumerable<Product> Products, int Version);