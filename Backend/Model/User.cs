using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Model
{
    [Table("user")]
    public class User
    {
        [Column("id")] public Guid Id { get; set; } = Guid.NewGuid();
        [Column("username")] public string Username { get; set; } = string.Empty;
        [Column("password")] public string Password { get; set; } = string.Empty;
        [Column("role")] public string Role { get; set; } = Roles.User.ToString();
        [JsonIgnore]
        public ICollection<Facility>? Facilities { get; set; } 
  

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Roles {
            User = 1,
            Manager = 2,
            Owner = 3
        }
    }

}
