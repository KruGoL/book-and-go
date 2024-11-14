using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Model
{
    public record Booking
    {
        [Column("id")] 
        public Guid Id { get; init; } = Guid.NewGuid();
        [Column("objectId")] 
        public string? ObjectId { get; set; }
        [Column("name")] 
        public string? Name { get; set; }
        [Column("phoneNumber")] 
        public string? PhoneNumber { get; set; }
        [Column("email")] 
        public string? Email { get; set; }
        [Column("bookingDate")] 
        public DateTime? BookingDate { get; set; }
        [Column("duration")] 
        public DateTime? Duration { get; set; }
        [Column("clientComment")] 
        public string? ClientComment { get; init; }
        [Column("adminComment")] 
        public string? AdminComment { get; set; }
        [Column("facilityId")] 
        public Guid? FacilityId { get; set; }
        [JsonIgnore]
        public Facility? Facility { get; set; }
    }
}