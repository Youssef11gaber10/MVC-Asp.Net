using Demo.DAL.Models;
using System.Buffers.Text;
using System.Net;
using System.Net.Mail;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;
using System.Xml.Linq;

namespace Demo.PL.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {

            var Client = new SmtpClient("smtp.gmail.com", 587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("youssef11gaber10@gmail.com", "gvixjjmjujqgqmez");
            Client.Send("youssef11gaber10@gmail.com", email.To,email.Subject,email.Body);



        }
    }

    //SMTP Server Address: smtp.gmail.com
    //Use Authentication: yes

    //Secure Connection: TLS / SSL based on your mail client / website SMTP plugin

    //SMTP Username: your Gmail account(xxxx@gmail.com)

    //SMTP Password: your Gmail password

    //Gmail SMTP port: 465(SSL) or 587(TLS)
    //==========================================
    //SMTP Host:	smtp.gmail.com
    //SMTP Port for SSL:	465 
    //SMTP Port for TLS / STARTTLS:	587
    //SMTP Username:	Your full email address(name@domain.com)
    //SMTP Password:	Your email account password
}
