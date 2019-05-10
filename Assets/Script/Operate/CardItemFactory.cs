using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CardItemFactory 
{
    IList<CrossCardCellData> Generator(int id);
}
