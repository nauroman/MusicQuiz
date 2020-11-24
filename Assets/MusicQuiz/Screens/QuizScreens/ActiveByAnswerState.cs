using UnityEngine;

namespace MusicQuiz.Screens.QuizScreens
{
    public class ActiveByAnswerState : MonoBehaviour
    {
        [SerializeField] private AnswerState _answerState = AnswerState.None;

        private void Awake()
        {
            SelectedAnswerState.Changed += SelectedAnswerStateOnChanged;
            SelectedAnswerStateOnChanged();
        }

        private void SelectedAnswerStateOnChanged()
        {
            gameObject.SetActive(_answerState.HasFlag(SelectedAnswerState.AnswerState));
        }
    }
}