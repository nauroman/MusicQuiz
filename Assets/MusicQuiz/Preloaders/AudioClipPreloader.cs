using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MusicQuiz.Preloaders
{
    public class AudioClipPreloader : MonoBehaviour
    {
        public int size = 10;

        private readonly Queue<string> _urlsToLoad = new Queue<string>();

        private readonly Dictionary<string, AudioClip> _loadedAudioClips = new Dictionary<string, AudioClip>(10);

        private readonly Queue<string> _loadedUrls = new Queue<string>(10);

        public event Action<string, AudioClip> Loaded =
            delegate(string url, AudioClip audioClip) { };

        public void Add(string url)
        {
            if (_urlsToLoad.Count < size && !_urlsToLoad.Contains(url) && !_loadedAudioClips.ContainsKey(url))
            {
                _urlsToLoad.Enqueue(url);

                if (_urlsToLoad.Count == 1)
                {
                    LoadNext();
                }
            }
        }

        public void Add(string[] assetRefs)
        {
            var updated = false;
            foreach (var url in assetRefs)
            {
                if (!_urlsToLoad.Contains(url) && !_loadedAudioClips.ContainsKey(url))
                {
                    _urlsToLoad.Enqueue(url);
                    updated = true;
                }
            }

            if (updated)
            {
                LoadNext();
            }
        }

        public void ClearQueue()
        {
            _urlsToLoad.Clear();
        }

        private void LoadNext()
        {
            if (_urlsToLoad.Count > 0)
            {
                var url = _urlsToLoad.Dequeue();

                StartCoroutine(Download(url));
            }
        }

        private IEnumerator Download(string url)
        {
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);

            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);

                _loadedAudioClips.Add(url, audioClip);
                _loadedUrls.Enqueue(url);

                if (_loadedAudioClips.Count > size)
                {
                    var first = _loadedUrls.Dequeue();

                    _loadedAudioClips.Remove(first);
                }

                Debug.Log($"AudioClip is Loaded {url}.");
                Loaded(url, audioClip);
            }

            LoadNext();
        }

        public bool IsLoaded(string url)
        {
            return _loadedAudioClips.ContainsKey(url);
        }

        public AudioClip GetAudioClip(string url)
        {
            return _loadedAudioClips[url];
        }
    }
}