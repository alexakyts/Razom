using Rule.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Rule.BL.Models
{
    public class PostsDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; }
        [Required, MaxLength(1500)]
        public string Description { get; set; }
        [Required]
        public int FinishSum { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }

        public int UsersId { get; set; }

        public int StatusPostId { get; set; }

        public int TypePostId { get; set; }
        [Required]
        public string Link { get; set; }
        public int? FoundationsId { get; set; }
        public string? PicturesPosts { get; set; }
    }
}
