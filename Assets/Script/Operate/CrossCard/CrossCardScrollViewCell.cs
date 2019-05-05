using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
//  分类选项卡
//
public class CrossCardScrollViewCell : FancyScrollViewCell<CrossCardCellData, CrossCardScrollViewContext>
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
        _index = cellData.Index;
        _title = cellData.Title;

        text_index.text = cellData.Index.ToString();
        text_position.text = cellData.Title.ToString();
        gameObject.name = "CrossCardScrollCell" + cellData.Index;

        // TODO 更新横向的选项卡
        IList<CrossCardCellData> datas = CardItemFactoryInstance.Instance.Generate(cellData.Id, cellData.Category);
        //crossCardScrollViewCellItem.UpdateData(datas);


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


    public void DoTest() {
    }

}
