using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagicWall {
    public class ScrollItemAgent : MonoBehaviour
    {
        [SerializeField] Image _cover;

        public void Init(ScrollData scrollData)
        {
            _cover.sprite = SpriteResource.Instance.GetData(MagicWallManager.FileDir +  scrollData.Cover);
            gameObject.name = scrollData.Description;

        }




    }

}
