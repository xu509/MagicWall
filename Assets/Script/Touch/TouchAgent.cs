using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace MagicWall
{
    public class TouchAgent : MonoBehaviour
    {
        //  存在时间
        [SerializeField] float alive_second = 0.1f;

        [SerializeField] Image image;


        //  生成时间
        float _gen_time;

        bool _init = false;


        // Start is called before the first frame update
        void Start()
        {
            _gen_time = Time.time;

            _init = true;

        }

        // Update is called once per frame
        void Update()
        {
            if (_init)
            {
                _init = false;


                GetComponent<RectTransform>()
                    .DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f)
                    .OnComplete(() =>
                    {
                        Destroy(gameObject);
                    });
            }


            //if ((Time.time - _gen_time) > alive_second) {


            //    // 进行销毁
            //    image.DOFade(0,1f).OnComplete(() => {
            //        Debug.Log("进行销毁");

            //        Destroy(gameObject);
            //    });
            //}

        }
    }
}