using System;
using System.Collections.Generic;

namespace AdminPanel.ModelsDb;

public partial class Product
{
    public int Id { get; set; }

    public string Model { get; set; } = null!;

    public double Price { get; set; }

    public int Quantity { get; set; }

    public string? Description { get; set; }

    public int IdCategory { get; set; }

    public int IdManufacturer { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual Manufacturer IdManufacturerNavigation { get; set; } = null!;

    public virtual ICollection<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();

    public virtual ICollection<PictureProduct> PictureProducts { get; } = new List<PictureProduct>();
}
