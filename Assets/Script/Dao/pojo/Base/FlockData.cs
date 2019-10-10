using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     浮动块数据基类
/// </summary>

namespace MagicWall
{
    public abstract class FlockData : BaseData
    {
        public abstract Sprite GetCoverSprite();

        public abstract string GetCover();

        public abstract int GetId();

        public abstract DataTypeEnum GetDataType();


    }
}