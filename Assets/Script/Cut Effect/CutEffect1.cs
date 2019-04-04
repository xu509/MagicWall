using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 1 
public class CutEffect1 : CutEffect
{
    public float dur_time = 3f;

    public override void run(FlockAgent prefab,MagicWallManager magicWallManager) {
        int row = magicWallManager.row;
        int column = magicWallManager.column;

        int itemWidth = 300;
        int itemHeight = 300;
        int gap = 0;

        //从左往右，从下往上
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                //FlockAgent go = Instantiate(prefab,magicWallManager.mainPanel);

                //RectTransform rectTransform = go.transform.GetComponent<RectTransform>();
                float x = j * (itemWidth + gap) + itemWidth / 2;
                float y = i * (itemHeight + gap) + itemHeight / 2;
                
                //FlockAgent go = magicWallManager.CreateNewAgent(x,y,i,j);
                
                //the_RectTransform.localPosition = new Vector3(x, y, 0);
                
                int middleY = row / 2;
                int middleX = column / 2;
                
                float delayX = j * 0.06f;
                float delayY;

                // ori_x;ori_y
                float ori_x,ori_y;


                if (i < middleY)
                {
                    delayY = System.Math.Abs(middleY - i) * 0.3f;
                    ori_x = (column + middleY - i - 1) * (itemWidth + gap) + itemWidth / 2;
                    ori_y = (column - j - middleY) * (itemHeight + gap) + itemHeight / 2;


                    //the_RectTransform.DOLocalMove(new Vector3((column + middleY - i - 1) * (itemWidth + gap) + itemWidth / 2, (column - j - middleY) * (itemHeight + gap) + itemHeight / 2, 0), dur_time - delayX + delayY).SetEase(Ease.InOutQuad).From();
                }
                else
                {
                    delayY = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    ori_x = (column + i - middleY) * (itemWidth + gap) + itemWidth / 2;
                    ori_y = -(column - j - middleY) * (itemHeight + gap) + itemHeight / 2;


                    //the_RectTransform.DOLocalMove(new Vector3((column + i - middleY) * (itemWidth + gap) + itemWidth / 2, -(column - j - middleY) * (itemHeight + gap) + itemHeight / 2, 0), dur_time - delayX + delayY).SetEase(Ease.InOutQuad).From();
                }

                FlockAgent go = magicWallManager.CreateNewAgent(ori_x, ori_y,x,y,i,j);
                RectTransform the_RectTransform = go.GetComponent<RectTransform>();

                the_RectTransform.DOAnchorPos(new Vector2(x, y), dur_time - delayX + delayY).SetEase(Ease.InOutQuad);

                the_RectTransform.DOScale(0.1f, (dur_time - delayX + delayY)).From();
                //go.GetComponent<RawImage>().DOFade(0, (dur_time - delayX + delayY)).From();
            }
        }

    }

}
