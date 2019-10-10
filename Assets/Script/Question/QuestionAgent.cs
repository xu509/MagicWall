using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace MagicWall
{
    public class QuestionAgent : MonoBehaviour
    {
        [SerializeField] Image _image;

        Action _onCloseAction;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(Action onCloseAction)
        {
            _onCloseAction = onCloseAction;
        }

        public void ShowReminder(QuestionTypeEnum questionTypeEnum)
        {
            UpdateContent(questionTypeEnum);
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        public void CloseReminder()
        {
            gameObject.SetActive(false);
            _onCloseAction.Invoke();
        }

        public void DoClose()
        {
            CloseReminder();
        }



        void UpdateContent(QuestionTypeEnum questionTypeEnum)
        {

            // 设置图片
            SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("SpriteAtlas/QuestionAtlas");

            if (questionTypeEnum == QuestionTypeEnum.CrossCard)
            {
                _image.sprite = spriteAtlas.GetSprite("help-crosscard");
            }
            else if (questionTypeEnum == QuestionTypeEnum.SliceCard)
            {
                _image.sprite = spriteAtlas.GetSprite("help-slicecard-3");
            }
            else if (questionTypeEnum == QuestionTypeEnum.SliceCardFour)
            {
                _image.sprite = spriteAtlas.GetSprite("help-slicecard-4");
            }
            else if (questionTypeEnum == QuestionTypeEnum.SingleCard)
            {
                _image.sprite = spriteAtlas.GetSprite("help-singlecard");
            }
            else if (questionTypeEnum == QuestionTypeEnum.SearchPanel)
            {
                _image.sprite = spriteAtlas.GetSprite("help-search");
            }
            else if (questionTypeEnum == QuestionTypeEnum.SearchResultPanel)
            {
                _image.sprite = spriteAtlas.GetSprite("help-searchresult");
            }
            else if (questionTypeEnum == QuestionTypeEnum.ScalePanel)
            {
                _image.sprite = spriteAtlas.GetSprite("help-scale");
            }

        }


    }
}