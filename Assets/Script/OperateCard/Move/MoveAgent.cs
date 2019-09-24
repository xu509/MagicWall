using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {
    public class MoveAgent : MonoBehaviour
    {

        private bool _show = false;

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
            gameObject.SetActive(true);
            _show = true;
        }

        public void Hide() {
            gameObject.SetActive(false);
            _show = false;
        }


    }
}


