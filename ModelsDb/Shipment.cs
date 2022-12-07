using System;
using System.Collections.Generic;

namespace AdminPanel.ModelsDb;

public partial class Shipment
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public double Cost { get; set; }

    public int IdWarehouse { get; set; }

    public int IdPaymentWay { get; set; }

    public int IdDelivery { get; set; }

    public virtual Delivery IdDeliveryNavigation { get; set; } = null!;

    public virtual PaymentWay IdPaymentWayNavigation { get; set; } = null!;

    public virtual Warehouse IdWarehouseNavigation { get; set; } = null!;

    public virtual ICollection<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();
}
