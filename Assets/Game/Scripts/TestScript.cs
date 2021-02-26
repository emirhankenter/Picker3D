using System.Collections;
using UnityEngine;
using System;
using Game.Scripts.Models;
using Mek.Extensions;
using Mek.Localization;
using Mek.Models;
using Mek.Utilities;
using MekCoroutine;
using MekNavigation;
using Sirenix.OdinInspector;

namespace Game.Scripts
{
    public struct TestStruct
    {
        public int Number;
    }
    public class TestScript : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleA;
        [SerializeField] private ParticleSystem _particleB;

        private const string CoroutineKey = "TestRoutine";

        private MekLog Log => new MekLog(nameof(TestScript), DebugLevel.Debug);

        private void Start()
        {
            //PlayerData.Prefs[PrefStats.PlayerLevel].Changed += OnPlayerLevelChanged;
        }

        private void OnDestroy()
        {
            //PlayerData.Prefs[PrefStats.PlayerLevel].Changed -= OnPlayerLevelChanged;
        }

        private void OnPlayerLevelChanged()
        {
            Log.Info($"Player Level Changed to: {PlayerData.Instance.PlayerLevel}");
        }


        private IEnumerator TestRoutine()
        {
            yield return new WaitForSeconds(1f);

            Log.Debug(CoroutineKey);
        }

        private void OnApplicationQuit()
        {
            //PlayerData.Instance.LastActive = DateTime.Now;
        }

        [Button]
        private void ParticleA()
        {
            _particleA.Spawn(Vector3.zero, Quaternion.identity);
        }
        [Button]
        private void ParticleB()
        {
            _particleB.Spawn(Vector3.zero, Quaternion.identity);
        }

        [Button]
        private void OpenMyPanel()
        {
            Navigation.Panel.Open(ViewTypes.InGamePanel);
        }
        [Button]
        private void CloseMyPanel()
        {
            Navigation.Panel.CloseActiveContent();
        }

        [Button]
        private void OpenMyPopup()
        {
            Navigation.Popup.Open(ViewTypes.MyPopup);
        }
        [Button]
        private void CloseMyPopup()
        {
            Navigation.Popup.CloseActiveContent();
        }

        //[Button]
        //private void Set()
        //{
        //    PlayerData.Instance.MyTest = new TestStruct(){Number = 123123};
        //}

        //[Button]
        //private void Get()
        //{
        //    //var myTest = PlayerData.Instance.MyTest;
        //    //Debug.LogError($"{myTest.Number}");

        //    //Debug.LogError($"{PlayerData.Instance.PlayerLevel}");
        //}

        [Button]
        private void SetLanguage(Language lang)
        {
            LocalizationManager.SetLanguage(lang);
        }


        //[SerializeField] private TestObject _prefab;
        //[Button]
        //private void Spawn()
        //{
        //    var testObject = ObjectPooling.Instance.Spawn(_prefab);

        //    CoroutineController.DoAfterGivenTime(1, () =>
        //    {
        //        ObjectPooling.Instance.Recycle(testObject);
        //    });
        //}
    }
}