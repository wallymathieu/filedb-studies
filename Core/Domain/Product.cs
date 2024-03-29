﻿using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core.Domain;

public record Product(int Id, float Cost, string Name, int Version, IDictionary<ProductProperty,string>? Properties);

public enum ProductProperty
{
    Height,
    Length,
    Width,
    Weight,
}