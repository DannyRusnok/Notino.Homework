using System.Net.Mail;

namespace Notinot.Homework.Application;

public class EmailSender : IEmailSender
{
    private string _password;
    private string _userName;
    private string _emailFrom;
    private string _host;

    public EmailSender(IConfiguration configuration)
    {
        var section = configuration.GetSection("Smtp");
        _password = section["Password"];
        _userName = section["UserName"];
        _emailFrom = section["EmailFrom"];
        _host = section["Host"];
    }
    public void SendConvertionEmail(string emailTo, string convertedFilePath)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient(_host);
        mail.From = new MailAddress(_emailFrom);
        mail.To.Add(emailTo);
        mail.Subject = "Converted File";
        mail.Body = "See converted file in attachment";

        Attachment attachment;
        attachment = new Attachment(convertedFilePath);
        mail.Attachments.Add(attachment);

        SmtpServer.Port = 587;
        SmtpServer.Credentials = new System.Net.NetworkCredential(_userName, _password);
        SmtpServer.EnableSsl = true;

        SmtpServer.Send(mail);

    }
}
