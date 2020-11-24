using System;
using MusicQuiz.Data;
using MusicQuiz.Preloaders;
using UnityEngine;

namespace MusicQuiz.Screens.PlaylistsScreens
{
    public class PlaylistsView : MonoBehaviour
    {
        [SerializeField] private Transform _content = default;
        [SerializeField] private GameObject _playlistItem = default;

        [SerializeField] private Texture2DPreloader _texture2DPreloader = default;
        [SerializeField] private AudioClipPreloader _audioClipPreloader = default;
        
        public event Action<Playlist> PlaylistSelected = delegate(Playlist playlist) { };

        private Playlist[] _playlists = default;

        public Playlist[] Playlists
        {
            set
            {
                if (_playlists != value)
                {
                    _playlists = value;
                    PreloadFirstPlaylistQuestion();
                    InstantiatePlayLists(_playlists, _playlistItem, _content);
                }
            }
        }
        
        public void PreloadFirstPlaylistQuestion()
        {
            foreach (var playlist in _playlists)
            {
                PreloadFirstPlaylistQuestion(playlist);
            }
        }

        private void PreloadFirstPlaylistQuestion(Playlist playlist)
        {
            if (playlist.Questions != null && playlist.Questions.Length > 0)
            {
                _texture2DPreloader.Add(playlist.Questions[0].Song.Picture.ToString());
                _audioClipPreloader.Add(playlist.Questions[0].Song.Sample.ToString());
            }
        }

        private void InstantiatePlayLists(Playlist[] playlists, GameObject playlistItemPrefab, Transform parent)
        {
            DestroyChildren(parent);

            foreach (var playlist in playlists)
            {
                InstantiatePlaylist(playlist, playlistItemPrefab, parent);
            }
        }

        private void DestroyChildren(Transform transform)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void InstantiatePlaylist(Playlist playlist, GameObject playlistItemPrefab, Transform parent)
        {
            var go = Instantiate(playlistItemPrefab, parent);

            var playlistItem = go.GetComponent<PlaylistItem>();
            playlistItem.Playlist = playlist;

            if (playlist.Questions != null && playlist.Questions.Length > 0)
            {
                playlistItem.Clicked += PlaylistItemOnClicked;
            }
        }

        private void PlaylistItemOnClicked(PlaylistItem playlistItem)
        {
            PlaylistSelected(playlistItem.Playlist);
        }
    }
}