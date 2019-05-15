using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
//  分类选项卡
//
public class SubScrollCell : SubScrollBaseCell<CrossCardCellData, CrossCardScrollViewContext>
{
    string _title;  // 标题
    [SerializeField] int _index; //  索引
    [SerializeField] Animator _animator;
    [SerializeField] float _position;

    [SerializeField] RectTransform scale_tool; // 缩小icon

    //
    //  Component Paramater 
    //
    [SerializeField] RawImage _image;


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

        gameObject.name = "CrossCardScrollCell" + cellData.Index;

        // TODO 更新横向的选项卡
        //IList<CrossCardCellData> datas = CardItemFactoryInstance.Instance.Generate(cellData.Id, cellData.Category);
        //crossCardScrollViewCellItem.UpdateData(datas);

        //  设置 Image
        _image.texture = cellData.ImageTexture;



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

    public override void InitData()
    {
        Debug.Log("Init Data");

    }
}
