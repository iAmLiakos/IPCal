using IPCal.Data;
using IPCal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace IPCal.Services
{
    public class EmailService
    {

        public void SendEmailService()
        {
            string reminderenabled = App.Current.Properties["EmailReminder"].ToString();
            if (reminderenabled == "True")
            {
                string emailbody = GetRantezvousList();
                string body = this.createEmailBody(App.Current.Properties["Name"].ToString(), "Οι καθαρισμοί για τις επόμενες " + App.Current.Properties["Days"].ToString() + " μέρες είναι", emailbody);

                this.SendHtmlFormattedEmail("Η καθημερινή ενημέρωση!", body);
            }
            else
                App.Current.MainPage.DisplayAlert("Μήνυμα Συστήματος", "Δεν έχετε ενεργοποιήσει την αποστολή μέσω email στο προφίλ σας", "Οκ");
        }

        private string GetRantezvousList()
        {
            string final;
            RantezvousDataAccess data = new RantezvousDataAccess();
            //List<Rantezvous> listdata = data.GetFilteredRantezvous10DaysNear().ToList();
            //data.Rantezvous.ToList();
            //final = data.ToString();
            int days = (int)App.Current.Properties["Days"];
            var query = from c in data.Rantezvous
                        orderby c.AppointmentDate where c.AppointmentDate >= DateTime.Now && c.AppointmentDate <= DateTime.Now.AddDays(days)
                        select new { c.CustomerName, c.CustomerAddress, c.AppointmentDate, c.DateTrimmed, c.CustomerPhone, c.Details};
            var results = query.ToList();
            StringBuilder strb = new StringBuilder();
            //int i=1;
            string table = "<table style='background-color: #f1f1c1' border='2'><tr><th>Όνομα</th><th>Διεύθυνση</th><th>Ημερομηνία</th><th>Τηλέφωνο</th><th>Λεπτομέρειες</th></tr>";
            foreach (var item in results)
            {
                string name = item.CustomerName;
                string address = item.CustomerAddress;
                string date =item.DateTrimmed;
                string phone = item.CustomerPhone.ToString();
                string details = item.Details;
                //strb.AppendFormat("<li>"+ i +") {0} <p>{1}</p> <p>{2}</p>"+ "</li>", name, address, date);
                strb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", name, address, date, phone, details);
                //strb.AppendLine();
                //i++;
            }
            final = table + strb.ToString() +"</table>";
            return final;
        }

        //button action
        protected void SendEmail(object sender, EventArgs e)

        {
            //calling for creating the email body with html template   

            string body = this.createEmailBody(App.Current.Properties["Name"].ToString(), "Data data data data data", "Some data for message");

            this.SendHtmlFormattedEmail("Η καθημερινή ενημέρωση!", body);

        }

        public string createEmailBody(string userName, string title, string message)

        {

            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Services\MailTemplate.html");
           
            using (StreamReader reader = new StreamReader(@"MailTemplate.html"))

            {

                body = reader.ReadToEnd();

            }

            body = body.Replace("{UserName}", userName); //replacing the required things  

            body = body.Replace("{Title}", title);

            body = body.Replace("{message}", message);

            return body;

        }

        public void SendHtmlFormattedEmail(string subject, string body)

        {

            using (MailMessage mailMessage = new MailMessage())

            {

                mailMessage.From = new MailAddress(App.Current.Properties["Email"].ToString());

                mailMessage.Subject = subject;

                mailMessage.Body = body;

                mailMessage.IsBodyHtml = true;

                mailMessage.To.Add(new MailAddress(App.Current.Properties["Email"].ToString()));

                SmtpClient smtp = new SmtpClient();

                smtp.Host = "smtp.gmail.com";

                smtp.EnableSsl = true;

                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

                NetworkCred.UserName = App.Current.Properties["Email"].ToString(); //reading from app properties  

                NetworkCred.Password = App.Current.Properties["EmailPassword"].ToString(); //reading from app properties 

                smtp.UseDefaultCredentials = true;

                smtp.Credentials = NetworkCred;

                smtp.Port = int.Parse("587");

                smtp.Send(mailMessage);

            }

        }
    }

}
