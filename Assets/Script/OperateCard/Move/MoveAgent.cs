using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagicWall {
    public class MoveAgent : MonoBehaviour
    {
        [SerializeField] Image _upImg;
        [SerializeField] Image _downImg;
        [SerializeField] Image _leftImg;
        [SerializeField] Image _rightImg;


        private bool _show = false;
        private bool _init = false;
        private MagicWallManager _manager;

        public void Init() {
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();


            InitUI();
            _init = true;
        }


        public void Start()
        {
            gameObject.SetActive(false);
            _show = false;
        }



        public void ChangeStatus() {
            if (_show)
            {
                Hide();
            }
            else {
                Show();
            }
        }

        public void Show() {
            if (_init == false) {
                Init();
            }
            gameObject.SetActive(true);
            _show = true;
        }

        public void Hide() {
            if (_init == false)
            {
                Init();
            }
            gameObject.SetActive(false);
            _show = false;
        }

        private void InitUI() {
            _upImg.sprite = _manager.themeManager.GetService().GetMoveAgentSprite(MoveAgentTypeEnum.UP);
            _downImg.sprite = _manager.themeManager.GetService().GetMoveAgentSprite(MoveAgentTypeEnum.DOWN);
            _leftImg.sprite = _manager.themeManager.GetService().GetMoveAgentSprite(MoveAgentTypeEnum.LEFT);
            _rightImg.sprite = _manager.themeManager.GetService().GetMoveAgentSprite(MoveAgentTypeEnum.RIGHT);
        }


    }
}


