using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public string? CommentContent { get; set; }

        [ForeignKey("StoryPostNav")]
        public int? Id { get; set; }
        public StoryPost? StoryPostNav { get; set; }
    }
}
