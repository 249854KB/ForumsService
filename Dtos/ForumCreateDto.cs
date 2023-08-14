using System.ComponentModel.DataAnnotations;

namespace ForumsService.Dtos
{
    public class ForumCreateDto
    {
        [Required]
        public string HowTo { get; set; }

        [Required]
        public string CommandLine { get; set; }
    }
}