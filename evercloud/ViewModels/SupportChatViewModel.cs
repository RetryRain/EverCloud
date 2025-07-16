using evercloud.Domain.Models;
using System.Collections.Generic;

namespace evercloud.ViewModels
{
    public class SupportChatViewModel
    {
        public Users SelectedUser { get; set; }
        public List<SupportMessage> Messages { get; set; }
    }
}
