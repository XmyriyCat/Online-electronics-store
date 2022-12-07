using System;
using System.Collections.Generic;

namespace AdminPanel.ModelsDb;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();
}
