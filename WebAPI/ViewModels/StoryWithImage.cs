namespace WebAPI.ViewModels
{
    public class StoryWithImage
    {
        public int PId { get; set; }
        public string PTitle { get; set; }
        public string PDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Like { get; set; }
        public string ImageBase64 { get; set; }

        public string ImageBase64PP { get; set; }
        public string HCUserId { get; set; }
        public string HCUserName { get; set; }

        
    }
}
