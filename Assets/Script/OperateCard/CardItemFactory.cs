using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall { 

    public interface CardItemFactory
{
    IList<CrossCardCellData> Generator(int id, CardAgent cardAgent);
}
}