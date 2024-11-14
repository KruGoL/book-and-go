
namespace Backend.Model
{
    public class BookableObject
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public Guid? RoomId { get; set; }
        public string? Name {get; set;}
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int? Seats { get; set; }
    }
}