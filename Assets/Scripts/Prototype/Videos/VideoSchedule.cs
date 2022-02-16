using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Prototype.Videos
{
    public class VideoSchedule : MonoBehaviour
    {
        [SerializeField] private bool _isSetupByValidation = true;
        [SerializeField] private VideoClip[] _videoClips;
        [SerializeField] private VideoPlayer[] _videoPlayersCollection;
        private Queue<VideoPlayer> _playersQueue;

        [SerializeField] private bool _isPlayOnStart = true;

        [Space]
        [SerializeField] private UnityEvent _onDone;

        [SerializeField] private float _forcePlayBackSpeed = 1f;

        private void OnValidate()
        {
            if (_isSetupByValidation)
            {
                _videoPlayersCollection = GetComponentsInChildren<VideoPlayer>();
                for (int i = 0; i < _videoPlayersCollection.Length; i++)
                {
                    var videoPlayer = _videoPlayersCollection[i];
                    if (videoPlayer && _videoClips != null && i < _videoClips.Length)
                    {
                        videoPlayer.clip = _videoClips[i];
                    }
                }
            }
        }

        private void Start()
        {
            if (_isPlayOnStart)
                StartCoroutine(PlayCoroutine());
        }

        private IEnumerator PlayCoroutine()
        {
            foreach (var videoPlayer in _videoPlayersCollection)
            {
                videoPlayer.playbackSpeed = _forcePlayBackSpeed;
                videoPlayer.gameObject.SetActive(false);
            }
            _playersQueue = new Queue<VideoPlayer>(_videoPlayersCollection);

            while (_playersQueue.Count > 0)
            {
                var nextVideo = _playersQueue.Dequeue();
                nextVideo.gameObject.SetActive(true);
                nextVideo.Prepare();
                while (!nextVideo.isPrepared)
                {
                    yield return null;
                }
                
                nextVideo.Play();
                
                while (nextVideo.isPlaying)
                {
                    yield return null;
                }

                yield return new WaitForEndOfFrame();
                nextVideo.gameObject.SetActive(false);
            }

            yield return new WaitForEndOfFrame();
            _onDone?.Invoke();
        }
    }
}
