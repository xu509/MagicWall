using UnityEngine;
using System.Collections.Generic;


public class CrossCardScrollView : FancyScrollView<CrossCardScrollViewCellData>
{
    [SerializeField] ScrollPositionController scrollPositionController;
    [SerializeField] GameObject cellPrefab;

    protected override GameObject CellPrefab => cellPrefab;

    void Start()
    {
        scrollPositionController.OnUpdatePosition(p => UpdatePosition(p));
    }

    public void UpdateData(IList<CrossCardScrollViewCellData> cellData)
    {
        UpdateContents(cellData);
        scrollPositionController.SetDataCount(cellData.Count);
    }
}

