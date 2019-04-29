using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrossCardScrollBarCell : FancyScrollViewCell<CrossCardCellData, CrossCardScrollViewContext>
{
    string _title;  // 标题
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
        _index = cellData.Index;
        _title = cellData.Title;

        text.text = cellData.Title.ToString();
        gameObject.name = "CrossCardScrollCell" + cellData.Index;

        Debug.Log("Do Update Content : " + cellData.Index + " | " + cellData.Title);


    }

    public override void UpdatePosition(float position)
    {
        currentPosition = position;
        _animator.Play(AnimatorHash.Scroll, -1, position);
        _animator.speed = 0;


    }

    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);


}
