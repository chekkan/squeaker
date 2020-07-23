using System.Threading.Tasks;

namespace Squeaker.Application
{
    public interface SqueakeByIdUseCase
    {
        Task<Squeake> FindById(string id);
    }
}