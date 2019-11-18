using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    public interface IWholeEffect 
    {
        void Init(WholeEffectManager wholeEffectManager);

        void Run();

        void End();


    }


}
