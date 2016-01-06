/*
       a program to be integrated into Lync client menu to make it
       easy for user to right click and send log files to thier organoization Admins
       the idea is to integrate it via GPO into the organization
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Net.Mail;

namespace SLL
{
    class Program
    {
        static void Main(string[] args)
        {
            string userName = Environment.UserName;
            string domainName = Environment.UserDomainName;
            string _from = userName + "@" + domainName;
            ComposeLogFile Lynclog = new ComposeLogFile();
            try {
                string _to = args[0].ToString();
                string _smtpServer = args[1].ToString();
                Lynclog.getLogFile(_to, _from, _smtpServer);
            }
            catch
            {
                MessageBox.Show("A valid Email address or SMTP server have not been provided \n please contact your Administrator", "Error");
            }
        }
        

    }
}
