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
        }

        public override void Execute()
        {
            base.Execute();
            RunNextCommand();
        }

        private void RunNextCommand()
        {
            if (_commandQueue.Count <= 0)
            {
                IsRunning = false;
                if (_currentCommand != null)
                {
                    _currentCommand.Stop();
                    _currentCommand = null;
                }
                return;
            }

            if (_currentCommand != null)
            {
                _currentCommand.Stop();
            }
            _currentCommand = _commandQueue.Dequeue();
            _currentCommand.Execute();
        }

        public override void UpdateCommand()
        {
            if (!IsRunning)
            {
                return;
            }
            base.UpdateCommand();

            if (_currentCommand != null)
            {
                _currentCommand.UpdateCommand();

                if (!_currentCommand.IsRunning)
                {
                    RunNextCommand();
                }
            }
        }
    }
}