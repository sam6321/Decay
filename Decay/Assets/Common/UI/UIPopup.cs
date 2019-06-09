using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    /// <summary>
    /// Pops up a UI element when this object is moused over
    /// </summary>
    public class UIPopup : MonoBehaviour
    {
        [Serializable]
        public class OnPopupEvent : UnityEvent<bool, GameObject, UIPopup> { }

        /// <summary>
        /// Canvas to instantiate popup elements into. 
        /// This is not required if 'instance' is set
        /// </summary>
        [SerializeField]
        private Transform canvas = null;

        /// <summary>
        /// The UI object prefab to instantiate at start time.
        /// If this is set, it will be instantiated on Start, regardless of whether 
        /// an instance is already set.
        /// </summary>
        [SerializeField]
        private GameObject prefab = null;

        /// <summary>
        /// The UI popup instance to show.
        /// </summary>
        [SerializeField]
        private GameObject instance = null;

        /// <summary>
        /// The offset, in screen pixels, to put the popup
        /// </summary>
        [SerializeField]
        private Vector2 offset = new Vector2(0, 0);

        /// <summary>
        /// If true, always show the popup instead of on mouse over
        /// </summary>
        [SerializeField]
        private bool alwaysShow = false;

        /// <summary>
        /// See <see cref="alwaysShow"/>
        /// </summary>
        public bool AlwaysShow
        {
            get => alwaysShow;
            set
            {
                alwaysShow = value;
                if(alwaysShow && instance)
                {
                    instance.SetActive(true);
                }
            }
        }

        /// <summary>
        /// An optional list of occluders. 
        /// If any of these are active, the popup will not be shown when moused over
        /// </summary>
        [SerializeField]
        private List<GameObject> occluders;

        /// <summary>
        /// An event fired whenever the popup appears or disappears.
        /// First parameter is the popup state, second parameter is the popup instance, third parameter is this UIPopup instance.
        /// </summary>
        [SerializeField]
        private OnPopupEvent onPopup = new OnPopupEvent();

        /// <summary>
        /// See <see cref="onPopup"/>
        /// </summary>
        public OnPopupEvent OnPopup => onPopup;

        /// <summary>
        /// Get the popup instance
        /// </summary>
        public GameObject Instance => instance;

        private void Start()
        {
            if(prefab)
            {
                instance = Instantiate(prefab, canvas);
            }
            
            if (!alwaysShow)
            {
                SetInstanceActive(false);
            }
        }

        private void OnEnable()
        {
            if(alwaysShow)
            {
                SetInstanceActive(true);
            }
        }

        private void OnDisable()
        {
            Hide();
        }

        private void LateUpdate()
        {
            if (alwaysShow)
            {
                instance.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            }
        }

        private void OnDestroy()
        {
            if(prefab)
            {
                Destroy(instance);
            }
        }

        private void OnMouseOver()
        {
            Show();
            instance.transform.position = (Vector2)Input.mousePosition + offset;
        }

        private void OnMouseExit()
        {
            Hide();
        }

        private void Show()
        {
            if (!alwaysShow && !instance.activeSelf && !occluders.Any(o => o.activeSelf))
            {
                SetInstanceActive(true);
            }
        }

        private void Hide()
        {
            if (!alwaysShow && instance.activeSelf)
            {
                SetInstanceActive(false);
            }
        }

        private void SetInstanceActive(bool active)
        {
            if (active != instance.activeSelf)
            {
                onPopup.Invoke(active, instance, this);
                instance.SetActive(active);
            }
        }
    }
}
