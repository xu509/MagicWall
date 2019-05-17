using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundManager : Singleton<BackgroundManager>
{

    public int test = 0;

    private MagicWallManager _manager;
    private Transform _bubblesBackground;
    private float last_create_time = 0f;

    //
    // Awake instead of Constructor
    //
    private void Awake()
    {
        _bubblesBackground = GameObject.Find("MagicWall/Background").GetComponent<Transform>();
        last_create_time = 0.0f;

        _manager = MagicWallManager.Instance;
    }

    //
    //  Constructor
    //
    protected BackgroundManager(){}


    public void run() {
        if ((Time.time - last_create_time) > _manager.backgroundUubbleInterval) {
            CreateBubble();
            last_create_time = Time.time;
        }
        //InvokeRepeating("CreateBubble", 0, bubbleInterval);

    }

    //创建气泡
    void CreateBubble()
    {
        GameObject bubble;
        float random = Random.Range(0.0f, 1.0f);
        if (random < 0.5)
        {
            bubble = Instantiate(_manager.backgroundPrefab) as GameObject; 
        }
        else
        {
            bubble = Instantiate(_manager.backgroundPrefab2) as GameObject;
        }
        RectTransform rectTransform = bubble.GetComponent<RectTransform>();
        rectTransform.SetParent(_bubblesBackground, false);
        float x = Random.Range(-1500, 9000);
        float z = Random.Range(500, 10000);
        float alpha = 1 - (z - 500) / 9500f;
        //print(alpha);
        bubble.GetComponent<RawImage>().DOFade(alpha, 0);
        rectTransform.anchoredPosition3D = new Vector3(x, -2500, z);
        rectTransform.DOLocalMoveY(2000, _manager.backgroundUpDuration).OnComplete(() => BubbleMoveComplete(bubble));

    }

    //气泡上升结束后销毁
    void BubbleMoveComplete(GameObject game)
    {
        Destroy(game);
    }





}
