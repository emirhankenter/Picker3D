using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MekNavigation
{
    public class ContentBase : MonoBehaviour
    {
        public Action Opened; 
        public Action Closed; 

        public virtual void Open(ViewParams viewParams)
        {
            gameObject.SetActive(true);
            Opened?.Invoke();
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
            Closed?.Invoke();
        }
    }
}