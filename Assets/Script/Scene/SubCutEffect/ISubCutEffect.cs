using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall { 
    public interface ISubCutEffect
    {
        void Init();

        void Run();

        void Stop();

    }
}