using System.Threading.Tasks;

namespace IOGRBot
{
    public interface IBot
    {
        Task PostAnnouncement(string message);
        Task ShutdownAsync();
        Task StartAsync();
    }
}