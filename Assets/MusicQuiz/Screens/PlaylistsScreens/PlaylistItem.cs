using System;
using MusicQuiz.Data;
using TMPro;
using UnityEngine;

namespace MusicQuiz.Screens.PlaylistsScreens
{
    public class PlaylistItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playlistText = default;

        public event Action<PlaylistItem> Clicked = delegate(PlaylistItem playlistItem) { };

        private Playlist _playlist;

        public Playlist Playlist
        {
            get => _playlist;
            set
            {
                if (_playlist != value)
                {
                    _playlist = value;
                    _playlistText.text = value.PlaylistPlaylist;
                }
            }
        }

        public void Click()
        {
            Clicked(this);
        }
    }
}