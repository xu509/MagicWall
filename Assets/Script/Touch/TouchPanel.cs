using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace MagicWall
{
    public class TouchPanel : MonoBehaviour
    {
        [SerializeField] TouchAgent touchAgent;
        [SerializeField] RectTransform context;
        [SerializeField] float _createInterval;

        private float _lastCreateTime = 0;

        public void Update()
        {


            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {

                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        Vector2 position = Input.GetTouch(i).position;
                        CreatePoint(position);
                    }

                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Vector2 position = Input.mousePosition;
                CreatePoint(position);
            }




        }


        public void CreatePoint(Vector2 position)
        {
            if (Time.time - _lastCreateTime > _createInterval)
            {
                StartCoroutine(show(position));  //开始协程
                _lastCreateTime = Time.time;
            }



        }



        IEnumerator show(Vector2 position)  //协程方法
        {
            yield return new WaitForSeconds(0.2f);  //暂停协程，2秒后执行之后的操作
            TouchAgent agent = Instantiate(touchAgent, context);
            agent.GetComponent<RectTransform>().anchoredPosition = position;

        }

    }
}