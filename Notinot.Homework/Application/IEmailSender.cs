namespace Notinot.Homework.Application
{
    public interface IEmailSender
    {
        void SendConvertionEmail(string emailTo, string convertedFilePath);
    }
}