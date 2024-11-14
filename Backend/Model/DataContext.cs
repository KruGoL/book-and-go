using Microsoft.EntityFrameworkCore;

namespace Backend.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Account>? AccountList { get; set; }
        public DbSet<Facility>? FacilityList { get; set; }
        public DbSet<Room>? RoomList { get; set; }
        public DbSet<BookableObject>? BookableObjectList { get; set; }
        public DbSet<Booking>? BookingList { get; set; }
        public DbSet<User>? UserList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Facility>().ToTable("Facilities").HasKey(x => x.Id);
            modelBuilder.Entity<Room>().ToTable("Rooms").HasKey(x => x.Id);
            modelBuilder.Entity<BookableObject>().ToTable("Objects").HasKey(x => x.Id);
            modelBuilder.Entity<Booking>().ToTable("Bookings").HasKey(x => x.Id);
            modelBuilder.Entity<Room>().Property(x => x.Id).HasIdentityOptions(6);
            modelBuilder.Entity<Booking>().Property(x => x.Id).HasIdentityOptions(5);
            
            modelBuilder.UseIdentityColumns();

            
            modelBuilder.Entity<Facility>().HasData(
                new Facility
                {
                    Id = Guid.Parse("1ee0dede-d213-4ede-8ac1-f4ad8391da49"),
                    Title = "testFacility1",
                    OwnerId = Guid.Parse("228d0cfe-4da5-499f-9431-8ab9d89b8231"),
                    //ManagerIdList = new List<Guid>(){Guid.Parse("228d0cfe-4da5-499f-9431-8ab9d89b8302")},
                    Address = "testAddress1"
                }
            ); 

             modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = Guid.Parse("111d0cfe-4da5-499f-9431-8ab9d89b8216"),
                    FacilityId = Guid.Parse("1ee0dede-d213-4ede-8ac1-f4ad8391da49"),
                    Title = "testTitle1",
                },
                new Room
                {
                    Id = Guid.Parse("222d0cfe-4da5-499f-9431-8ab9d89b8217"),
                    FacilityId = Guid.Parse("92e0ed93-f099-485a-adf0-848d6009d23c"),
                    Title = "testTitle2",
                }
            ); 

            modelBuilder.Entity<BookableObject>().HasData(
                new BookableObject
                {
                    Id = "123d0cfe-4da5-499f-9431-8ab9d89b8200",
                    RoomId = Guid.Parse("111d0cfe-4da5-499f-9431-8ab9d89b8216"),
                    Name = "bookableObject1",
                    Width = 1,
                    Height = 1,
                    X = 10,
                    Y = 10,
                },
                new BookableObject
                {
                    Id = "234d0cfe-4da5-499f-9431-8ab9d89b8201",
                    RoomId = Guid.Parse("222d0cfe-4da5-499f-9431-8ab9d89b8217"),
                    Name = "bookableObject2",
                    Width = 1,
                    Height = 1,
                    X = 20,
                    Y = 20,
                }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = Guid.Parse("1d2950cb-100b-46f2-bbde-077d8414ac9f"),
                    ObjectId = "123d0cfe-4da5-499f-9431-8ab9d89b8200",
                    PhoneNumber = "+372 55958692",
                    Email = "ilja@gmail.com",
                    BookingDate = new DateTime(2023, 11, 12)
                },
                new Booking
                {
                    Id = Guid.Parse("2d2950cb-100b-46f2-bbde-077d8414ac9f"),
                    ObjectId = "234d0cfe-4da5-499f-9431-8ab9d89b8201",
                    PhoneNumber = "+372 55667788",
                    Email = "andrei@gmail.com",
                    BookingDate = new DateTime(2023, 11, 7)
                }
            );
        }
    }
}