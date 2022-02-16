using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Prototype.Videos
{
    public class VideoSchedule : MonoBehaviour
    {
        [System.Serializable]
        private class VideoData
        {
            public VideoPlayer Player;
            public bool IsWaitingForInputAtEnd;
            public bool IsWaitingForInputAllTime;
        }
        
        [SerializeField] private bool _isSetupByValidation = true;
        [SerializeField] private VideoClip[] _videoClips;
        [SerializeField] private VideoPlayer[] _videoPlayersCollection;

        private Queue<VideoPlayer> _playersQueue;

        [SerializeField] private bool _isPlayOnStart = true;
        [SerializeField] private VideoData[] _videoDataCollection;
        
        [Space]
        [SerializeField] private UnityEvent _onDone;

        [SerializeField] private float _forcePlayBackSpeed = 1f;

        private Dictionary<string, VideoData> _videoDataDict;

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

        private void Awake()
        {
            _videoDataDict = _videoDataCollection.ToDictionary(v => v.Player.clip.name);
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

                VideoData videoData = default;
                _videoDataDict.TryGetValue(nextVideo.clip.name, out videoData);
                
                while (!nextVideo.isPrepared)
                {
                    yield return null;
                }
                
                nextVideo.Play();

                if (nextVideo.isLooping)
                {
                    while (nextVideo.isPlaying)
                    {
                        if (CheckInput())
                        {
                            break;
                        }
                        yield return null;
                    }
                }
                else
                {
                    while (nextVideo.isPlaying)
                    {
                        if (videoData != null && videoData.IsWaitingForInputAllTime)
                        {
                            while (!CheckInput())
                            {
                                if (nextVideo.isPaused)
                                {
                                    break;
                                }
                                yield return null;
                            }
                            break;
                        }
                        
                        yield return null;
                    }   
                }

                if (videoData != null && videoData.IsWaitingForInputAtEnd)
                {
                    while (!CheckInput())
                    {
                        yield return null;
                    }
                }

                yield return new WaitForEndOfFrame();
                nextVideo.gameObject.SetActive(false);
            }

            yield return new WaitForEndOfFrame();
            _onDone?.Invoke();
        }

        private bool CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return true;
            }
            return false;
        }
    }
    
}
