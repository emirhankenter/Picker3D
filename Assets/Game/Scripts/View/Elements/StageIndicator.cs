using System.Collections.Generic;
using Game.Scripts.Behaviours;
using Game.Scripts.Controllers;
using Game.Scripts.Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.Elements
{
    public class StageIndicator : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private Text _currentLevelText;
        [SerializeField] private Text _nextLevelText;
        [SerializeField] private StageIndicatorElement _stageIndicatorElementPrefab;

        private List<StageIndicatorElement> _stageIndicatorElements = new List<StageIndicatorElement>();

        public void Init()
        {
            _currentLevelText.text = PlayerData.Instance.PlayerLevel.ToString();
            _nextLevelText.text = (PlayerData.Instance.PlayerLevel + 1).ToString();

            Recycle();

            var count = GameController.Instance.CurrentLevel.StageCount - 1; // -1 is for final stage (taptap stage)

            for (int i = 0; i < count; i++)
            {
                var element = Instantiate(_stageIndicatorElementPrefab, _content);
                element.transform.SetSiblingIndex(i+1);
                element.ToggleState(false);
                _stageIndicatorElements.Add(element);
            }

            LevelBehaviour.StageCompleted += OnStageCompleted;
        }

        public void Dispose()
        {
            LevelBehaviour.StageCompleted -= OnStageCompleted;

            Recycle();
        }

        private void Recycle()
        {
            foreach (var element in _stageIndicatorElements)
            {
                Destroy(element.gameObject);
            }
            _stageIndicatorElements.Clear();
        }

        private void OnStageCompleted()
        {
            if (GameController.Instance.CurrentLevel.CurrentStageIndex >= _stageIndicatorElements.Count) return;
            _stageIndicatorElements[GameController.Instance.CurrentLevel.CurrentStageIndex].ToggleState(true);
        }
    }
}
