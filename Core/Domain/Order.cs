using System;
using System.Collections.Generic;
using SomeBasicFileStoreApp.Core.Domain;

namespace SomeBasicFileStoreApp.Core;

public record Order(int Id, Customer Customer, DateTime OrderDate, IEnumerable<Product> Products, int Version);