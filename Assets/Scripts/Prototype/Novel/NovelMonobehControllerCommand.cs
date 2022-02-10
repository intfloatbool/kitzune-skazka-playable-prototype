using UnityEngine;

namespace Prototype.Novel
{
    public class NovelMonobehControllerCommand : NovelCommandBase
    {
        protected readonly GameObject _gameObject;
        public NovelMonobehControllerCommand(VisualNovelController novelController, GameObject gameObject) : base(novelController)
        {
            _gameObject = gameObject;
        }

        public override void Execute()
        {
            base.Execute();
            _gameObject.SetActive(true);
        }

        public override void Stop()
        {
            base.Stop();
            _gameObject.SetActive(false);
        }
    }
}