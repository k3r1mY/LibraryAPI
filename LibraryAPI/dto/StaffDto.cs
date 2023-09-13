using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.dto
{
    public class StaffDto 
    {
        public int StaffId { get; set; }

        public string? Name { get; set; }

        public string? Position { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
