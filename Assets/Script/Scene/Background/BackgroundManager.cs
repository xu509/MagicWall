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

    private List<GameObject> bubbles;
    private bool hasInit = false;

    //
    // Awake instead of Constructor
    //
    private void Awake()
    {
        _bubblesBackground = GameObject.Find("MagicWall/Background").GetComponent<Transform>();
        _manager = MagicWallManager.Instance;
        Init();
    }

    private void Init() {
        last_create_time = 0.0f;
        bubbles = new List<GameObject>();
        hasInit = true;
    }



    //
    //  Constructor
    //
    protected BackgroundManager(){}


    public void run() {
        if (!hasInit)
            return;

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
        Init();
    }


}
