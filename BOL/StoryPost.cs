using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
        [Table("StoryPost")]
        public class StoryPost
    {
        [Key]
        public int PId { get; set; }

        

        [Required]
        public string? PTitle { get; set; }
        [Required]
        public string? PDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Like { get; set; }


        [ForeignKey("HCUserNav")]
        public string? HCUserId { get; set; }
        public HCUser? HCUserNav { get; set; }

        public IEnumerable<Comment>? Comments { get; set; }
    }
}
