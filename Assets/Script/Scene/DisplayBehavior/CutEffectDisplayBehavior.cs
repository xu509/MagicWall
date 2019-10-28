using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MagicWall
{
    public interface CutEffectDisplayBehavior
    {
        void Init(DisplayBehaviorConfig displayBehaviorConfig);

        void Run();
    }

}