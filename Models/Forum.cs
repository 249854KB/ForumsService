using System.ComponentModel.DataAnnotations;

namespace ForumsService.Models
{
    public class Forum
    {
        [Key]
         [Required]
        public int Id { get; set; }
         [Required]
        public string HowTo { get; set; }
         [Required]
        public string CommandLine { get; set; }
         [Required]
        public int UserId{ get; set; }
        public User User { get; set; }
    }
}