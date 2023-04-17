using Microsoft.Extensions.Logging;
using System.IO;

namespace BuildingShop.Utility
{
    public class NullMailService : IMailService
    {
        private readonly ILogger<NullMailService> logger;

        public NullMailService(ILogger<NullMailService> logger)
        {
            this.logger = logger;
        }

        public void SendMessage(string to, string subject, string body)
        {
            // Log the message
            logger.LogInformation($"To: {to} Subject: {subject} Body: {body}");
            File.WriteAllText(@$"d:\temp\{to}.html", body);
        }
    }
}
