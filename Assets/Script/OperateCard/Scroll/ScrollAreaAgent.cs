using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MagicWall {
    public class ScrollAreaAgent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        private Vector2 _startDragPoint;
        private Action<ScrollDirectionEnum> _onRecognizeDirection;

        private bool _hasInit = false;



        public void Init(Action<ScrollDirectionEnum> onRecognizeDirection) {
            _onRecognizeDirection = onRecognizeDirection;
            _hasInit = true;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {            
            _startDragPoint = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_hasInit)
                return;

            var offset = eventData.position - _startDragPoint;
            FindDirection(offset);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }


        private void FindDirection(Vector2 offset) {
            bool isHor = false;

            if ((Mathf.Abs(offset.x) - Mathf.Abs(offset.y)) > 0)
            {
                isHor = true;
            }
            else {
                isHor = false;
            }


            if (isHor)
            {
                if (offset.x > 0)
                {
                    _onRecognizeDirection.Invoke(ScrollDirectionEnum.Right);
                }
                else
                {
                    _onRecognizeDirection.Invoke(ScrollDirectionEnum.Left);
                }
            }
            else {
                if (offset.y > 0)
                {
                    _onRecognizeDirection.Invoke(ScrollDirectionEnum.Top);

                }
                else
                {
                    _onRecognizeDirection.Invoke(ScrollDirectionEnum.Bottom);

                }
            }



        }


    }
}


