using System.Net.Mail;
using System.Net;

namespace PharmactMangmentEditeIdea.HelperMethod
{
    public static class EmailSettings
    {

        // علشان احط فيها الايميل اللي هبعت منو الايميلات
        public static bool SendEmail(Email email)
        {
            try
            {
                //cdqionvvnjoaubsw
                // Mail Server  :Gmail OR Outlock

                //SMTP ==.> Simple Mail Transfer Protocol علشان ده بروتوكول نقل البريد البسيط
                // عباره عن سيرفر ابعت منه الايميلات
                var client = new SmtpClient("smtp.gmail.com", 587);
                // علشان لو في يوم حبيت اغير البورت
                client.EnableSsl = true; //  تشفير البيانات  
                // هويه المرسل
                client.Credentials = new NetworkCredential("mohamedosamagfg@gmail.com", "jzysjzjwbcofukrb");// sender 
                client.Send("mohamedosamagfg@gmail.com", email.To, email.Subject, email.Body); //
                return true;

            }
            catch (Exception e)
            {

                return false;
            }
        }
    }
}
