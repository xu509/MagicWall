using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {

    public class DaoServiceFactory : MonoBehaviour
    {
        [SerializeField]
        MockSceneConfig _mockSceneConfig;

        // Start is called before the first frame update
        [SerializeField, Header("Data Service")] MockDaoService _mockDaoService;
        [SerializeField] MockFeiyueDaoService _mockFeiyueDaoService;
        [SerializeField, Tooltip("智博会 - 飞越体感")] MockZBHFeiyueDaoService _mockZBHFeiyueDaoService;
        [SerializeField, Tooltip("智博会 - 奉贤企业")] MockZBHFengxianDaoService _mockZBHFengxianDaoService;
        [SerializeField, Tooltip("智博会 - 土布")] MockZBHTubuDaoService _mockZBHTubuDaoService;
        [SerializeField, Tooltip("智博会 - 土布")] MockZBHAiqiguDaoService _mockZBHAiqiguDaoService;
        [SerializeField] MockZhichengDaoService _mockZhichengDaoService;
        [SerializeField] MockShicunDaoService _mockShicunDaoService;
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
                var time = sceneConfigs[i].durtime;


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



    }

}
