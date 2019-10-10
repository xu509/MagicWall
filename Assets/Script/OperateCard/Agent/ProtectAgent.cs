using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MagicWall { 
    public class ProtectAgent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        Action _onUpdatedAction;

        private void Start()
        {
            DoClose();
        }


        public void DoActive(Action onUpdateAction) {
            _onUpdatedAction = onUpdateAction;
            gameObject.SetActive(true);
        }

        public void DoClose()
        {
            gameObject.SetActive(false);
        }

        private void IsUpdated() {
            //Debug.Log("ProtectAgent Is Updated");
            _onUpdatedAction.Invoke();
        }



        public void OnBeginDrag(PointerEventData eventData)
        {
            IsUpdated();
        }

        public void OnDrag(PointerEventData eventData)
        {
            IsUpdated();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsUpdated();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            IsUpdated();
        }
    }
}