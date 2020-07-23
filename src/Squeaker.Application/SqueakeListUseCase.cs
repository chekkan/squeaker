using System.Threading.Tasks;

namespace Squeaker.Application
{
    public interface ListSqueakesUseCase
    {
        Task<(Squeake[], int)> FindAll(int limit = 10, int page = 1);
    }
}