﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using Microsoft.VisualBasic;


namespace SLL
{
    class ComposeLogFile
    {
        //constracting the location of the log file to be zipped
        static string path1 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        //below is path for logs files used by Office 16.0 (SkypeFB) change the 16.0 to 15.0 for (lync)
        static string path2 = @"AppData\Local\Microsoft\Office\16.0\Lync\Tracing\";
        static string _logLocation = Path.Combine(path1, path2);
        //location where i want to save the zipped file
        string _folderToZip = @"c:\tempSkype4b\logs";
        string _zippedLogs = @"c:\tempSkype4b\Skype4b_logs.zip";
        string _lyncLogCopy = @"c:\tempSkype4b\logs\lynclogfile.uccapilog";
        string[] _getLogFiles = Directory.GetFiles(_logLocation, "*.uccapilog");
        //Attachment for the Email to be sent later on
        string attachment = @"c:\tempSkype4b\Skype4b_logs.zip";

        //method to create the file and send it
        public void getLogFile(string toEmail, string fromEmail, string exchangeServer)
        {
            //start by checking if the templync directory exist or not if not it will create it
            //if it exist then it will check if its empty or not, if not it will delete existing files

            //check if logging is enabled
                if (Directory.Exists(_logLocation) == true)
                {
                    //if directory exist
                    if (Directory.Exists("C:\\tempSkype4b\\logs") == true)
                    {
                        //check if old log file exist and clean it up
                        if (File.Exists("c:\\tempSkype4b\\Skype4b_logs.zip") == true)
                        {
                            //delete the file ans use Task to wait for the deletion to finish before start copying
                            Task DeleteFile = Task.Factory.StartNew(() => File.Delete(_zippedLogs));
                            DeleteFile.Wait();
                            //start copy the new log file to the templync folder and zip it
                            CopyLogFile();


                        }
                        //if the templync folder was empty then start copying and zipping
                        else
                        {
                            //copy the log file and zip it
                            CopyLogFile();
                        }
                    }
                    //if templync folder does not exist, create it, copy log file to it and zip it
                    else
                    {
                        //wait for creating the directory before starting the zipping process
                        Task CreateDirecotry = Task.Factory.StartNew(() => Directory.CreateDirectory("C:\\tempSkype4b\\logs"));
                        CreateDirecotry.Wait();
                        //copy log file and zip it
                        CopyLogFile();
                    }
                }
            else

            { 
                //if no folder exist then logging is disabled and return an error
                MessageBox.Show("Lync Logging is Not Enabled.\nPlease enable Logging under Skype for Business client options\nand restart your Lync client", "Error");
            }
            //Part to send the logs as email
            
            //compose the email
                MailMessage _email = new MailMessage(
                    fromEmail,
                    toEmail,
                    "Skype For Business client Log file",
                    "this email is generated by automatic Lync/Skype4b SIP-Stack Log creator \n for feedbacks, please send email to feedback@skype4b.com\nor visit www.lyncdude.com");
                //attach the log file
                Attachment data = new Attachment(attachment);
                _email.Attachments.Add(data);
                //NetworkCredential userCredentials = new NetworkCredential(_username, _password);
                //setting up the Exchange server

                SmtpClient _mailClient = new SmtpClient();
                _mailClient.Host = exchangeServer;
                _mailClient.Credentials = CredentialCache.DefaultNetworkCredentials;
                //send the email
                try
                {
                    _mailClient.Send(_email);
                    MessageBox.Show("Email with Log file been sent to your Administrator","Successful");
                }
                catch (Exception ex)
                {
                    //error catched
                    MessageBox.Show("Email could not be sent, check your credentials or server settings!!", "Error");
                }

        }
        //method to do the copying
        private void CopyLogFile()
        {
            foreach (string file in _getLogFiles)
            {
                //copy the file and wait for the copy to finish
                Task CopyLogFile = Task.Factory.StartNew(() => File.Copy(file, _lyncLogCopy, true));
                CopyLogFile.Wait();
                //zip the copied log file
                ZipFile.CreateFromDirectory(_folderToZip, _zippedLogs, CompressionLevel.Optimal, false);
            }
        }

    }
}
