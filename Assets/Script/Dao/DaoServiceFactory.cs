using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {

    public class DaoServiceFactory : MonoBehaviour
    {
        [SerializeField]
        MockSceneConfig _mockSceneConfig;

        [SerializeField, Header("Data Service")] MockDaoService _mockDaoService;
        [SerializeField] MockFeiyueDaoService _mockFeiyueDaoService;
        [SerializeField, Tooltip("智博会 - 飞越体感")] MockZBHFeiyueDaoService _mockZBHFeiyueDaoService;
        [SerializeField, Tooltip("智博会 - 奉贤企业")] MockZBHFengxianDaoService _mockZBHFengxianDaoService;
        [SerializeField, Tooltip("智博会 - 土布")] MockZBHTubuDaoService _mockZBHTubuDaoService;

        [SerializeField, Tooltip("智博会 - 爱企")] MockZBHAiqiguDaoService _mockZBHAiqiguDaoService;
        [SerializeField] MockZhichengDaoService _mockZhichengDaoService;
        [SerializeField] MockShicunDaoService _mockShicunDaoService;
        [SerializeField] MockTestDaoService _mockTestDaoService;
        [SerializeField, Tooltip("虹口 - 飞越体感")] MockHKFeiyueDaoService _mockHKFeiyueDaoService;
        [SerializeField, Tooltip("虹口 - 爱企谷Logo")] MockHKLogoDaoService _mockHKLogoDaoService;
        [SerializeField, Tooltip("虹口 -  爱企谷照片墙")] MockHKPictureDaoService _mockHKPictureDaoService;
        [SerializeField, Tooltip("虹口 -  爱企")] MockHKAiqiguDaoService _mockHKAiqiguDaoService;

        [SerializeField, Tooltip("爱企谷 -  爱企")] MockAQGAiqiguDaoService _mockAQGAiqiguDaoService;

        [SerializeField] DaoService _realDaoService;


        public IDaoService GetDaoService(DaoTypeEnum type)
        {
            IDaoService _daoService = null;

            if (type == DaoTypeEnum.CBHAiqigu)
            {
                _daoService = _mockZBHAiqiguDaoService; // 暂缺
            }
            else if (type == DaoTypeEnum.CBHFeiyue)
            {
                _daoService = _mockZBHFeiyueDaoService;
            }
            else if (type == DaoTypeEnum.CBHTubu)
            {
                _daoService = _mockZBHTubuDaoService;
            }
            else if (type == DaoTypeEnum.CBHFengxian)
            {
                _daoService = _mockZBHFengxianDaoService;
            }
            else if (type == DaoTypeEnum.ShiCunFeiyue)
            {
                _daoService = _mockFeiyueDaoService;
            }
            else if (type == DaoTypeEnum.ShiCunZhicheng)
            {
                _daoService = _mockShicunDaoService;
            }
            else if (type == DaoTypeEnum.ShiCunShiCun)
            {
                _daoService = _mockShicunDaoService;
            }
            else if (type == DaoTypeEnum.Test) {
                _daoService = _mockTestDaoService; 
            }
            else if (type == DaoTypeEnum.HongKouFeiyue)
            {
                _daoService = _mockHKFeiyueDaoService;
            }
            else if (type == DaoTypeEnum.HongKouLogo)
            {
                _daoService = _mockHKLogoDaoService;
            }
            else if (type == DaoTypeEnum.HongKouPicture)
            {
                _daoService = _mockHKPictureDaoService;
            }
            else if (type == DaoTypeEnum.HongKouAiqigu)
            {
                _daoService = _mockHKAiqiguDaoService;
            }
            else if (type == DaoTypeEnum.AQGAiqigu)
            {
                _daoService = _mockAQGAiqiguDaoService;
            }
            return _daoService;
        }

        public MWConfig GetConfig() {

            return new MWConfig();

        }

        public List<SceneConfig> GetShowConfigs() {

            List<SceneConfig> items = new List<SceneConfig>();

            var sceneConfigs = _mockSceneConfig.sceneConfigs;

            for (int i = 0; i < sceneConfigs.Count; i++)
            {
                var scene = sceneConfigs[i].sceneType;
                var data = sceneConfigs[i].dataType;


                if (scene == SceneTypeEnum.Stars && data == DataTypeEnum.Enterprise)
                {
                    continue;
                }

                if (scene == SceneTypeEnum.FrontBackUnfold && data == DataTypeEnum.Enterprise)
                {
                    continue;
                }


                items.Add(sceneConfigs[i]);
            }

            return items;
        }

        public int GetLikes(string path)
        {
            var likes = TheDataSource.Instance.GetLikeDataBase();
            int r = 0;

            for (int i = 0; i < likes.list.Count; i++)
            {
                var like = likes.list[i];
                if (like.Path == path)
                {
                    r = like.Number;
                    break;
                }
            }

            return r;
        }

        public bool UpdateLikes(string path)
        {
            //Debug.Log("更新喜欢数:" + path);
            try
            {
                var likes = TheDataSource.Instance.GetLikeDataBase();

                bool hasPath = false;

                for (int i = 0; i < likes.list.Count; i++)
                {
                    var like = likes.list[i];
                    if (like.Path == path)
                    {
                        hasPath = true;
                        like.Number = like.Number + 1;
                        break;
                    }
                }

                if (!hasPath)
                {
                    var like = new Like();
                    like.Path = path;
                    like.Number = 1;
                    likes.list.Add(like);
                }


                TheDataSource.Instance.SaveLikes();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            finally
            {

            }

            return false;
        }

        public List<SearchBean> Search(string keys) {

            List<SearchBean> datas = new List<SearchBean>();
            if (keys == null || keys.Length == 0)
            {
                return datas;
            }
            var sceneConfigs = _mockSceneConfig.sceneConfigs;
            Debug.Log("搜索KEYS ：" + keys + "  sceneConfigs: " + sceneConfigs.Count);

            //TOTO 目前搜索了所有sceneConfigs，所以搜索结果会出现重复的内容
            //for (int i = 0; i < sceneConfigs.Count; i++)
            //{
            //    var dataTypeEnum = sceneConfigs[i].daoTypeEnum;
            //    var service = GetDaoService(dataTypeEnum);

            //    var result = service.Search(keys);

            //    if (result != null)
            //    {
            //        datas.AddRange(result);
            //    }
            //}


            //这个方法不会搜索到重复的内容，但是滚动条及搜索结果显示需要修改
            List<SceneConfig> configs = new List<SceneConfig>();
            for (int i = 0; i < sceneConfigs.Count; i++)
            {
                bool isIn = false;
                foreach (var item in configs)
                {
                    if (item.dataType == sceneConfigs[i].dataType)
                    {
                        isIn = true;
                        break;
                    }
                }
                if (!isIn)
                {
                    configs.Add(sceneConfigs[i]);
                }
            }
            for (int i = 0; i < configs.Count; i++)
            {
                var dataTypeEnum = configs[i].daoTypeEnum;
                var service = GetDaoService(dataTypeEnum);

                var result = service.Search(keys);

                if (result != null && result.Count > 40) {
                    result.RemoveRange(20, result.Count - 41);
                }


                if (result != null )
                {
                    datas.AddRange(result);
                }
            }

            return datas;
        }



    }

}
