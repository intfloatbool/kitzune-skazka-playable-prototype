using System.Collections.Generic;

namespace Prototype.Novel
{
    public class MacroNovelCommandBase : NovelCommandBase
    {
        protected NovelCommandBase _currentCommand;
        protected Queue<NovelCommandBase> _commandQueue;

        public MacroNovelCommandBase(VisualNovelController novelController, params NovelCommandBase[] subCommands) : base(novelController)
        {
            _commandQueue = new Queue<NovelCommandBase>(subCommands);
            RunNextCommand();

        }

        private void RunNextCommand()
        {
            if (_commandQueue.Count <= 0)
            {
                Done();
            }
            _currentCommand = _commandQueue.Dequeue();
            _currentCommand.Execute();
        }

        private void Done()
        {
            IsRunning = false;
        }

        public override void UpdateCommand()
        {
            base.UpdateCommand();

            if (_currentCommand != null)
            {
                _currentCommand.UpdateCommand();

                if (!_currentCommand.IsRunning)
                {
                    
                }
            }
        }
    }
}