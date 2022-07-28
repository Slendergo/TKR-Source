using System.Threading;

namespace tk.bot.ca.threading.tasks.procedures
{
#pragma warning disable

    public interface IAttachedTask
    {
        CancellationToken GetToken { get; }

        void AttachToParent(CancellationToken token);
    }

#pragma warning restore
}
