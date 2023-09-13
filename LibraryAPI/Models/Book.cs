using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryAPI.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string? Isbn { get; set; }

    public string? Title { get; set; }

    public int? AuthorId { get; set; }

    public int? PublisherId { get; set; }

    public int? PublishYear { get; set; }

    public int? GenreId { get; set; }

    public string? AvailabilityStatus { get; set; }

    [JsonIgnore]
    public virtual Author? Author { get; set; }
    [JsonIgnore]
    public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
    [JsonIgnore]
    public virtual Genre? Genre { get; set; }
    [JsonIgnore]
    public virtual Publisher? Publisher { get; set; }
    [JsonIgnore]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
