using tk.bot.ca.threading.tasks.procedures;

namespace tk.bot.handler
{
    public interface ICoreHandler : IAttachedTask
    {
        void Execute();
    }
}
