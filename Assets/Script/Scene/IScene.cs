using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScene
{
	//
	//	启动,返回值表明场景是否正在运行,当为false时表示该场景已运行完毕
	//
	bool Run();

    void Init(SceneConfig sceneConfig, MagicWallManager manager);

    //
    //  获取内容类型
    //
    DataType GetDataType();

}

