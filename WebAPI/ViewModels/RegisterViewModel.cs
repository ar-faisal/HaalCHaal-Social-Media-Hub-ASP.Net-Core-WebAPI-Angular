using System.ComponentModel.DataAnnotations;

namespace WebAPI.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? ProfilePicPath { get; set; }
    }
}
