using System.Threading;
using System.Threading.Tasks;

namespace api.reserveerme.nu.Tasks.Scheduling
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}