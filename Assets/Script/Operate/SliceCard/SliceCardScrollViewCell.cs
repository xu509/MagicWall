using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//
//  分类选项卡
//
public class SliceCardScrollViewCell : SliceCardBaseCell<SliceCardCellData, SliceCardCellContext>
{
    bool _hasClickedLiked = false;   // 标识符，是否已点击喜欢

    bool _hasLikeNumber = false;

    SliceCardAgent _sliceCardAgent; // Card Agent

    string _title;  // 标题
    [SerializeField] int _index; //  索引
    [SerializeField] SliceCardCellData _cellData; //  cellData
    int _likes;

    public SliceCardCellData cellData { set { _cellData = value; } get { return _cellData; } }

    [SerializeField] RawImage _cover;
    [SerializeField] Animator _animator;
    [SerializeField] float _position;
    [SerializeField] RectTransform scale_tool; // 缩小icon
    [SerializeField] Button btn_like;
    [SerializeField] Button btn_like_withnumber;


    [SerializeField] RectTransform videoContainer;
    [SerializeField] RawImage _video_cover;
    [SerializeField] ButtonLikeAgent _buttonLikeAgent;


    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }


    // Start is called before the first frame update
    void Start()
    {
        
        //button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));

    }

    public override void UpdateContent(SliceCardCellData cellData)
    {
        SetupData(cellData);

        _cellData = cellData;
        _index = cellData.Index;

        gameObject.name = "SliceCardScrollCell" + cellData.Index;
        // 需要判断是否为视频还是图片

        if (cellData.IsImage)
        {
            _cover.gameObject.SetActive(true);

            // 配置图片
            string address;
            if (cellData.IsProduct())
            {
                address = MagicWallManager.FileDir + cellData.Image;
            }
            else
            {
                address = MagicWallManager.FileDir  + cellData.Image;
            }

            _cover.texture = TextureResource.Instance.GetTexture(address);
            CanvasExtensions.SizeToParent(_cover);
            videoContainer.gameObject.SetActive(false);


            // 调整 Like 按钮
            if (cellData.IsProduct())
            {
                _likes = DaoService.Instance.GetLikesByProductDetail(_cellData.Id);
            }
            else
            {
                _likes = DaoService.Instance.GetLikesByActivityDetail(_cellData.Id);
            }


        }
        else {
            videoContainer.gameObject.SetActive(true);
            string address = MagicWallManager.FileDir + cellData.Image;

            _video_cover.texture = TextureResource.Instance.GetTexture(address);
            CanvasExtensions.SizeToParent(_video_cover);

            _cover.gameObject.SetActive(false);
        }
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

    // 当卡片被选中时，会走这个方法
    public override void UpdateComponentStatus()
    {
        //Debug.Log("Index : " + Index + " | Current Index : " + Context.SelectedIndex);


        if (!_cellData.IsImage)
        {
            scale_tool.gameObject.SetActive(false);
        }
        else
        {
            scale_tool.gameObject.SetActive(true);
        }

        // 调整 Like 按钮
        if (cellData.IsProduct())
        {
            _likes = DaoService.Instance.GetLikesByProductDetail(_cellData.Id);
        }
        else
        {
            _likes = DaoService.Instance.GetLikesByActivityDetail(_cellData.Id);
        }

        _likes = 0;


        _buttonLikeAgent.Init(_likes, OnClickLikeBtn);


        GetComponent<RectTransform>().SetAsLastSibling();

        //// 调用改变描述
        //Context.OnDescriptionChanged.Invoke(cellData.Description);
    }



    public override void ClearComponentStatus()
    {
        // 清除 缩放 按钮
        if (scale_tool.gameObject.activeSelf)
        {
            scale_tool.gameObject.SetActive(false);
        }

        if (_buttonLikeAgent.gameObject.activeSelf)
        {
            _buttonLikeAgent.gameObject.SetActive(false);
        }
    }


    // 获取当前的描述
    public override string GetCurrentDescription() {
        return cellData.Description;
    }

    public void DoScale()
    {
        // 获取图片
        Context.OnScaleClicked?.Invoke(_cover.texture);
    }

    public void DoPlayVideo()
    {
        //string url = _cellData.VideoUrl;
        Context.OnPlayVideo?.Invoke(_cellData);
    }



    public void DoLike()
    {
        if (_hasClickedLiked)
        {
            return;
        }

        _hasClickedLiked = true;

        //  显示数增加
        if (!_hasLikeNumber)
        {
            btn_like.GetComponentInChildren<Text>().text = (_likes + 1).ToString();
            // 需从不显示改为显示
            btn_like.GetComponentInChildren<Text>().DOFade(1, Time.deltaTime);
        }
        else
        {
            int newLikes = _likes + 1;
            string newLikeStr = newLikes > 99 ? "99+" : newLikes.ToString();
            btn_like_withnumber.GetComponentInChildren<Text>().DOText(newLikeStr, Time.deltaTime);
        }

        // TODO 数据逻辑上进行添加数值
        _likes = _likes + 1;
        _cellData.sliceCardAgent.DoUpdate();
    }

    private void OnClickLikeBtn() {

    }

}
