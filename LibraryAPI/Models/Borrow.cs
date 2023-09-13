using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryAPI.Models;

public partial class Borrow
{
    public int BorrowId { get; set; }

    public int? BookId { get; set; }

    public int? MemberId { get; set; }

    public DateTime? BorrowDate { get; set; }

    public DateTime? DueReturnDate { get; set; }

    public DateTime? ActualReturnDate { get; set; }
    [JsonIgnore]
    public virtual Book? Book { get; set; }
    [JsonIgnore]
    public virtual Member? Member { get; set; }
}
