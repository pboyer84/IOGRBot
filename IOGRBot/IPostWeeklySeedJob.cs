using Quartz;
using System.Threading.Tasks;

namespace IOGRBot
{
    public interface IPostWeeklySeedJob
    {
        Task Execute(IJobExecutionContext context);
    }
}