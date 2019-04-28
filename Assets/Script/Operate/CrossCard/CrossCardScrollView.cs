using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossCardScrollView : FancyScrollView<CrossCardCellData>
{

    [SerializeField] ScrollPositionController scrollPositionController;
    [SerializeField] GameObject cellPrefab;

    protected override GameObject CellPrefab => cellPrefab;

    void Start()
    {
        scrollPositionController.OnUpdatePosition(p => UpdatePosition(p));
    }

    public void UpdateData(IList<CrossCardCellData> cellData)
    {
        UpdateContents(cellData);
        scrollPositionController.SetDataCount(cellData.Count);
    }

}