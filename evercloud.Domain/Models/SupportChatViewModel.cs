using System.Collections.Generic;

namespace evercloud.Domain.Models
{
    public class SupportChatViewModel
    {
        public Users SelectedUser { get; set; }
        public List<SupportMessage> Messages { get; set; }
    }
}
