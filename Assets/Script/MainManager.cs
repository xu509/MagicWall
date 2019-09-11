using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 只在8屏时用
/// 用于判断是否为定制屏幕
/// </summary>
public class MainManager : MonoBehaviour
{
    [SerializeField] SceneType _sceneType;

    [SerializeField] bool isMock;
    [SerializeField] MockDaoService _mockDaoService;
    [SerializeField] DaoService _daoService;


    // Start is called before the first frame update
    void Start()
    {
        // 加载配置表
        IDaoService daoService;

        if (isMock)
        {
            daoService = _mockDaoService;
        }
        else {
            daoService = _daoService;
        }

        bool isCustom = daoService.IsCustom();

        // 加载场景
        LoadScene(_sceneType, isCustom);
    }

    private void LoadScene(SceneType sceneType,bool isCustom) {
        if (sceneType == SceneType.Eight) {
            if (isCustom) {
                SceneManager.LoadScene("CustomScene");

            } else {
                SceneManager.LoadScene("SampleScene");
            }
        }

        if (sceneType == SceneType.Five) {
            if (isCustom)
            {
                SceneManager.LoadScene("CustomSceneFive");
            }
            else
            {
                SceneManager.LoadScene("MagicWallFive");
            }
        }
    }



}

public enum SceneType {
    Eight,Five
}
