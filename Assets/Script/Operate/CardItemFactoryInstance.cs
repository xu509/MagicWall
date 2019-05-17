using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemFactoryInstance : Singleton<CardItemFactoryInstance>
{
    void Awake() { }

    protected CardItemFactoryInstance() { }

    public IList<CrossCardCellData> Generate(int id, CrossCardCategoryEnum categoryEnum,CardAgent cardAgent) {
        if (categoryEnum == CrossCardCategoryEnum.INDEX)
        {
            return new CardItemIndexFactory().Generator(id, cardAgent);
        }
        else if (categoryEnum == CrossCardCategoryEnum.PRODUCT)
        {
            return new CardItemProductFactory().Generator(id, cardAgent);
        }
        else if (categoryEnum == CrossCardCategoryEnum.ACTIVITY)
        {
            return new CardItemActivityFactory().Generator(id, cardAgent);
        }
        else if (categoryEnum == CrossCardCategoryEnum.CATALOG)
        {
            return new CardItemCatalogFactory().Generator(id, cardAgent);
        }
        else if (categoryEnum == CrossCardCategoryEnum.VIDEO)
        {
            return new CardItemVideoFactory().Generator(id, cardAgent);
        }
        return null;
    }

}
