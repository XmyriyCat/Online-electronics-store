using System;
using System.Collections.Generic;

namespace AdminPanel.ModelsDb;

public partial class PaymentWay
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Shipment> Shipments { get; } = new List<Shipment>();
}
