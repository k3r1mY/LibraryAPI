using System;
using System.Collections.Generic;

namespace LibraryAPI.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string? Name { get; set; }

    public string? Position { get; set; }

    public string? PhoneNumber { get; set; }
}
