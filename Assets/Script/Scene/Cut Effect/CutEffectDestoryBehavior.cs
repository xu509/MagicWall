using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MagicWall
{
    public interface CutEffectDestoryBehavior
    {
        void Init(MagicWallManager manager, Action onDestoryCompleted);

        void Run();
    }

}