using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    public class CardItemActivityFactory : CardItemFactory
    {
        MagicWallManager _manager;

        public CardItemActivityFactory(MagicWallManager manager)
        {
            _manager = manager;
        }

        //
        //  生成活动
        //
        public IList<CrossCardCellData> Generator(int id, CardAgent cardAgent)
        {
            var activities = _manager.daoService.GetActivitiesByEnvId(id);
            List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

            for (int i = 0; i < activities.Count; i++)
            {
                Activity e = activities[i];
                CrossCardCellData cd = new CrossCardCellData();
                cd.magicWallManager = _manager;

                string address = e.Image;
                cd.Image = address;
                cd.Description = e.Name;

                cd.IsImage = true;
                cd.Id = e.Ent_id;
                cd.Category = CrossCardCategoryEnum.ACTIVITY;
                cd.crossCardAgent = cardAgent as CrossCardAgent;
                _cellDatas.Add(cd);
            }

            return _cellDatas;
        }
    }
}