using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
//  分类选项卡
//
public class SubScrollCell : SubScrollBaseCell<CrossCardCellData, CrossCardScrollViewContext>
{
    CrossCardCellData _cellData;

    string _title;  // 标题
    int _index; //  索引
    public int Index { set { _index = value; } get { return _index; } }

    [SerializeField] Animator _animator;
    [SerializeField] float _position;
    [SerializeField] RectTransform scale_tool; // 缩小icon

    [SerializeField] Button btn_like;
    [SerializeField] Button btn_like_withnumber;

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
        _cellData = cellData;

        _index = cellData.Index;
        _title = cellData.Title;

        gameObject.name = "CrossSubScroll" + cellData.Index;

        // TODO 更新横向的选项卡
        //IList<CrossCardCellData> datas = CardItemFactoryInstance.Instance.Generate(cellData.Id, cellData.Category);
        //crossCardScrollViewCellItem.UpdateData(datas);

        //  设置 Image
        _image.texture = cellData.ImageTexture;

        // 隐藏组件
        scale_tool.gameObject.SetActive(false);

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

    }

    public void DoScale() {

        Debug.Log("Do Scale. ");
    }

    public override void UpdateComponentStatus()
    {
        // 调整 Scale 按钮

        if (!_cellData.IsImage)
        {
            scale_tool.gameObject.SetActive(false);
        }
        else
        {
            scale_tool.gameObject.SetActive(true);
        }

        // 调整 Like 按钮

        // 将 Card 放在最前端
        GetComponent<RectTransform>().SetAsLastSibling();


    }

    public override void ClearComponentStatus()
    {
        // 清除 缩放 按钮
        if (scale_tool.gameObject.activeSelf) {
            scale_tool.gameObject.SetActive(false);
        }


        // 清除喜欢按钮
        if (btn_like.gameObject.activeSelf) {
            btn_like.gameObject.SetActive(false);
        }
        if (btn_like_withnumber.gameObject.activeSelf)
        {
            btn_like_withnumber.gameObject.SetActive(false);
        }


    }
}
