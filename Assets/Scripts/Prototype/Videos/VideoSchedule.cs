using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prototype.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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

            public Button ButtonToSkip;
            public UnityEvent OnVideoEnd;
        }
        
        [SerializeField] private bool _isSetupByValidation = true;
        [SerializeField] private VideoClip[] _videoClips;
        [SerializeField] private VideoPlayer[] _videoPlayersCollection;

        private Queue<VideoPlayer> _playersQueue;

        [SerializeField] private bool _isPlayOnStart = true;
        [SerializeField] private VideoData[] _videoDataCollection;
        
        [Space]
        [SerializeField] private UnityEvent _onDone;

        [SerializeField] private int _startFromIndex = 0;
        [SerializeField] private float _forcePlayBackSpeed = 1f;
        [SerializeField] private float _playspeedByCheats = 3f;

        private Dictionary<string, VideoData> _videoDataDict;

        private class ButtonSkipper
        {
            public bool IsClicked { get; private set; }
            public ButtonSkipper(Button btn)
            {
                btn.onClick.AddListener(() =>
                {
                     if(IsClicked)
                         return;

                     IsClicked = true;
                });
            }
        }

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

                    if (videoPlayer)
                    {
                        videoPlayer.gameObject.name = $"vPlayer_{videoPlayer.clip.name}";
                    }
                }
            }
        }

        private void Awake()
        {
            var collection = _videoDataCollection.ToArray();
            _videoDataDict = _videoDataCollection.ToDictionary(v => v.Player.clip.name);
        }

        private void Start()
        {
            if(GlobalManager.IsCheatMode)
            {
                _forcePlayBackSpeed = _playspeedByCheats;
            }
            
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

            var playerCollection = _videoPlayersCollection.ToArray();
            if (_startFromIndex > 0)
            {
                var cuttedData = playerCollection.ToList();
                for (int i = 0; i < _startFromIndex; i++)
                {
                    cuttedData[i] = null;
                }

                cuttedData.RemoveAll(c => c == null);
                playerCollection = cuttedData.ToArray();

            }
            
            _playersQueue = new Queue<VideoPlayer>(playerCollection);

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
                    ButtonSkipper skipper = default;
                    if (videoData != null && videoData.ButtonToSkip)
                    {
                        skipper = new ButtonSkipper(videoData.ButtonToSkip);
                    }
                    
                    while (nextVideo.isPlaying)
                    {
                        if (skipper != null)
                        {
                            if(skipper.IsClicked)
                                break;
                        }
                        else
                        {
                            if (CheckInput())
                            {
                                break;
                            }
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

                nextVideo.frame = (long) nextVideo.frameCount - 1;

                if (videoData != null)
                {
                    videoData.OnVideoEnd?.Invoke();
                }
                if (videoData != null && videoData.IsWaitingForInputAtEnd)
                {
                    while (!CheckInput())
                    {
                        yield return null;
                    }
                }

                if (videoData != null && !videoData.Player.isLooping)
                {
                    ButtonSkipper skipper = default;
                    if (videoData.ButtonToSkip)
                    {
                        skipper = new ButtonSkipper(videoData.ButtonToSkip);
                    }
                
                    if (skipper != null)
                    {
                        while (!skipper.IsClicked)
                        {
                            yield return null;
                        }
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
            return GameHelper.IsAnyButtonPressed();
        }
    }
    
}
