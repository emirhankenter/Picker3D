using System;
using Mek.Utilities;
using UnityEngine;

namespace MekNavigation
{
    public class BaseUiManager : MonoBehaviour
    {
        [Serializable] public class ContentDictionary : SerializableDictionary<string, ContentBase> { };

        public ContentDictionary Contents;

        private ContentBase _activeView;

        public bool Open(ViewParams viewParams)
        {
            if (_activeView != null)
            {
                Debug.LogError($"There is an active content with name: {_activeView.GetType()}");
                return false;
            }
            if (Contents.TryGetValue(viewParams.Key, out ContentBase content))
            {
                content.Open(viewParams);
                _activeView = content;
                return true;
            }
            return false;
        }

        public bool Open(string viewType)
        {
            if (_activeView != null)
            {
                Debug.LogError($"There is an active content with name: {_activeView.GetType()}");
                return false;
            }
            if (Contents.TryGetValue(viewType, out ContentBase content))
            {
                content.Open(new ViewParams(viewType));
                _activeView = content;
                return true;
            }

            return false;
        }

        public bool Change(ViewParams viewParams)
        {
            return Change(viewParams.Key);
        }
        public bool Change(string viewType)
        {
            if (_activeView != null)
            {
                _activeView.Closed += OnViewClosed;
                _activeView.Close();
            }

            void OnViewClosed()
            {
                _activeView.Closed -= OnViewClosed;
                _activeView = null;
            }

            return Open(viewType);
        }

        public void CloseActiveContent()
        {
            if (_activeView == null)
            {
                Debug.LogError($"There is not an active content!");
            }
            else
            {
                _activeView.Close();
                _activeView = null;
            }
        }

        #region Singleton

        private static BaseUiManager _baseUiManager;

        public static BaseUiManager Instance
        {
            get
            {
                if (_baseUiManager == null)
                {
                    _baseUiManager = FindObjectOfType<BaseUiManager>();
                }
                return _baseUiManager;
            }
        }

        #endregion

    }
}