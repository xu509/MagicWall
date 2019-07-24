using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemFactoryInstance :MonoBehaviour
{

    private MagicWallManager _manager;

    public void Init(MagicWallManager manager) {
        _manager = manager;
    }


    public IList<CrossCardCellData> Generate(int id, CrossCardCategoryEnum categoryEnum,CardAgent cardAgent) {
        if (categoryEnum == CrossCardCategoryEnum.INDEX)
        {
            return new CardItemIndexFactory(_manager).Generator(id, cardAgent);
        }
        else if (categoryEnum == CrossCardCategoryEnum.PRODUCT)
        {
            return new CardItemProductFactory(_manager).Generator(id, cardAgent);
        }
        else if (categoryEnum == CrossCardCategoryEnum.ACTIVITY)
        {
            return new CardItemActivityFactory(_manager).Generator(id, cardAgent);
        }
        else if (categoryEnum == CrossCardCategoryEnum.CATALOG)
        {
            return new CardItemCatalogFactory(_manager).Generator(id, cardAgent);
        }
        else if (categoryEnum == CrossCardCategoryEnum.VIDEO)
        {
            return new CardItemVideoFactory(_manager).Generator(id, cardAgent);
        }
        return null;
    }

}
