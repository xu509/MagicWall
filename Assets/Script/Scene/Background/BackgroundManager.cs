using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BackgroundManager : MonoBehaviour
{

    // 气泡预制体
    [SerializeField] GameObject _backgroundPrefab;
    // 气泡预制体2 
    [SerializeField] GameObject _backgroundPrefab2;

    /// 背景配置项
    //气泡上升时间
    [SerializeField, Range(10f, 100f)] float _backgroundUpDuration = 60f;
    //生成气泡时间间隔
    [SerializeField, Range(0.1f, 10f)] float _backgroundUubbleInterval = 0.2f;

    MagicWallManager _manager;
      

    private Transform _bubblesBackground;
    private float last_create_time = 0f;

    private List<GameObject> bubbles;
    private bool hasInit = false;

    //
    // Awake instead of Constructor
    //
    private void Awake()
    {
        
    }

    public void Init(MagicWallManager manager) {

        _manager = manager;

        last_create_time = 0.0f;
        bubbles = new List<GameObject>();
        hasInit = true;

        //  初始化对象池

    }



    //
    //  Constructor
    //
    protected BackgroundManager(){}


    public void run() {

        if (!hasInit)
            return;

        if ((Time.time - last_create_time) > _backgroundUubbleInterval) {
            CreateBubble();
            last_create_time = Time.time;
        }
        //InvokeRepeating("CreateBubble", 0, bubbleInterval);

    }

    //创建气泡
    void CreateBubble()
    {
        GameObject bubble;
        float random = Random.Range(0, 2);
        int w = 0;
        float alpha = 1;
        float duration = _backgroundUpDuration;
        if (random == 0)
        {
            //实球
            bubble = Instantiate(_backgroundPrefab) as GameObject;
            w = Random.Range(400, 80);
            alpha = w / (400f+100f);
            //print(w);
            duration -= alpha * 5;
        }
        else
        {
            //虚球
            bubble = Instantiate(_backgroundPrefab2) as GameObject;
            int r = Random.Range(0, 2);
            w = (r == 0) ? 200 : 80;
            duration += 10f;
            alpha = r == 0 ? 0.8f : 0.4f;
        }
        RectTransform rectTransform = bubble.GetComponent<RectTransform>();
        rectTransform.SetParent(_manager.BackgroundPanel, false);
        rectTransform.sizeDelta = new Vector2(w, w);

        float x = Random.Range(-100, (int)_manager.mainPanel.rect.width);

        bubble.GetComponent<RawImage>().DOFade(alpha, 0);
        rectTransform.anchoredPosition3D = new Vector3(x, -w, 0);
        rectTransform.DOLocalMoveY(2000, duration).OnComplete(() => BubbleMoveComplete(bubble));

        bubbles.Add(bubble);
    }

    //气泡上升结束后销毁
    void BubbleMoveComplete(GameObject game)
    {
        Destroy(game);
        bubbles.Remove(game);
    }


    public void Reset() {
        hasInit = false;
        for (int i = 0; i < bubbles.Count; i++) {
            Destroy(bubbles[i]);
        }
        Init(_manager);
    }


}
