

using System.Text.Json.Serialization;

namespace Backend.Model
{
    public class Facility
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string? Title { get; set; }
        public Guid? OwnerId { get; set; }
        //public List<Guid>? ManagerIdList { get; set; }
        public string? Address { get; set; }
        [JsonIgnore]
        public ICollection<Booking>? Bookings { get; set; }
    }
}