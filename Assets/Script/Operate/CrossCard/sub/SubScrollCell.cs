using UnityEngine;
using UnityEngine.UI;


//
//  分类选项卡
//
public class SubScrollCell : SubScrollBaseCell<CrossCardCellData, CrossCardScrollViewContext>
{
    bool _hasClickedLiked = false;   // 标识符，是否已点击喜欢

    CrossCardCellData _cellData;
    string _title;  // 标题
    int _index; //  索引

    int _likes;

    public int Index { set { _index = value; } get { return _index; } }

    [SerializeField] Animator _animator;
    [SerializeField] float _position;

    [SerializeField] RectTransform scale_tool; // 缩小icon

    //
    //  Component Paramater 
    //

    [SerializeField] RectTransform videoContainer;
    [SerializeField] Image _cover;
    [SerializeField] Image _video_cover;
    [SerializeField] ButtonLikeAgent _buttonLikeAgent;


    bool _hasLikeNumber = false;

    MagicWallManager _manager;


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

        _manager = cellData.magicWallManager;

        System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
        sw2.Start();


        _cellData = cellData;
        _index = cellData.Index;
        _title = cellData.Title;

        gameObject.name = "CrossSubScroll" + cellData.Index;
            
        if (cellData.Category == CrossCardCategoryEnum.VIDEO)
        {
            videoContainer.gameObject.SetActive(true);
            _cover.gameObject.SetActive(false);
            _video_cover.sprite = SpriteResource.Instance.GetData(MagicWallManager.FileDir + cellData.Image);
            CanvasExtensions.SizeToParent(_video_cover);

        }
        else {
            //  设置 Image
            _cover.gameObject.SetActive(true);
            _cover.sprite = SpriteResource.Instance.GetData(MagicWallManager.FileDir +  cellData.Image);
            CanvasExtensions.SizeToParent(_cover);

            // 关闭视频框
            videoContainer.gameObject.SetActive(false);
        }

        sw2.Stop();
        //Debug.Log("[" + _title  + "] Sub Cell Time : " + sw2.ElapsedMilliseconds / 1000f);


    }

    public void DoPlayVideo() {
        //string url = _cellData.VideoUrl;

        Context.OnPlayVideo?.Invoke(_cellData);
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

        Sprite sprite = _cover.sprite;


        var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();


        Context.OnScaleClicked?.Invoke(croppedTexture);
    }

    public override void DoUpdateDescription()
    {
        // 获取图片
        Context.OnDescriptionChanged?.Invoke(_cellData.Description);

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
        //_manager.daoService.
        //_likes = _manager.daoService.GetLikes(_cellData.Id, _cellData.Category);
        _likes = _manager.daoService.GetLikes(_cellData.Image);

        // 设置喜欢
        _buttonLikeAgent.Init(_likes, OnClickLike);

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
        _buttonLikeAgent.gameObject.SetActive(false);

    }


    public override CrossCardCellData GetData()
    {
        return _cellData;
    }


    private void OnClickLike() {
        Debug.Log("On Click Like");
    }



}
