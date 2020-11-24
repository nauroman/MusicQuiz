using System;
using MusicQuiz.Data;
using MusicQuiz.Preloaders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MusicQuiz.Screens.QuizScreens
{
    public class QuizScreen : MonoBehaviour
    {
        [SerializeField] private Image _image = default;

        [SerializeField] private TMP_Text _title = default;
        [SerializeField] private TMP_Text _artist = default;
        [SerializeField] private AudioSource _audioSource = default;

        [SerializeField] private ChoicesView _choicesView = default;

        [SerializeField] private Texture2DPreloader _texture2DPreloader = default;
        [SerializeField] private AudioClipPreloader _audioClipPreloader = default;

        public event Action<Choice, bool> Answered = delegate(Choice choice, bool right) { };

        private void OnEnable()
        {
            _choicesView.Selected += ChoicesViewOnSelected;
            _texture2DPreloader.Loaded += Texture2DPreloaderOnLoaded;
            _audioClipPreloader.Loaded += AudioClipPreloaderOnLoaded;
        }

        private void OnDisable()
        {
            _choicesView.Selected -= ChoicesViewOnSelected;
            _texture2DPreloader.Loaded -= Texture2DPreloaderOnLoaded;
            _audioClipPreloader.Loaded -= AudioClipPreloaderOnLoaded;
        }

        private void ChoicesViewOnSelected(Choice choice)
        {
            if (SelectedAnswerState.AnswerState == AnswerState.None ||
                SelectedAnswerState.AnswerState == AnswerState.Waiting)
            {
                var correct = choice.Index == Question.AnswerIndex;

                if (correct)
                {
                    SelectedAnswerState.AnswerState =
                        AnswerState.RightChoiceSelected;

                    _choicesView.ShowRightChoice(choice.Index);
                }
                else
                {
                    SelectedAnswerState.AnswerState =
                        AnswerState.WrongChoiceSelected;

                    _choicesView.ShowWrongChoice(choice.Index);
                    _choicesView.ShowRightChoice(Question.AnswerIndex);
                }

                Answered(choice, correct);
            }
        }

        private Question[] _questions = default;

        public Question[] Questions
        {
            private get => _questions;
            set
            {
                if (_questions != value)
                {
                    _questions = value;
                    First();
                }
            }
        }

        private Question _question = default;

        private Question Question
        {
            get => _question;
            set
            {
                if (_question != value)
                {
                    _question = value;

                    _title.text = value.Song.Title;
                    _artist.text = value.Song.Artist;

                    var pictureUrl = _question.Song.Picture.ToString();

                    if (_texture2DPreloader.IsLoaded(pictureUrl))
                    {
                        Debug.Log($"Texture was preloaded {pictureUrl}.");

                        _image.overrideSprite =
                            Texture2DPreloader.GetSprite(_texture2DPreloader.GetTexture2D(pictureUrl));
                    }
                    else
                    {
                        _texture2DPreloader.Add(pictureUrl);
                    }

                    var sampleUrl = _question.Song.Sample.ToString();

                    if (_audioClipPreloader.IsLoaded(sampleUrl))
                    {
                        Debug.Log($"Sample was preloaded {sampleUrl}.");

                        _audioSource.clip = _audioClipPreloader.GetAudioClip(sampleUrl);
                        _audioSource.Play();
                    }
                    else
                    {
                        _audioClipPreloader.Add(sampleUrl);
                    }

                    _choicesView.Choices = _question.Choices;

                    SelectedAnswerState.AnswerState = AnswerState.Waiting;
                }
            }
        }

        private void First()
        {
            QuestionIndex = 0;
        }

        public void Next()
        {
            if (QuestionIndex == Questions.Length - 1)
            {
                First();
                SelectedScreenState.ScreenState = ScreenState.Result;
            }
            else
            {
                QuestionIndex++;
            }
        }

        private bool IsNextQuestion => QuestionIndex < Questions.Length - 1;

        private Question NextQuestion
        {
            get
            {
                if (IsNextQuestion)
                {
                    return Questions[QuestionIndex + 1];
                }

                return null;
            }
        }

        private void PreloadNextQuestion()
        {
            if (IsNextQuestion)
            {
                PreloadSong(NextQuestion.Song);
            }
        }

        private void PreloadSong(Song song)
        {
            _texture2DPreloader.Add(song.Picture.ToString());
            _audioClipPreloader.Add(song.Sample.ToString());
        }

        private int _questionIndex = -1;

        private int QuestionIndex
        {
            get => _questionIndex;
            set
            {
                if (_questionIndex != value)
                {
                    _questionIndex = value;
                    PreloadNextQuestion();
                    Question = Questions[_questionIndex];
                }
            }
        }


        private void Texture2DPreloaderOnLoaded(string url, Texture2D texture)
        {
            if (url == Question.Song.Picture.ToString())
            {
                _image.overrideSprite = Texture2DPreloader.GetSprite(texture);
            }
        }

        private void AudioClipPreloaderOnLoaded(string url, AudioClip audioClip)
        {
            if (url == Question.Song.Sample.ToString())
            {
                _audioSource.clip = audioClip;
                _audioSource.Play();
            }
        }
    }
}