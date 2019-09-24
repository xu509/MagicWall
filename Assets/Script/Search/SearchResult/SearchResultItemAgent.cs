using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  搜索 Items Agent
/// </summary>
namespace MagicWall
{
    public class SearchResultItemAgent : MonoBehaviour
    {
        [SerializeField] RawImage _image;

        private SearchBean _searchBean;
        private int _id; // id
        private DataTypeEnum _type;   //  类型
        private MagicWallManager _manager;  //  主管理器

        Action<SearchBean> _OnClickItem;


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init()
        {

        }

        public void InitData(SearchBean searchBean, MagicWallManager manager)
        {
            _manager = manager;

            string address = MagicWallManager.FileDir + searchBean.cover;
            _image.texture = TextureResource.Instance.GetTexture(address);

            _id = searchBean.id;
            _type = searchBean.type;

            _searchBean = searchBean;



        }

        /// <summary>
        /// 点击
        /// </summary>
        public void DoClick()
        {
            //Debug.Log()


            //  点开卡片

            //_itemsFactory.GenerateCardAgent();

            _OnClickItem.Invoke(_searchBean);

            //  关闭原来的搜索框

        }

        #region 配置事件
        public void SetOnClickItem(Action<SearchBean> action)
        {
            _OnClickItem = action;

        }
        #endregion

    }
}