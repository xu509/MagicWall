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




    public void UpdateAsCurrent() {

        int length = _cellData.Title.Length;
        if (length == 2)
        {
            _signRect.DOAnchorPosX(30, 0.5f);
        }
        else if (length == 4)
        {
            _signRect.DOAnchorPosX(15, 0.5f);
        }
        else if (length == 7)
        {
            _signRect.DOAnchorPosX(-5, 0.5f);

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
