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
    private int _type; // 类型，0：产品；1：活动


    [SerializeField, Header("卡片 - 标题")] Text _title;
    [SerializeField, Header("卡片 - 描述")] Text _description;
    [SerializeField] SliceCardScrollViewController _scrollController;


    void Awake() {
        AwakeAgency();
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
    public void InitData(int id,int type) {

        _type = type;

        List<SliceCardCellData> cellDatas;
        if (type == 0)
        {
            Product product = DaoService.Instance.GetProductDetail(id);

            // 获取产品标题
            _title.text = product.Name;

            // 获取产品所属公司信息
            InitComponents(product.Ent_id != 0);

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
            Activity activity = DaoService.Instance.GetActivityDetail(id);

            // 获取产品所属公司信息
            InitComponents(activity.Ent_id != 0);

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

    }


    public void SwitchScaleMode(Texture texture)
    {
        //scaleController.SetImage(texture);
        //scaleController.OpenScaleBox();
    }


    private void OnScrollControllerSelectionChanged(int index) {
        SliceCardBaseCell<SliceCardCellData,SliceCardCellContext> cell = _scrollController.GetCell(index);
        cell.GetComponent<RectTransform>().SetAsLastSibling();
        UpdateDescription(cell.CellData.Description);
    }

    private void UpdateDescription(string description) {
        _description.text = description;
    }

    private void OnOperationAction() {
        DoUpdate();
    }


}


