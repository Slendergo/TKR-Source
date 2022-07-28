using System.Threading.Tasks;

namespace tk.assetformatter.handler
{
    public interface IHandler
    {
        Task ExecuteAsync(params string[] args);
    }
}
