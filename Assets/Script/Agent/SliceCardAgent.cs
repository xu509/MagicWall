using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

//
//  滑动卡片代理，用于product和activity
//
public class SliceCardAgent : CardAgent
{

    [SerializeField] Text _title;
    [SerializeField] Text _description;
    [SerializeField] SliceCardScrollViewController _scrollController;
    [SerializeField] RectTransform _buttomTool;

    void Awake() {
    }

    //
    //  更新
    //
    void Update() {
        UpdateAgency();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">产品ID或活动ID</param>
    /// <param name="type">类型</param>
    public void InitSliceCard() {

        InitAgency();

        List<SliceCardCellData> cellDatas;
        if (type == MWTypeEnum.Product)
        {
            Product product = DaoService.Instance.GetProductDetail(DataId);

            // 获取产品标题
            _title.text = product.Name;

            // 获取产品所属公司信息
            //InitComponents(product.Ent_id != 0);

            InitComponents(Random.Range(0, 5) > 2);




            // 获取产品详细（图片，描述）
            cellDatas = new List<SliceCardCellData>();
            for (int i = 0; i < product.ProductDetails.Count; i++)
            {
                SliceCardCellData cellData = new SliceCardCellData();
                cellData.Type = 0;
                cellData.sliceCardAgent = this;
                cellData.LoadProductDetail(product.ProductDetails[i]);
                cellDatas.Add(cellData);
            }
        }
        else {
            // 初始化活动信息
            Activity activity = DaoService.Instance.GetActivityDetail(DataId);

            // 获取产品所属公司信息
            InitComponents(Random.Range(0, 5) > 2);
            InitComponents(true);


            // 获取产品详细（图片，描述）
            cellDatas = new List<SliceCardCellData>();
            for (int i = 0; i < activity.ActivityDetails.Count; i++)
            {
                SliceCardCellData cellData = new SliceCardCellData();
                cellData.Type = 0;
                cellData.sliceCardAgent = this;
                cellData.LoadActivityDetail(activity.ActivityDetails[i]);
                cellDatas.Add(cellData);
            }
        }

        
        _scrollController.SetUpCardAgent(this);
        _scrollController.UpdateData(cellDatas);
        _scrollController.OnSelectionChanged(OnScrollControllerSelectionChanged);
        _scrollController.SetOnScrollerOperated(OnOperationAction);


        SetOnCreatedCompleted(OnCreatedCompleted);

    }


    public void SwitchScaleMode(Texture texture)
    {
        //scaleController.SetImage(texture);
        //scaleController.OpenScaleBox();
    }


    private void OnScrollControllerSelectionChanged(int index) {
        SliceCardBaseCell<SliceCardCellData,SliceCardCellContext> cell = _scrollController.GetCell(index);
        cell.GetComponent<RectTransform>().SetAsLastSibling();

        string description = _scrollController.GetCurrentCardDescription();

        //  更新描述
        UpdateDescription(description);

        // 更新下方操作栏
        UpdateToolComponent();
    }

    public void UpdateDescription(string description) {
        // 从透明到不透明，向下移动
        _description.text = description;

        //_description.GetComponent<RectTransform>().anchoredPosition = Description_Origin_Position;
        //_description.DOFade(0, 0f);

        //_description.GetComponent<RectTransform>().DOAnchorPos(Description_Go_Position, 0.5f);
        //_description.DOFade(1, 0.5f);

    }

    private void UpdateToolComponent() {


        //_buttomTool.GetComponent<RectTransform>().anchoredPosition = ButtomTool_Origin_Position;
        //_buttomTool.GetComponent<CanvasGroup>().DOFade(0, Time.deltaTime);

        //_buttomTool.GetComponent<RectTransform>().DOAnchorPos(ButtomTool_Go_Position, 0.5f);
        //_buttomTool.GetComponent<CanvasGroup>().DOFade(1, 0.5f);


    }


    private void OnOperationAction() {
        DoUpdate();
    }


    private void OnCreatedCompleted() {

        string description = _scrollController.GetCurrentCardDescription();

        //  更新描述
        UpdateDescription(description);

        //  更新操作栏
        UpdateToolComponent();

    }


}


