using System;
using System.Collections.Generic;

namespace AdminPanel.ModelsDb;

public partial class Warehouse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string WorkSchedule { get; set; } = null!;

    public virtual ICollection<Shipment> Shipments { get; } = new List<Shipment>();
}
