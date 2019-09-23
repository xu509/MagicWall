using UnityEngine;


public abstract class SubScrollBaseCell<CrossCardCellData, CrossCardScrollViewContext> : MonoBehaviour
{
    private bool _hasInit = false;
    public bool HasInit { set { _hasInit = value; } get { return _hasInit; } }

    /// <summary>
    /// Gets or sets the index of the data.
    /// </summary>
    /// <value>The index of the data.</value>
    public int Index { get; set; } = -1;

    /// <summary>
    /// Gets a value indicating whether this <see cref="T:FancyScrollView.FancyScrollViewCell`2"/> is visible.
    /// </summary>
    /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
    public virtual bool IsVisible => gameObject.activeSelf;


    public abstract CrossCardCellData GetData();

    public abstract void DoUpdateDescription();

    /// <summary>
    /// Gets the context.
    /// </summary>
    /// <value>The context.</value>
    protected CrossCardScrollViewContext Context { get; private set; }

    /// <summary>
    /// Setup the context.
    /// </summary>
    /// <param name="context">Context.</param>
    public virtual void SetupContext(CrossCardScrollViewContext context) => Context = context;

    /// <summary>
    /// Sets the visible.
    /// </summary>
    /// <param name="visible">If set to <c>true</c> visible.</param>
    public virtual void SetVisible(bool visible) => gameObject.SetActive(visible);

    /// <summary>
    /// Updates the content.
    /// </summary>
    /// <param name="itemData">Item data.</param>
    public abstract void UpdateContent(CrossCardCellData itemData);

    /// <summary>
    /// Updates the position.
    /// </summary>
    /// <param name="position">Position.</param>
    public abstract void UpdatePosition(float position);

    public abstract void InitData();

    public abstract void UpdateComponentStatus();

    public abstract void ClearComponentStatus();

    public abstract void SetAsLastPosition();


}
