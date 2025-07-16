namespace evercloud.ViewModels
{
    public class AdminInboxUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool HasUnread { get; set; }
    }

}
