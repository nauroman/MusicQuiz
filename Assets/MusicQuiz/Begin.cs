using MusicQuiz.Data;
using MusicQuiz.Screens;
using MusicQuiz.Screens.PlaylistsScreens;
using UnityEngine;

namespace MusicQuiz
{
    public class Begin : MonoBehaviour
    {
        [SerializeField] private string _playlistsJson = default;

        [SerializeField] private PlaylistsView _playlistsView = default; 
        void Start()
        {
            _playlistsView.Playlists = Playlists.FromJson(_playlistsJson);
            SelectedScreenState.ScreenState = ScreenState.Playlists;
        }
    }
}