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

        private float _StartTime = 0f;

        MagicSceneEnum _magicSceneEnumStatus;

        private StartSceneStatus _startSceneStatus;

        private static bool LOG = true;


        float RunTime
        {
            get
            {
                return Time.time - _StartTime;
            }
        }


        private IDaoService _daoService;
        private MagicWallManager _manager;

        Action _onRunCompleted;
        Action _onRunEndCompleted;


        enum StartSceneStatus {
            Init,
            showUI,
            showUICompleted,
            BeginLoadResource,
            LoadResourceCompleted,
            StartHideUI,
            HideUICompleted
        }



        public void Init(SceneConfig sceneConfig, MagicWallManager manager)
        {
            _manager = manager;
            _daoService = manager.daoService;

            _startSceneStatus = StartSceneStatus.Init;

            Reset();
        }


        private void Reset()
        {
            _StartTime = Time.time;

            _magicSceneEnumStatus = MagicSceneEnum.Running;

        }



        public bool Run()
        {
            //DoDebug("Start Scene Is Start!");
            if (_startSceneStatus == StartSceneStatus.Init) {
                _startSceneStatus = StartSceneStatus.showUI;
                _manager.BgLogo.gameObject.SetActive(true);
                _manager.BgLogo.GetComponent<Image>()
                    .DOFade(1, 1f)
                    .OnComplete(() =>
                    {
                        _startSceneStatus = StartSceneStatus.showUICompleted;
                    });
            }

            if (_startSceneStatus == StartSceneStatus.showUICompleted) {
                _startSceneStatus = StartSceneStatus.BeginLoadResource;

                // 读取配置表
                LoadConfig();

                // 加载资源
                LoadResource();
            }

            if (_startSceneStatus == StartSceneStatus.LoadResourceCompleted) {
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

            if (_startSceneStatus == StartSceneStatus.HideUICompleted)
            {
                _onRunCompleted.Invoke();
                _startSceneStatus = StartSceneStatus.Init;
            }


            return true;
        }

        private void Awake()
        {
            Debug.Log("Load Start Scene now !");

            // 加载 Config.xml
            Debug.Log("加载 Config.xml 成功");

            // 根据 tid 获取信息
            Debug.Log("根据 tid 获取信息列表成功 ");

            // 加载关联资源
            Debug.Log("加载关联的资源");

            // 完成数据加载, 提供字典
            Debug.Log("完成数据加载,提供字典");
        }


        private void LoadResource()
        {
            _manager.daoService.InitData();

            var addresses = _manager.daoService.GetMatImageAddresses();

            foreach (string address in addresses)
            {
                string imageAddress = MagicWallManager.FileDir + address;
                //TextureResource.Instance.GetTexture(imageAddress);
                SpriteResource.Instance.GetData(imageAddress);
            }

            // 加载其他资源
            //  - 手写板用的texture
            Texture2D writePanelWordPanel = new Texture2D(_manager.writePanelConfig.writePanelWordRectWidth, _manager.writePanelConfig.writePanelWordRectHeight, TextureFormat.ARGB32, false);
            TextureResource.Instance.Add(TextureResource.Write_Pad_Texture, writePanelWordPanel);

            Texture2D writePanelTotalPanel = new Texture2D(_manager.writePanelConfig.writePanelTotalRectWidth, _manager.writePanelConfig.writePanelTotalRectHeight, TextureFormat.ARGB32, false);
            TextureResource.Instance.Add(TextureResource.Write_Pad_Texture_Big, writePanelTotalPanel);

            _startSceneStatus = StartSceneStatus.LoadResourceCompleted;

        }

        private void LoadConfig()
        {
            // 设置配置表

            MWConfig _config = _daoService.GetConfig();
            _manager.globalData.SetMWConfig(_config);


        }


        public void SetOnRunCompleted(Action onRunCompleted)
        {
            _onRunCompleted = onRunCompleted;
        }

        public MagicSceneEnum GetSceneStatus()
        {
            return _magicSceneEnumStatus;
        }

        DataTypeEnum IScene.GetDataType()
        {
            return DataTypeEnum.Start;
        }

    }
}