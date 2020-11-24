using System;

namespace MusicQuiz.Screens.QuizScreens
{
    [Flags]
    public enum AnswerState
    {
        None = 0,
        Waiting = 1,
        RightChoiceSelected = 2,
        WrongChoiceSelected = 4
    }

    public static class SelectedAnswerState
    {
        public static event Action Changed = delegate { };

        private static AnswerState _answerState = AnswerState.None;

        public static AnswerState AnswerState
        {
            get => _answerState;
            set
            {
                if (_answerState != value)
                {
                    _answerState = value;
                    Changed();
                }
            }
        }
    }
}