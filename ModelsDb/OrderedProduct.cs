using System;
using System.Collections.Generic;

namespace AdminPanel.ModelsDb;

public partial class OrderedProduct
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public string? Description { get; set; }

    public int IdProduct { get; set; }

    public int IdShipment { get; set; }

    public int IdCustomer { get; set; }

    public virtual Customer IdCustomerNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual Shipment IdShipmentNavigation { get; set; } = null!;
}
