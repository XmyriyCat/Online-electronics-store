using System;
using System.Collections.Generic;

namespace AdminPanel.ModelsDb;

public partial class Manager
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public string Salt { get; set; } = null!;
}
