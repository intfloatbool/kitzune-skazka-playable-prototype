using System.Threading.Tasks;

namespace Prototype.Novel
{
    public sealed class NullNovelCommand : NovelCommandBase
    {
        public NullNovelCommand(VisualNovelController novelController) : base(novelController)
        {
            
        }

        public override void Execute()
        {
            base.Execute();
            DelayStop();
        }

        async void DelayStop()
        {
            await Task.Delay(1000);
            IsRunning = false;
        }
    }
}