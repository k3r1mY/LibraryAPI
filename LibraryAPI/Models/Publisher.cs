using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryAPI.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }
    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
