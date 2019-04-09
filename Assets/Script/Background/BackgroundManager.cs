using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[CreateAssetMenu(menuName = "Flock/Manager/BackgroundManager")]
public class BackgroundManager : ScriptableObject
{

    //public GameObject bubblePrefab;//气泡预制体
    //public float upDuration = 20f;//气泡上升时间
    //public float bubbleInterval = 0.4f;//生成气泡时间间隔

    private MagicWallManager magicWallManager;
    private Transform bubblesBackground;
    private float last_create_time = 0f;


    private void Awake()
    {
       
    }

    public void init(MagicWallManager manager) {
        bubblesBackground = GameObject.Find("MagicWall/Background").GetComponent<Transform>();
        this.last_create_time = 0.0f;
        magicWallManager = manager;
    }


    public void run() {
        if ((Time.time - last_create_time) > magicWallManager.backgroundUubbleInterval) {
            CreateBubble();
            last_create_time = Time.time;
        }
        //InvokeRepeating("CreateBubble", 0, bubbleInterval);

    }

    //创建气泡
    void CreateBubble()
    {
        GameObject bubble = Instantiate(magicWallManager.backgroundPrefab) as GameObject;
        float random = Random.Range(0.0f, 1.0f);
        if (random < 0.5)
        {
            bubble.GetComponent<RawImage>().texture = Resources.Load("Textures/bubble1") as Texture;
        }
        else
        {
            bubble.GetComponent<RawImage>().texture = Resources.Load("Textures/bubble2") as Texture;
        }
        RectTransform rectTransform = bubble.GetComponent<RectTransform>();
        rectTransform.SetParent(bubblesBackground, false);
        float x = Random.Range(-1500, 9000);
        float z = Random.Range(500, 10000);
        float alpha = 1 - (z - 500) / 9500f;
        //print(alpha);
        bubble.GetComponent<RawImage>().DOFade(alpha, 0);
        rectTransform.anchoredPosition3D = new Vector3(x, -2500, z);
        rectTransform.DOLocalMoveY(2000, magicWallManager.backgroundUpDuration).OnComplete(() => BubbleMoveComplete(bubble));

    }

    //气泡上升结束后销毁
    void BubbleMoveComplete(GameObject game)
    {
        Destroy(game);
    }





}
