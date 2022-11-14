using System.Threading.Tasks;

namespace DoAn_IE307_N11.Interfaces
{
    public interface IMessageService
    {
        Task ShowAsync(string message);
        Task<bool> ShowAskAsync(string message);
    }
}
