using MusicQuiz.Data;
using MusicQuiz.Preloaders;
using MusicQuiz.Screens.QuizScreens;
using UnityEngine;

namespace MusicQuiz.Screens.PlaylistsScreens
{
    public class PlaylistScreen : MonoBehaviour
    {
        [SerializeField] private PlaylistsView _playlistsView = default;
        [SerializeField] private QuizScreen _quizScreen = default;

        [SerializeField] private Texture2DPreloader _texture2DPreloader = default;
        [SerializeField] private AudioClipPreloader _audioClipPreloader = default;
        
        void Awake()
        {
            _playlistsView.PlaylistSelected += PlaylistsViewOnPlaylistSelected;
            SelectedScreenState.Changed += SelectedScreenStateOnChanged;
        }

        private void SelectedScreenStateOnChanged()
        {
            if (SelectedScreenState.ScreenState == ScreenState.Playlists)
            {
                _playlistsView.PreloadFirstPlaylistQuestion();
            }
        }

        private void PlaylistsViewOnPlaylistSelected(Playlist playlist)
        {
            _texture2DPreloader.ClearQueue();
            _audioClipPreloader.ClearQueue();
            
            SelectedScreenState.ScreenState = ScreenState.Quiz;
            _quizScreen.Questions = playlist.Questions;
        }
    }
}