using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Content {  get; set; }    
        public string Subject { get; set; }

        public Message(IEnumerable<string> to, string content,string subject)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x=> new MailboxAddress("email",x)));
            this.Content = content;
            this.Subject = subject;


        }
    }
}
