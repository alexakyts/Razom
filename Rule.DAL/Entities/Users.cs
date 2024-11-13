using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rule.DAL.Entities
{
    public class Users
    {
        [Key, Column(TypeName = "tinyint")]
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(50)]
        public required string LastName { get; set; }
        [MaxLength(100)]
        public required string Username { get; set;}
        public required int Phone { get; set;}
        public required string Email { get; set; }
        public required string Password { get; set; }

        public ICollection<Posts>? Posts { get; set; }
        public string? Pictures { get; set; }
    }
}
