using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace Prototype.Videos
{
    public class VideosController : MonoBehaviour
    {
        [SerializeField] private VideoClip[] _videoClips;
        [SerializeField] private bool[] _isLoopArray;
        
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private bool _isPlayOnAwake = true;

        private Coroutine _playCoroutine;

        private void Awake()
        {
            if(_isPlayOnAwake)
                Play();
        }

        public void Play()
        {
            if (_playCoroutine != null)
            {
                Debug.LogError("Las PlayProcess is not done! ");
                return;
            }

            _playCoroutine = StartCoroutine(PlayCoroutine());
        }

        private IEnumerator PlayCoroutine()
        {

            for (int i = 0; i < _videoClips.Length; i++)
            {
                var clip = _videoClips[i];
                if (clip)
                {
                    _videoPlayer.clip = clip;
                    _videoPlayer.Prepare();
                    while (!_videoPlayer.isPrepared)
                    {
                        yield return null;
                    }
                    bool isLoop = false;
                    if (i < _isLoopArray.Length)
                    {
                        isLoop = _isLoopArray[i];
                    }

                    _videoPlayer.isLooping = isLoop;
                    _videoPlayer.Play();
                    if(isLoop)
                        break;

                    while (_videoPlayer.isPlaying)
                    {
                        yield return null;
                    }
                    
                    _videoPlayer.Stop();
                    yield return null;
                }
            }
            
            _playCoroutine = null;
            yield return null;
        }
    }
}
