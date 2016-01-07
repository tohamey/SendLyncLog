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
            //this part will be considered as a Torjan by Avira, ignore that it is false positive
            //this part just to get the username and domain of looged user to use it as the "fromEmail"
            string userName = Environment.UserName;
            string domainName = Environment.UserDomainName;
            string _from = userName + "@" + domainName;
            //calling an Instance of ComposeLogFile Class to start the work
            ComposeLogFile Lynclog = new ComposeLogFile();
            try {
                //using the provided arguments in the command-line 
                // args[0] will be pointing to the toEmail variable needed by the Method
                //args[1] will be pointing to the exchangeServer variable needed by the Method
                string _to = args[0].ToString();
                string _smtpServer = args[1].ToString();
                //call the Method and pass the required Parameters to it
                Lynclog.getLogFile(_to, _from, _smtpServer);
            }
            catch
            {
                MessageBox.Show("A valid Email address or SMTP server have not been provided \n please contact your Administrator", "Error");
            }
        }
        

    }
}
