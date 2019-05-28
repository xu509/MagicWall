using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        string content = text.text;
        text.text = " | " + content + " | ";

        float v2 = GetComponent<RectTransform>().rect.xMin;
        Debug.Log("V2: " + v2);


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
