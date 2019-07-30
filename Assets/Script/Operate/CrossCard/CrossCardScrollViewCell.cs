using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//
//  分类选项卡
//
public class CrossCardScrollViewCell : CrossCardBaseCell<CrossCardCellData, CrossCardScrollViewContext>
{

    bool _hasLikeNumber = false;

    CrossCardAgent _crossCardAgent; // Cross Card Agent

    string _title;  // 标题
    [SerializeField] int _index; //  索引
    [SerializeField] CrossCardCellData _cellData; //  cellData
    public CrossCardCellData cellData { set { _cellData = value; } get { return _cellData; } }


    [SerializeField] Animator _animator;
    [SerializeField] float _position;


    //
    //  Component Paramater 
    //
    [SerializeField] SubScrollController subScrollController;


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
        // TODO 非常耗时， 每个都耗时



        System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
        sw2.Start();

        //_crossCardAgent = crossCardAgent;

        _cellData = cellData;

        _index = cellData.Index;
        _title = cellData.Title;

        gameObject.name = "CrossCardScrollCell" + cellData.Index;


        IList<CrossCardCellData> datas = cellData.magicWallManager.cardItemFactoryInstance
            .Generate(cellData.EnvId, cellData.Category,cellData.crossCardAgent);




        System.Diagnostics.Stopwatch sw3 = new System.Diagnostics.Stopwatch();
        sw3.Start();
        subScrollController.UpdateData(datas);
        sw3.Stop();
        // Debug.Log("- [" + _title + "] cell 分耗时: " + sw3.ElapsedMilliseconds / 1000f);

        subScrollController.crossCardScrollViewCell = this;

        // 传递context
        subScrollController.Context.OnScaleClicked = Context.OnScaleClicked;
        subScrollController.Context.OnPlayVideo = Context.OnPlayVideo;
        subScrollController.Context.OnDescriptionChanged = Context.OnDescriptionChanged;

        subScrollController.OnSelectionChanged(OnSelectionChanged);

        // 隐藏组件
        ClearComponentStatus();

        sw2.Stop();
        // Debug.Log("[" + _title + "] cell 总耗时: " + sw2.ElapsedMilliseconds / 1000f);
    }

    /// <summary>
    /// 此段代码始终在运行
    /// </summary>
    /// <param name="position"></param>
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

    public override void UpdateComponentStatus()
    {
        GetComponent<RectTransform>().SetAsLastSibling();
        subScrollController.UpdateAllComponents();
    }

    // 次级滚动栏变化时内容
    public void OnSelectionChanged(int index) {
        // 更新 card 状态
        _cellData.crossCardAgent.DoUpdate();

        SubScrollBaseCell<CrossCardCellData, CrossCardScrollViewContext> cell = subScrollController.GetCell(index);
        CrossCardCellData data = cell.GetData();

        if (_index == Context.SelectedIndex) {
            cell.DoUpdateDescription();
        }

        //DoUpdateDescription(data.Description);
        
    }


    public override void ClearComponentStatus()
    {
        subScrollController.ClearAllComponents();

    }


    // 获取当前的描述
    public override string GetCurrentDescription() {

        string str = subScrollController.GetCurrentDescription();
        return str ;
    }

}
