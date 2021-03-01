using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Controllers;
using Mek.Extensions;
using MekCoroutine;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class FinalStage : StageBehaviour
    {
        public static event Action<int> Multiplied;

        [SerializeField] private List<MultiplierArea> _multiplierAreas;

        private PlayerController _player;

        private string _evaluationRoutineKey => $"evaluationRoutine{GetInstanceID()}";

        public override void Init()
        {
            _player = GameController.Instance.CurrentLevel.Player;
            base.Init();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (CoroutineController.IsCoroutineRunning(_evaluationRoutineKey))
            {
                CoroutineController.StopCoroutine(_evaluationRoutineKey);
            }
        }

        public override void StartEvaluation()
        {
            base.StartEvaluation();
            //Complete(true);

            CoroutineController.StartCoroutine(_evaluationRoutineKey, EvaluationRoutine());
        }

        private IEnumerator EvaluationRoutine()
        {
            while (Mathf.Abs(_player.Speed) >= 0.1f)
            {
                yield return new WaitForFixedUpdate();
            }

            var area = GetMultiplierArea();
            var multiplyAmount = area != null ? area.MultiplyAmount : 1;
            Debug.Log($"MultipliedBy: {multiplyAmount}");
            Multiplied?.Invoke(multiplyAmount);
            Complete(true);
        }

        private MultiplierArea GetMultiplierArea()
        {
            var area = _multiplierAreas
                .Where(a => a.Inside)
                .ToList();

            if (area.Count != 0)
            {
                var distanceList = area.OrderBy(a => Vector3.Distance(a.transform.position, _player.transform.position)).ToList();

                return distanceList.First();
            }
            else
            {
                return null;
            }
        }
    }
}
