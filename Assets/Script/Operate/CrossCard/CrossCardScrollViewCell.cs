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

    bool _hasClickedLiked = false;   // 标识符，是否已点击喜欢


    //
    //  Component Paramater 
    //
    [SerializeField] Button btn_like;
    [SerializeField] Button btn_like_withnumber;

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
        //_crossCardAgent = crossCardAgent;

        _cellData = cellData;

        _index = cellData.Index;
        _title = cellData.Title;

        gameObject.name = "CrossCardScrollCell" + cellData.Index;

        // TODO 更新横向的选项卡
        IList<CrossCardCellData> datas = CardItemFactoryInstance.Instance.Generate(cellData.EnvId, cellData.Category);
        subScrollController.UpdateData(datas);

        subScrollController.crossCardScrollViewCell = this;

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

    public override void UpdateBtnLikeStatus()
    {
        // Reset
        _hasLikeNumber = false;

        bool hasLikesNumber = false;
        int likes = _cellData.Likes;
        string likesStr;

        if (likes > 0)
        {
            //_btnLikeAnimator.Play(AnimatorHash.CardButtonLikeNormalWithNumber);
            hasLikesNumber = true;
            _hasLikeNumber = true;
        }
        else {
            _hasLikeNumber = false;
        }

        if (likes > 99)
        {
            likesStr = "99+";
        }
        else
        {
            likesStr = _cellData.Likes.ToString();
        }

        
        if (hasLikesNumber)
        {
            // 已存在喜欢数时，开启相关的按钮，并赋值
            btn_like.gameObject.SetActive(false);
            btn_like_withnumber.gameObject.SetActive(true);
            btn_like_withnumber.GetComponentInChildren<Text>().text = likesStr;
        }
        else {
            btn_like_withnumber.gameObject.SetActive(false);
            btn_like.gameObject.SetActive(true);
        
        }
    }


    public void DoLike() {
        if (_hasClickedLiked) {
            return;
        }

        _hasClickedLiked = true;

        int likes = _cellData.Likes;

        //  显示数增加
        if (!_hasLikeNumber)
        {
            btn_like.GetComponentInChildren<Text>().text = (likes + 1).ToString();
            // 需从不显示改为显示
            btn_like.GetComponentInChildren<Text>().DOFade(1, Time.deltaTime);
        }
        else
        {
            int newLikes = likes + 1;
            string newLikeStr = newLikes > 99 ? "99+" : newLikes.ToString();

            btn_like_withnumber.GetComponentInChildren<Text>().DOText(newLikeStr, Time.deltaTime);
        }


        // TODO 数据逻辑上进行添加数值
        _cellData.Likes = _cellData.Likes + 1;

    }

}
