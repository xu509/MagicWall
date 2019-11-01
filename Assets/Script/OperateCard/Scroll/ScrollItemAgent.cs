using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagicWall {
    public class ScrollItemAgent : MonoBehaviour
    {
        ScrollData _data;
        Action<string> _onClickScale;

        MagicWallManager _manager;

        [SerializeField] Image _cover;
        [SerializeField] RectTransform _scaleBtn;
        [SerializeField] RectTransform _likeContainer;
        [SerializeField] RectTransform _videoContainer;

        [SerializeField] ButtonLikeAgent _buttonLikeAgent;


        void Awake() {
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();            
        }


        public void Init(ScrollData scrollData,Action<string> onClickScale)
        {
            gameObject.name = scrollData.Description;
            _data = scrollData;
            _onClickScale = onClickScale;

            // 视频
            if (scrollData.Type == 1)
            {
                _videoContainer.gameObject.SetActive(true);
            }
            else {
                _cover.sprite = SpriteResource.Instance.GetData(MagicWallManager.FileDir + scrollData.Cover);
                _videoContainer.gameObject.SetActive(false);
            }

            var imageUrl = scrollData.Cover;

            _buttonLikeAgent.Init(_manager.daoServiceFactory.GetLikes(imageUrl), () =>
            {
                _manager.daoServiceFactory.UpdateLikes(imageUrl);                
            });

        }

        void Update() {
            ScrollPanelAgent panelAgent = GetComponentInParent<ScrollPanelAgent>();
            if (panelAgent.currentLocation == PanelLocationEnum.Middle)
            {
                ShowComponents();
            }
            else {
                HideComponents();
            }

        }


        public void ShowComponents() {
            _scaleBtn.gameObject.SetActive(true);
            _likeContainer.gameObject.SetActive(true);
        }

        public void HideComponents() {
            _scaleBtn.gameObject.SetActive(false);
            _likeContainer.gameObject.SetActive(false);
        }


        public void DoScale() {

            Debug.Log("DO SCALE");

            _onClickScale.Invoke(_data.Cover);
        }

        public void DoLike() {



        }            


    }

}
