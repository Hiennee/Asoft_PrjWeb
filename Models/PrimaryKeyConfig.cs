using System;
using System.Collections.Generic;

namespace TestPrj3.Models;

public partial class PrimaryKeyConfig
{
    public int Id { get; set; }

    public string TableName { get; set; } = null!;

    public string Format { get; set; } = null!;

    public string Symbol1 { get; set; } = null!;

    public string Symbol2 { get; set; } = null!;

    public string Symbol3 { get; set; } = null!;

    public int NumberLength { get; set; }

    public int LastKey { get; set; }
}
