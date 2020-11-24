using System;

namespace MusicQuiz.Screens
{
    public enum ScreenState
    {
        None = 0,
        Playlists = 1,
        Quiz = 2,
        Result = 3
    }

    public static class SelectedScreenState
    {
        public static event Action Changed = delegate { };

        private static ScreenState _screenState = ScreenState.None;

        public static ScreenState ScreenState
        {
            get => _screenState;
            set
            {
                if (_screenState != value)
                {
                    _screenState = value;
                    Changed();
                }
            }
        }
    }
}