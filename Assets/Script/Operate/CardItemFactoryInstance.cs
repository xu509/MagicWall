using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemFactoryInstance : Singleton<CardItemFactoryInstance>
{
    void Awake() { }

    protected CardItemFactoryInstance() { }

    public IList<CrossCardCellData> Generate(int id, CrossCardCategoryEnum categoryEnum) {
        if (categoryEnum == CrossCardCategoryEnum.INDEX)
        {
            return new CardItemIndexFactory().Generator(id);
        }
        else if (categoryEnum == CrossCardCategoryEnum.PRODUCT)
        {
            return new CardItemProductFactory().Generator(id);
        }
        else if (categoryEnum == CrossCardCategoryEnum.ACTIVITY)
        {
            return new CardItemActivityFactory().Generator(id);
        }
        else if (categoryEnum == CrossCardCategoryEnum.CATALOG)
        {
            return new CardItemCatalogFactory().Generator(id);
        }
        else if (categoryEnum == CrossCardCategoryEnum.VIDEO)
        {
            return new CardItemVideoFactory().Generator(id);
        }
        return null;
    }

}
