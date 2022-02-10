using UnityEngine.SceneManagement;

namespace Prototype.Novel
{
    public class NextSceneCommand : NovelCommandBase
    {
        private readonly string _sceneName;
        public NextSceneCommand(VisualNovelController novelController, string sceneName) : base(novelController)
        {
            _sceneName = sceneName;
        }

        public override void Execute()
        {
            base.Execute();
            SceneManager.LoadScene(_sceneName);
        }
    }
}