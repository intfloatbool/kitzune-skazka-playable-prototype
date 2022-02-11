namespace Prototype.Novel
{
    public class NovelDialogueCommand : NovelCommandBase
    {
        private readonly VisualNovelDialogue _dialogue;
        public NovelDialogueCommand(VisualNovelController novelController, VisualNovelDialogue dialogue) : base(novelController)
        {
            _dialogue = dialogue;
        }

        public override void Execute()
        {
            base.Execute();
            
            _dialogue.gameObject.SetActive(true);
            _dialogue.StartDialog();
            
            _dialogue.OnDialogDoneCallback = DialogDoneCallback;
        }

        private void DialogDoneCallback(VisualNovelDialogue obj)
        {
            IsRunning = false;
            obj.gameObject.SetActive(false);
        }
    }
}