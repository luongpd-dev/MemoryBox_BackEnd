using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Domain.Interfaces.Email
{
    public interface ISendMail
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
