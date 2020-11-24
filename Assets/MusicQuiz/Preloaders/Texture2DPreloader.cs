using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MusicQuiz.Preloaders
{
    public class Texture2DPreloader : MonoBehaviour
    {
        public int size = 10;

        private readonly Queue<string> _urlsToLoad = new Queue<string>();

        private readonly Dictionary<string, Texture2D> _loadedTextures = new Dictionary<string, Texture2D>(10);

        private readonly Queue<string> _loadedUrls = new Queue<string>(10);

        public event Action<string, Texture2D> Loaded =
            delegate(string url, Texture2D texture) { };

        public void Add(string url)
        {
            if (_urlsToLoad.Count < size && !_urlsToLoad.Contains(url) && !_loadedTextures.ContainsKey(url))
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
                if (!_urlsToLoad.Contains(url) && !_loadedTextures.ContainsKey(url))
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

                StartCoroutine(DownloadImage(url));
            }
        }

        private IEnumerator DownloadImage(string url)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture) request.downloadHandler).texture;

                _loadedTextures.Add(url, texture);
                _loadedUrls.Enqueue(url);

                if (_loadedTextures.Count > size)
                {
                    var first = _loadedUrls.Dequeue();

                    _loadedTextures.Remove(first);
                }

                Debug.Log($"Texture is Loaded {url}.");
                Loaded(url, texture);
            }

            LoadNext();
        }

        public bool IsLoaded(string url)
        {
            return _loadedTextures.ContainsKey(url);
        }

        public Texture2D GetTexture2D(string url)
        {
            return _loadedTextures[url];
        }

        public static Sprite GetSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(texture.width / 2, texture.height / 2));
        }
    }
}