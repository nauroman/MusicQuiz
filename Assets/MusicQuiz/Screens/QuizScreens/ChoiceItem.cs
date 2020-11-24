using System;
using MusicQuiz.Data;
using TMPro;
using UnityEngine;

namespace MusicQuiz.Screens.QuizScreens
{
    public class ChoiceItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title = default;
        [SerializeField] private TMP_Text _artist = default;

        public event Action<ChoiceItem> Clicked = delegate(ChoiceItem playlistItem) { };

        private Choice _choice;

        public Choice Choice
        {
            get => _choice;
            set
            {
                if (_choice != value)
                {
                    _choice = value;
                    _title.text = value.Title;
                    _artist.text = value.Artist;
                }
            }
        }

        public void Click()
        {
            Clicked(this);
        }
    }
}