using UnityEngine;

namespace MusicQuiz.Screens
{
    public class ActiveByScreenState : MonoBehaviour
    {
        [SerializeField] private ScreenState _screenState = ScreenState.None;

        private void Awake()
        {
            SelectedScreenState.Changed += SelectedScreenStateOnChanged;
            SelectedScreenStateOnChanged();
        }

        private void SelectedScreenStateOnChanged()
        {
            gameObject.SetActive(_screenState == SelectedScreenState.ScreenState);
        }
    }
}