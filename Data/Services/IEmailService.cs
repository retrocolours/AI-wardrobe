using AI_Wardrobe.Models;
using SendGrid;

namespace AI_Wardrobe.Data.Services
{
    public interface IEmailService
    {
        Task<Response> SendSingleEmail(ComposeEmailModel payload);
    }
}
