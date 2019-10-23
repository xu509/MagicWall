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
        [SerializeField] MockZhichengDaoService _mockZhichengDaoService;
        [SerializeField] MockShicunDaoService _mockShicunDaoService;
        [SerializeField] DaoService _realDaoService;


        public IDaoService GetDaoService(DaoTypeEnum type)
        {
            IDaoService _daoService = null;

            if (type == DaoTypeEnum.CBHAiqigu)
            {
                _daoService = null; // 暂缺
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



    }

}
