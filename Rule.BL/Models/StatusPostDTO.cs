using System.ComponentModel.DataAnnotations;

namespace Rule.BL.Models
{
    public class StatusPostDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Status { get; set; }
    }
}
