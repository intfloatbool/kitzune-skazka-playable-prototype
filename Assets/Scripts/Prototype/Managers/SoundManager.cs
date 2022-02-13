using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Prototype.Managers
{
    public class SoundManager : MonoBehaviour
    {

        [SerializeField] private AudioSource _defaultSoundSource;
        [SerializeField] private AudioSource _defaultMusicSource;

        [Space] 
        [SerializeField] private AudioClip[] _clips;

        private Dictionary<string, AudioClip> _clipsDict;

        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                _clipsDict = _clips.ToDictionary(c => c.name);
            }
            else
            {
                Debug.LogError("Some instance already initialized!");
            }
        }

        public void PlayMusic(string musicName, GameObject target = default, bool isLoop = false)
        {
            var audioSource = _defaultMusicSource;
            if (target)
            {
                if (target.TryGetComponent(out AudioSource anotherSource))
                {
                    audioSource = anotherSource;
                }
            }

            var clip = GetClipByName(musicName);
            if (clip)
            {
                audioSource.Stop();
                audioSource.clip = clip;
                audioSource.Play();
                audioSource.loop = isLoop;
            } 
        }

        public void PlaySound(string soundName, GameObject target = default)
        {
            var audioSource = _defaultSoundSource;
            if (target)
            {
                if (target.TryGetComponent(out AudioSource anotherSource))
                {
                    audioSource = anotherSource;
                }
            }

            var clip = GetClipByName(soundName);
            if (clip)
            {
                audioSource.PlayOneShot(clip);   
            }
        }

        private AudioClip GetClipByName(string clipName)
        {
            if (_clipsDict.TryGetValue(clipName, out AudioClip clip))
            {
                return clip;
            }
            
            Debug.LogError($"AudioClip with name {clipName} is not exists!");

            return null;
        }
    }
}
