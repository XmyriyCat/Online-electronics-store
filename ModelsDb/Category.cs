using System;
using System.Collections.Generic;

namespace AdminPanel.ModelsDb;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
