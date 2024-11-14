using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Model
{
    public class Room
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid? FacilityId { get; set; }
        public string? Title { get; set; }
        public string? Img { get; set; }
    }
}