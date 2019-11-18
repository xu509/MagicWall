using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{

    public class WholeEffectManager : MonoBehaviour
    {
        [SerializeField, Header("开关")] bool _open;
        [SerializeField, Header("常驻效果类型")] WholeEffectEffectTypeEnum _type;

        [SerializeField] WholeEffectFactory wholeEffectFactory;

        private IWholeEffect _wholeEffect;
        private MagicWallManager _manager;


        public bool isOpen() {
            return _open;
        }


        public void Init(MagicSceneManager magicSceneManager) {

            _wholeEffect = wholeEffectFactory.GetWholeEffect(_type);
            _wholeEffect.Init(this);



        }

        public void Run() {
            _wholeEffect.Run();
        }


        public void End() {
            _wholeEffect.End();
        }

    }

}