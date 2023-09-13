using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryAPI.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? MembershipStatus { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
    [JsonIgnore]
    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();
    [JsonIgnore]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
