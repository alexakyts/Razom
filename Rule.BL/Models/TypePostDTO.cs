using System.ComponentModel.DataAnnotations;

namespace Rule.BL.Models
{
    public class TypePostDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Type { get; set; }
    }
}
