using System;
using System.Collections.Generic;

namespace AdminPanel.ModelsDb;

public partial class PictureProduct
{
    public int Id { get; set; }

    public int IdProduct { get; set; }

    public int PicturePosition { get; set; }

    public string PicturePath { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
