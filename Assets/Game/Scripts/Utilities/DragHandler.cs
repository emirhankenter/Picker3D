using System;
using System.Collections;
using Mek.Extensions;
using MekCoroutine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.Utilities
{
    public class DragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public static event Action<Vector2> PointerDown; 
        public static event Action<Vector2> PointerUp; 
        public static event Action<Vector2> DragBegin; 
        public static event Action<Vector2> DragEnd; 
        public static event Action<Vector2> Drag; 

        public static event Action<Vector2> MovePerformed; 

        private Image _image;
        private Image Image => _image ? _image : _image = GetComponent<Image>();

        public bool IsActive => Image.enabled;

        public bool IsPressing { get; private set; }
        public Vector2 MousePosition { get; private set; }
        private string _mouseEventRoutineKey => $"dragMouseEventRoutine{GetInstanceID()}";

        public void ToggleInput(bool state)
        {
            Image.enabled = state;
            CoroutineController.ToggleRoutine(state, _mouseEventRoutineKey, MouseEventRoutine());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(!IsActive) return;
            IsPressing = true;
            PointerDown?.Invoke(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsActive) return;
            IsPressing = false;
            PointerUp?.Invoke(eventData.position);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!IsActive) return;
            DragBegin?.Invoke(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!IsActive) return;
            DragEnd?.Invoke(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!IsActive) return;
            Drag?.Invoke(eventData.position);
        }

        private IEnumerator MouseEventRoutine()
        {
            while (true)
            {
                MousePosition = Input.mousePosition;

                if (IsPressing)
                {
                    MovePerformed?.Invoke(MousePosition);
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}
