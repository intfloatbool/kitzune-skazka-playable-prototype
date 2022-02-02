using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Prototype.Videos
{
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoPlayerController : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onStart;
        [SerializeField] private UnityEvent _onVideoEnd;
        [SerializeField] private float _endDelay = 0.3f;
        
        private VideoPlayer _videoPlayer;
        private bool _isEnd;
        private void Awake()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
        }

        private void Start()
        {
            _videoPlayer.Play();
            _onStart?.Invoke();
        }

        private void Update()
        {
            if(_isEnd)
                return;
            if(_videoPlayer.isLooping)
                return;
            
            if(!_videoPlayer.isPrepared)
                return;
            if (_videoPlayer.isPlaying)
            {
                 return;
            }
            
            _onVideoEnd?.Invoke();
            _isEnd = true;
            Invoke(nameof(SelfDisableDelayed), _endDelay);
        }

        private void SelfDisableDelayed()
        {
            gameObject.SetActive(false);
        } 
    }
}
