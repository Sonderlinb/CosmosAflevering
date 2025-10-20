using System.Collections.Generic;
using System.Threading.Tasks;
using SupportApp.Models;

namespace SupportApp.Services
{
    public interface ISupportService
    {
        Task AddSupportMessageAsync(SupportMessage message);
        Task<IEnumerable<SupportMessage>> GetAllSupportMessagesAsync();
    }
}
