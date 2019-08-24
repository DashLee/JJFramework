﻿using UnityEngine;
using System.Collections.Generic;

namespace JJFramework.Runtime.Resource
{
    [DisallowMultipleComponent]
    public class SoundManager
    {
        private static readonly string SOUND = "sound";
        private IResourceLoader _resourceLoader;

        private List<AudioSource> _effectSource = new List<AudioSource>();
        private AudioSource _musicSource;

        private Dictionary<string, AudioClip> _clipsDic = new Dictionary<string, AudioClip>();
        private int _currentIndex;
        private int _maxIndex;

        public void Init(IResourceLoader resourceLoader, int effectBuffer)
        {
            _resourceLoader = resourceLoader;

            _maxIndex = effectBuffer;

            var obj = new GameObject(nameof(SoundManager));

            for (int listLoop = 0; listLoop < effectBuffer; ++listLoop)
            {
                var audioSource = obj.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                _effectSource.Add(audioSource);
            }

            _musicSource = obj.AddComponent<AudioSource>();
            _musicSource.playOnAwake = false;

            GameObject.DontDestroyOnLoad(obj);
        }

        public void PreloadEffects(params string[] clipList)
        {
            if (clipList != null)
            {
                foreach (var clip in clipList)
                {
                    LoadClip(clip);
                }
            }
        }

        private void LoadClip(string clip)
        {
            if (_clipsDic.ContainsKey(clip) == false)
            {
                var audio = _resourceLoader.Load<AudioClip>(SOUND, clip);
                if (audio != null)
                {
                    _clipsDic.Add(clip, audio);
                }
                else
                {
                    Debug.LogError("AudioClip이 없어요!");
                }
            }
        }

        public void PlaySingle(string clip, bool isLoop = false, float volume = 1f)
        {
            LoadClip(clip);
            PlaySingle(_clipsDic[clip], isLoop, volume);
        }

        public int PlaySingle(AudioClip clip, bool isLoop = false, float volume = 1f)
        {
            _effectSource[_currentIndex].clip = clip;
            _effectSource[_currentIndex].loop = isLoop;
            _effectSource[_currentIndex].volume = volume;
            _effectSource[_currentIndex].Play();

            var current = _currentIndex;
            ++_currentIndex;
            if (_currentIndex >= _maxIndex)
            {
                _currentIndex = 0;
            }

            return current;
        }

        public void PlayBGM(string clip, bool isLoop = true, float volume = 1f)
        {
            LoadClip(clip);
            PlayBGM(_clipsDic[clip], isLoop, volume);
        }

        public void PlayBGM(AudioClip clip, bool isLoop = true, float volume = 1f)
        {
            _musicSource.clip = clip;
            _musicSource.loop = isLoop;
            _musicSource.volume = volume;
            _musicSource.Play();
        }

        public void SetBGMVolume(float volume)
        {
            _musicSource.volume = volume;
        }

        public void StopEffect(int index)
        {
            _effectSource[index].Stop();
        }

        public void StopAllEffect()
        {
            _effectSource.ForEach(x => x.Stop());
        }

        public void StopBGM()
        {
            _musicSource.Stop();
        }

        public void Stop(string clip)
        {
            _effectSource.Find(x => x == _clipsDic[clip])?.Stop();
        }
    }
}
