using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemVideoFactory : CardItemFactory
{
    MagicWallManager _manager;

    public CardItemVideoFactory(MagicWallManager manager)
    {
        _manager = manager;
    }

    //
    //  生存公司卡片
    //
    public IList<CrossCardCellData> Generator(int id,CardAgent cardAgent)
    {
        List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

        // 根据公司ID 获取视频列表
        List<Video> videos = _manager.daoService.GetVideosByEnvId(id);


        for (int i = 0; i < videos.Count; i++)
        {
            Video e = videos[i];
            CrossCardCellData cd = new CrossCardCellData();
            cd.IsImage = false;
            cd.Description = e.Description;
            cd.Id = e.V_id;
            cd.Category = CrossCardCategoryEnum.VIDEO;
            cd.crossCardAgent = cardAgent as CrossCardAgent;
            cd.VideoUrl = e.Address;
            cd.magicWallManager = _manager;

            // 设置video的封面
            string address = e.Cover;
            cd.Image = address;
            
            _cellDatas.Add(cd);
        }

        return _cellDatas;
    }
}
