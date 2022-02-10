using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Video;

namespace Prototype.Novel
{
    public class NovelVideoCommand : NovelMonobehControllerCommand
    {
        private readonly VideoPlayer _videoPlayer;
        private bool _isPrepared = false;
        public NovelVideoCommand(VisualNovelController novelController, GameObject gameObject) : base(novelController, gameObject)
        {
            _videoPlayer = gameObject.GetComponentInChildren<VideoPlayer>();
            _videoPlayer.Prepare();
            _videoPlayer.prepareCompleted += VideoPlayerOnPrepareCompleted;
            Assert.IsNotNull(_videoPlayer, "_videoPlayer != null");
        }

        private void VideoPlayerOnPrepareCompleted(VideoPlayer source)
        {
            source.Play();
            _isPrepared = true;
        }

        public override void UpdateCommand()
        {
            if (!_isPrepared)
            {
                return;
            }
            if(!IsRunning)
                return;
            
            base.UpdateCommand();

            if (_videoPlayer.isPrepared && !_videoPlayer.isPlaying)
            {
                IsRunning = false;
            }
        }
    }
}