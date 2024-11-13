using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rule.DAL.Entities
{
    public class Posts
    {
        [Key, Column(TypeName = "tinyint")]
        public int Id { get; set; }
        [MaxLength(200)]
        public required string Name { get; set; }
        [MaxLength(1500)]
        public required string Description { get; set; }
        public required int FinishSum {  get; set; }
        [DataType(DataType.Date)]
        public required DateTime CreationTime { get; set; }
        public int UsersId { get; set; }
        public virtual Users Users { get; set; }
        public int StatusPostId { get; set; }
        public virtual StatusPost StatusPost { get; set; }
        public int TypePostId {  get; set; }
        public virtual TypePost TypePost { get; set; }
        public required string Link { get; set; }
        public int? FoundationsId {  get; set; }
        public virtual Foundations? Foundations { get; set; }
        public string? PicturesPosts { get; set; }
    }
}
