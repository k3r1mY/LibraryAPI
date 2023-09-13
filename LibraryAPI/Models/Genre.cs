using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryAPI.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string? GenreName { get; set; }

    public string? Description { get; set; }
    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
