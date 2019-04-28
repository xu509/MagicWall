using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossCardScrollCell : FancyScrollViewCell<CrossCardCellData>
{
    string _title;  // 标题
    int _index; //  索引
    public int Index { set { _index = value; } get { return _index; } }


    [SerializeField] Animator _animator;
    [SerializeField] float _position;

    static readonly int ScrollTriggerHash = Animator.StringToHash("scroll");



    // Start is called before the first frame update
    void Start()
    {
        _animator.Play(ScrollTriggerHash, -1, _position);

    }

    // Update is called once per frame
    void Update()
    {

        //_animator.Play(ScrollTriggerHash, -1, _position);
        //_animator.speed = 0;
    }

    //public void Initialize(CrossCardIndexItem item) {
    //    _index = item.Index;
    //    _title = item.Title;


    //}


    public override void UpdateContent(CrossCardCellData cellData)
    {
        _index = cellData.Index;
        _title = cellData.Title;
    }

    public override void UpdatePosition(float position)
    {
        Debug.Log("POSITION: " + position);

        currentPosition = position;
        _animator.Play(ScrollTriggerHash, -1, position);
        _animator.speed = 0;
    }

    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);


}
