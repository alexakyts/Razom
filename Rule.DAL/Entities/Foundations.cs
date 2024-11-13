using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rule.DAL.Entities
{
    public class Foundations
    {
        [Key, Column(TypeName = "tinyint")]
        public int Id { get; set; }
        [MaxLength(200)]
        public required string Name { get; set; }  
        [MaxLength(1500)]
        public required string Description { get; set; }
        public required string Link { get; set; }
        public required string SourceLink { get; set; }

        public string? Pictures { get; set; }
    }
}
