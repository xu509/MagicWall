using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MagicWall {
    public class SliceScrollItemAgent : MonoBehaviour
    {
        ScrollData _data;
        Action<string> _onClickScale;

        float _minWidthImage;
        float _minHeightImage;
        float _maxHeightImage;

        private float _imageWidth;
        private float _imageHeight;

        private float _scaleAniTime = 0.1f;


        MagicWallManager _manager;

        [SerializeField] Image _cover;
        [SerializeField] RectTransform _photoframe;
        [SerializeField] RectTransform _scaleBtn;
        [SerializeField] RectTransform _likeContainer;
        [SerializeField] RectTransform _videoContainer;

        [SerializeField] ButtonLikeAgent _buttonLikeAgent;


        void Awake() {
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();

            if (_manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            {
                _minWidthImage = 600f;
                _minHeightImage = 400f;
                _maxHeightImage = 900f;
            }
            else if (_manager.screenTypeEnum == ScreenTypeEnum.Screen720P) {
                _minWidthImage = 600f * 0.66f;
                _minHeightImage = 400f * 0.66f;
                _maxHeightImage = 900f * 0.66f;
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

            // 设置封面图片
            var coverSprite = SpriteResource.Instance.GetData(MagicWallManager.FileDir + scrollData.Cover);
            SetUpImage(coverSprite);


            // 设置喜欢控件
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


        private void SetUpImage(Sprite sprite) {
            float w = sprite.texture.width;
            float h = sprite.texture.height;

            float width;
            float height;


            if (w < _minWidthImage)
            {
                height = h / w * _minWidthImage;
                if (height < _minHeightImage)
                {
                    width = w / h * _minHeightImage;
                    height = _minHeightImage;
                }
                else
                {
                    width = _minWidthImage;
                }
            }
            else {
                height = h / w * _minWidthImage;

                if (height < _minHeightImage)
                {
                    width = w / h * _minHeightImage;
                    height = _minHeightImage;
                }
                else {
                    width = _minWidthImage;
                }
            }

            // 得到图片的新长宽
            _cover.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

            sprite.texture.wrapMode = TextureWrapMode.Clamp;

            //Debug.Log("图片原始尺寸： " + w + " * " + h + " ==> 修改后 " + width + "*" + height);

            _imageWidth = width;
            _imageHeight = height;

            _cover.sprite = sprite;
        }

        public void SetAsMiddle(Action onCompleted) {
            GetComponent<RectTransform>().DOScale(1f, _scaleAniTime);


            var width = _cover.GetComponent<RectTransform>().rect.width;
            var height = _cover.GetComponent<RectTransform>().rect.height;
            _photoframe.DOScale(1f, _scaleAniTime);


            if (height > _maxHeightImage) {
                height = _maxHeightImage;
            }

            _photoframe.DOSizeDelta(new Vector2(width, height), _scaleAniTime).OnComplete(() =>
            {
                onCompleted.Invoke();
            });

        }

        public void RecoverFrame() {
            _photoframe.sizeDelta = new Vector2(_minWidthImage, _minHeightImage);
            _photoframe.DOScale(0.8f, Time.deltaTime);
        }

        public Vector2 GetImageSize() {
            return new Vector2(_imageWidth, _imageHeight);
        }

    }

}
