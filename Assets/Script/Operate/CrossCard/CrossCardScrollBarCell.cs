using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CrossCardScrollBarCell : FancyScrollViewCell<CrossCardCellData, CrossCardScrollViewContext>
{
    private CrossCardCellData _cellData;
    string _title;  // 标题

    RectTransform _signRect;

    [SerializeField] int _index; //  索引
    [SerializeField] Animator _animator;

    //
    //  Component Paramater 
    //
    [SerializeField] Text text;

    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }


    // Start is called before the first frame update
    void Start()
    {

        //button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));

    }

    public override void UpdateContent(CrossCardCellData cellData)
    {
        _cellData = cellData;

        _index = cellData.Index;
        _title = cellData.Title;

        text.text = cellData.Title.ToString();
        gameObject.name = "CrossCardScrollCell" + cellData.Index;



    }

    public override void UpdatePosition(float position)
    {
        currentPosition = position;
        _animator.Play(AnimatorHash.Scroll, -1, position);
        _animator.speed = 0;
    }

    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);



    /// <summary>
    /// 更新 scroll bar 的左侧符号
    /// </summary>
    public void UpdateAsCurrent() {
        Debug.Log("UpdateAsCurrent");


        // 每单个icon的宽度 ： anchors min -> max 0.1171
        // else : min - 0.3003   ,  max - 0.7058


        int length = _cellData.Title.Length;
        if (length == 2)
        {
            Vector2 anchor_min = new Vector2(0.307f, 0.3003f);
            Vector2 anchor_max = new Vector2(0.378f, 0.7058f);

            _signRect.anchorMax = anchor_max;
            _signRect.anchorMin = anchor_min;
            _signRect.DOAnchorPos(Vector2.zero, 1f);
        }
        else if (length == 4)
        {
            //  anchors min (-0.15) | anchors max (-0.0329)
            Vector2 anchor_min = new Vector2(0.219f, 0.3003f);
            Vector2 anchor_max = new Vector2(0.29f, 0.7058f);

            _signRect.anchorMax = anchor_max;
            _signRect.anchorMin = anchor_min;
            _signRect.DOAnchorPos(Vector2.zero,1f);
        }
        else if (length == 7)
        {
            Vector2 anchor_min = new Vector2(0.190f, 0.3003f);
            Vector2 anchor_max = new Vector2(0.261f, 0.7058f);

            _signRect.anchorMax = anchor_max;
            _signRect.anchorMin = anchor_min;
            _signRect.DOAnchorPos(Vector2.zero, 1f);
        }

        text.DOText(" | " + _cellData.Title + " | ", Time.deltaTime);

        //TODO 有时候text 改变无效
    }


    public void UpdateComponent(RectTransform rect) {
        _signRect = rect;

        if (_index == Context.SelectedIndex)
        {
            UpdateAsCurrent();
        }
        else {
            text.text = _cellData.Title.ToString();
        }
    }



}
