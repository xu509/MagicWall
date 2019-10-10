using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MagicWall
{
    interface CutEffectDisplayBehavior
    {
        void Init(DisplayBehaviorConfig displayBehaviorConfig);

        void Run();
    }

}