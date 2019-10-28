using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.U2D;
using System;

//
//   启动的场景 
//  - 需在此处完成数据模块的加载
//
namespace MagicWall
{
    public class StartScene : IScene
    {

        private bool _doLoadResourse = false;

        MagicSceneEnum _magicSceneEnumStatus;
        private StartSceneStatus _startSceneStatus;
        private static bool LOG = true;
        private IDaoService _daoService;
        private MagicWallManager _manager;

        Action _onRunCompleted;
        Action _onRunEndCompleted;
        Action _onSceneCompleted;


        enum StartSceneStatus {
            Init,
            showUI,
            showUICompleted,
            BeginLoadResource,
            LoadResourceCompleted,
            StartHideUI,
            HideUICompleted
        }


        public void Init(SceneConfig sceneConfig, MagicWallManager manager,Action onSceneCompleted)
        {
            _manager = manager;
            _startSceneStatus = StartSceneStatus.Init;
            _onSceneCompleted = onSceneCompleted;
        }


        public bool Run()
        {
            //DoDebug("Start Scene Is Start!");
            if (_startSceneStatus == StartSceneStatus.Init) {


                if (_manager.magicSceneManager.runLogoAni)
                {
                    _startSceneStatus = StartSceneStatus.showUI;
                    _manager.BgLogo.gameObject.SetActive(true);
                    _manager.BgLogo.GetComponent<Image>()
                        .DOFade(1, 1f)
                        .OnComplete(() =>
                        {
                            _startSceneStatus = StartSceneStatus.showUICompleted;
                        });
                }
                else {
                    _startSceneStatus = StartSceneStatus.showUICompleted;
                }
            }

            if (_startSceneStatus == StartSceneStatus.showUICompleted) {
                _startSceneStatus = StartSceneStatus.BeginLoadResource;

                // 读取配置表
                LoadConfig();

                // 加载资源
                LoadResource();
            }

            if (_startSceneStatus == StartSceneStatus.LoadResourceCompleted) {
                if (_manager.magicSceneManager.runLogoAni)
                {
                    _startSceneStatus = StartSceneStatus.StartHideUI;

                    _manager.BgLogo.GetComponent<Image>()
                        .DOFade(0, 1f)
                        .OnComplete(() =>
                        {
                            //_doHideLogoComplete = true;
                            // RunEnd();
                            _manager.BgLogo.gameObject.SetActive(false);
                            _startSceneStatus = StartSceneStatus.HideUICompleted;
                        });
                }
                else {
                    _startSceneStatus = StartSceneStatus.HideUICompleted;
                }
            }

            if (_startSceneStatus == StartSceneStatus.HideUICompleted)
            {
                _onSceneCompleted.Invoke();
                _startSceneStatus = StartSceneStatus.Init;
            }


            return true;
        }

        private void LoadResource()
        {


            var showConfigs = _manager.daoServiceFactory.GetShowConfigs();

            for (int i = 0; i < showConfigs.Count; i++) {
                var service = _manager.daoServiceFactory.GetDaoService(showConfigs[i].daoTypeEnum);

                service.InitData();

                var addresses = service.GetMatImageAddresses();

                foreach (string address in addresses)
                {
                    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                    watch.Start();

                    string imageAddress = MagicWallManager.FileDir + address;
                    //TextureResource.Instance.GetTexture(imageAddress);               
                    SpriteResource.Instance.GetData(imageAddress);

                    watch.Stop();

                    if ((watch.ElapsedMilliseconds / 1000f) > 0.5f)
                    {
                        Debug.Log("Time - " + imageAddress + " - second : " + watch.ElapsedMilliseconds / 1000f);
                    }

                }
            }



            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

           

            // 加载其他资源
            //  - 手写板用的texture
            Texture2D writePanelWordPanel = new Texture2D(_manager.writePanelConfig.writePanelWordRectWidth, _manager.writePanelConfig.writePanelWordRectHeight, TextureFormat.ARGB32, false);
            TextureResource.Instance.Add(TextureResource.Write_Pad_Texture, writePanelWordPanel);

            Texture2D writePanelTotalPanel = new Texture2D(_manager.writePanelConfig.writePanelTotalRectWidth, _manager.writePanelConfig.writePanelTotalRectHeight, TextureFormat.ARGB32, false);
            TextureResource.Instance.Add(TextureResource.Write_Pad_Texture_Big, writePanelTotalPanel);

            _startSceneStatus = StartSceneStatus.LoadResourceCompleted;

            sw.Stop();
            Debug.Log("2 Time : " + sw.ElapsedMilliseconds / 1000f);

        }

        private void LoadConfig()
        {
            // 设置配置表

            //MWConfig _config = _daoService.GetConfig();
            //_manager.globalData.SetMWConfig(_config);


        }


        public MagicSceneEnum GetSceneStatus()
        {
            return _magicSceneEnumStatus;
        }

        DataTypeEnum IScene.GetDataType()
        {
            return DataTypeEnum.Start;
        }

        public void RunEnd(Action onEndCompleted)
        {
            _manager.mainPanel.GetComponent<CanvasGroup>().DOFade(0, 1.5f)
                .OnComplete(() => {
                    _manager.Clear();
                    onEndCompleted.Invoke();
                });
        }

        public SceneConfig GetSceneConfig()
        {
            return new SceneConfig();
        }
    }
}