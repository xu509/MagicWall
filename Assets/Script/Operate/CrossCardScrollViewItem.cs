using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrossCardScrollViewItem : FancyScrollViewCell<CrossCardScrollViewCellData>
{
    [SerializeField] Animator animator;

    static readonly int ScrollTriggerHash = Animator.StringToHash("scroll");

    /// <summary>
    /// Updates the content.
    /// </summary>
    /// <param name="cellData">Cell data.</param>
    public override void UpdateContent(CrossCardScrollViewCellData cellData)
    {

    }

    /// <summary>
    /// Updates the position.
    /// </summary>
    /// <param name="position">Position.</param>
    public override void UpdatePosition(float position)
    {
        currentPosition = position;
        animator.Play(ScrollTriggerHash, -1, position);
        animator.speed = 0;
    }

    // GameObject が非アクティブになると Animator がリセットされてしまうため
    // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);
}

