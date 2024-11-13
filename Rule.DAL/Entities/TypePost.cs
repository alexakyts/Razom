using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rule.DAL.Entities
{
    public class TypePost
    {
        [Key, Column(TypeName = "tinyint")]
        public int Id { get; set; }
        [MaxLength(100)]
        public required string Type { get; set; }
    }
}
