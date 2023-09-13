using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryAPI.Models;

public partial class Fine
{
    public int FineId { get; set; }

    public int? MemberId { get; set; }

    public decimal? Amount { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? DatePaid { get; set; }
    [JsonIgnore]
    public virtual Member? Member { get; set; }
}
