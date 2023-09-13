using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryAPI.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int? BookId { get; set; }

    public int? MemberId { get; set; }

    public DateTime? ReservationDate { get; set; }

    public DateTime? PickupDate { get; set; }
    [JsonIgnore]
    public virtual Book? Book { get; set; }
    [JsonIgnore]
    public virtual Member? Member { get; set; }
}
