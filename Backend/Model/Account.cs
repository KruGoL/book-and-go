
using System.ComponentModel.DataAnnotations.Schema;



namespace Backend.Model
{
    public class Account
    {      
        [Column("id")] 
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("firstName")] 
        public string? FirstName { get; set; }
        [Column("lastName")] 
        public string? LastName { get; set; }
        [Column("address")] 
        public string? Address { get; set; }
        [Column("city")] 
        public string? City { get; set; }
        [Column("postCode")] 
        public string? PostCode { get; set; }
        [Column("phoneNumber")] 
        public string? PhoneNumber { get; set; }
        [Column("email")] 
        public string? Email { get; set; }
        [Column("about")] 
        public string? About { get; set; }
    }
    /*    id: string;
    firstName?: string;
    lastName?: string;
    address?: string;
    city?:string;
    email?: string;
    phoneNumber?: string;
    postCode?: string;
    about?:string;*/
}