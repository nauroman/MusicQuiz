using System.Collections.Generic;
using MusicQuiz.Data;
using MusicQuiz.Screens.QuizScreens;
using UnityEngine;

namespace MusicQuiz.Screens.ResultScreens
{
    public class ResultScreen : MonoBehaviour
    {
        private class Answer
        {
            public readonly Choice Choice;
            public readonly bool Right;

            public Answer(Choice choice, bool right)
            {
                Choice = choice;
                Right = right;
            }
        }

        [SerializeField] private ChoicesView _resultView = default;
        [SerializeField] private QuizScreen _quizScreen = default;

        private readonly List<Answer> _answers = new List<Answer>();

        public void ClickButtonPlayAgain()
        {
            SelectedScreenState.ScreenState = ScreenState.Playlists;
        }

        private void Awake()
        {
            _quizScreen.Answered += QuizScreenOnAnswered;
            SelectedScreenState.Changed += SelectedScreenStateOnChanged;
        }

        private void QuizScreenOnAnswered(Choice choice, bool right)
        {
            _answers.Add(new Answer(choice, right));
        }

        private void SelectedScreenStateOnChanged()
        {
            if (SelectedScreenState.ScreenState == ScreenState.Result)
            {
                InstantiateAnswers();
            }
            else
            {
                _answers.Clear();
            }
        }

        private void InstantiateAnswers()
        {
            _resultView.DestroyChildren();
            
            foreach (var answer in _answers)
            {
                if (answer.Right)
                {
                    _resultView.InstantiateRightChoice(answer.Choice);
                }
                else
                {
                    _resultView.InstantiateWrongChoice(answer.Choice);
                }
            }
        }
    }
}