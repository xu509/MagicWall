using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrossCardScrollViewCellItemCell : FancyScrollViewCell<CrossCardCellData, CrossCardScrollViewContext>
{
    string _title;  // 标题
    [SerializeField] int _index; //  索引
    [SerializeField] Animator _animator;
    [SerializeField] float _position;

    //
    //  Component Paramater 
    //
    [SerializeField] Text text_index;
    [SerializeField] Text text_position;



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
        gameObject.name = "ItemCell" + cellData.Id;
        
    }

    public override void UpdatePosition(float position)
    {
        currentPosition = position;
        _animator.Play(AnimatorHash.Scroll, -1, position);
        _animator.speed = 0;

        _position = position;

    }

    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);


}
