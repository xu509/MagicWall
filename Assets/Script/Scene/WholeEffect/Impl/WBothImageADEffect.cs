using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall { 
    public class WBothImageADEffect : MonoBehaviour,IWholeEffect
    {
        [SerializeField] ImageBothSideController _imageBothSideController;
        [SerializeField] DaoTypeEnum _daoType;

        WholeEffectManager _wholeEffectManager;
        MagicWallManager _manager;


        bool _hasInit = false;


        public void End()
        {
            //throw new System.NotImplementedException();
        }

        public void Init(WholeEffectManager wholeEffectManager)
        {
            _wholeEffectManager = wholeEffectManager;
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();
            _imageBothSideController.Init(_manager, _daoType);
        }

        public void Run()
        {
            if (!_hasInit) {
                _imageBothSideController.StartPlay();
                _hasInit = true;
            }

            
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}