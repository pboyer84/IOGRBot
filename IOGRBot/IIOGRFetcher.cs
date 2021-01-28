using System.Threading.Tasks;

namespace IOGRBot
{
    public interface IIOGRFetcher
    {
        Task<string> GetNewSeedPermalink();
    }
}