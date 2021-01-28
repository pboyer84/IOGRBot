using System.Threading.Tasks;

namespace IOGRBot
{
    public interface IScheduler
    {
        Task Shutdown();
        Task Start();
        Task<bool> TryInitWithSchedule(IBot owner, string cronExpression);
    }
}