namespace BuildingShop.Utility
{
    public interface IMailService
    {
        void SendMessage(string to, string subject, string body);
    }
}