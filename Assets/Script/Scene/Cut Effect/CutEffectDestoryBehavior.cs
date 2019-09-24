using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MagicWall
{
    interface CutEffectDestoryBehavior
    {
        void Init(MagicWallManager manager, float destoryDurTime);

        void Run();
    }

}