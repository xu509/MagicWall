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

        float _imageWidth;
        float _imageHeight;

        MagicWallManager _manager;

        [SerializeField] Image _cover;
        [SerializeField] RectTransform _scaleBtn;
        [SerializeField] RectTransform _likeContainer;
        [SerializeField] RectTransform _videoContainer;

        [SerializeField] ButtonLikeAgent _buttonLikeAgent;


        void Awake() {
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();

            if (_manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            {
                _imageWidth = 600f;
                _imageHeight = 400f;
            }
            else if (_manager.screenTypeEnum == ScreenTypeEnum.Screen720P) {
                _imageWidth = 600f * 9f / 16f;
                _imageHeight = 400f * 16f / 9f;
            }
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
                _videoContainer.gameObject.SetActive(false);
            }

            var sprite = SpriteResource.Instance.GetData(MagicWallManager.FileDir + scrollData.Cover);
            SetUpImage(sprite);
            //_cover.sprite = SpriteResource.Instance.GetData(MagicWallManager.FileDir + scrollData.Cover);


            var imageUrl = scrollData.Cover;

            _buttonLikeAgent.Init(_manager.daoServiceFactory.GetLikes(imageUrl), () =>
            {
                _manager.daoServiceFactory.UpdateLikes(imageUrl);                
            });

        }

        void Update() {
            ScrollPanelAgent panelAgent = GetComponentInParent<ScrollPanelAgent>();

            if (panelAgent != null)
            {
                if (panelAgent.currentLocation == PanelLocationEnum.Middle)
                {
                    ShowComponents();
                }
                else
                {
                    HideComponents();
                }
            }

            else {
                SliceScrollPanelAgent sliceScrollPanelAgent = GetComponentInParent<SliceScrollPanelAgent>();
                if (sliceScrollPanelAgent != null) {
                    if (sliceScrollPanelAgent.currentLocation == PanelLocationEnum.Middle)
                    {
                        ShowComponents();
                    }
                    else
                    {
                        HideComponents();
                    }

                }
            }
        }


        public void ShowComponents() {
            if (_data.Type != 1)
            {
                _scaleBtn.gameObject.SetActive(true);
            }        
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




        private void SetUpImage(Sprite sprite)
        {
            float w = sprite.texture.width;
            float h = sprite.texture.height;

            float width;
            float height;

            width = _imageWidth;
            height = h / w * width;

            if (height < _imageHeight) {
                height = _imageHeight;
                width = _imageHeight * w / h;
            }

            // 得到图片的新长宽
            _cover.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

            sprite.texture.wrapMode = TextureWrapMode.Clamp;

            Debug.Log("图片原始尺寸： " + w + " * " + h + " ==> 修改后 " + width + "*" + height);

            //_imageWidth = width;
            //_imageHeight = height;

            _cover.sprite = sprite;
        }


    }

}
