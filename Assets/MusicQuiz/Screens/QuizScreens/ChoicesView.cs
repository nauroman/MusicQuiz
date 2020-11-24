using System;
using System.Collections.Generic;
using MusicQuiz.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace MusicQuiz.Screens.QuizScreens
{
    public class ChoicesView : MonoBehaviour
    {
        [SerializeField] private Transform _content = default;

        [FormerlySerializedAs("_item")] [SerializeField]
        private GameObject _choiceItem = default;

        [SerializeField] private GameObject _rightChoiceItem = default;
        [SerializeField] private GameObject _wrongChoiceItem = default;

        private readonly Dictionary<int, ChoiceItem> _instances = new Dictionary<int, ChoiceItem>();

        public event Action<Choice> Selected = delegate(Choice choice) { };

        private Choice[] _choices = default;

        public Choice[] Choices
        {
            set
            {
                if (_choices != value)
                {
                    _choices = value;
                    InstantiateChoices(_choices, _choiceItem, _content);
                }
            }
        }

        private void InstantiateChoices(Choice[] choices, GameObject playlistItemPrefab, Transform parent)
        {
            DestroyChildren();

            _instances.Clear();

            for (int i = 0; i < choices.Length; i++)
            {
                var choice = choices[i];
                choice.Index = i;
                InstantiateChoice(choice, playlistItemPrefab, parent);
            }
        }

        public void InstantiateRightChoice(Choice choice)
        {
            InstantiateChoice(choice, _rightChoiceItem, _content);
        }

        public void InstantiateWrongChoice(Choice choice)
        {
            InstantiateChoice(choice, _wrongChoiceItem, _content);
        }

        public void ShowRightChoice(int choiceIndex)
        {
            Replace(GetChoiceItem(choiceIndex), _rightChoiceItem);
        }

        public void ShowWrongChoice(int choiceIndex)
        {
            Replace(GetChoiceItem(choiceIndex), _wrongChoiceItem);
        }

        private ChoiceItem GetChoiceItem(int index)
        {
            return _instances[index];
        }

        private void Replace(ChoiceItem choiceItem, GameObject playlistItemPrefab)
        {
            var childIndex = choiceItem.transform.GetSiblingIndex();

            var rightChoiceItem = InstantiateChoice(choiceItem.Choice, playlistItemPrefab, _content);
            Destroy(choiceItem.gameObject);

            rightChoiceItem.transform.SetSiblingIndex(childIndex);
        }

        public void ReplaceByWrongChoiceItem(ChoiceItem choiceItem)
        {
            var childIndex = choiceItem.transform.GetSiblingIndex();

            var rightChoiceItem = InstantiateChoice(choiceItem.Choice, _wrongChoiceItem, _content);
            Destroy(choiceItem.gameObject);

            rightChoiceItem.transform.SetSiblingIndex(childIndex);
        }

        public void DestroyChildren()
        {
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
        }

        private ChoiceItem InstantiateChoice(Choice choice, GameObject playlistItemPrefab, Transform parent)
        {
            var go = Instantiate(playlistItemPrefab, parent);

            var choiceItem = go.GetComponent<ChoiceItem>();
            choiceItem.Choice = choice;

            _instances[choice.Index] = choiceItem;

            choiceItem.Clicked += PlaylistItemOnClicked;

            return choiceItem;
        }

        private void PlaylistItemOnClicked(ChoiceItem choiceItem)
        {
            Selected(choiceItem.Choice);
        }
    }
}