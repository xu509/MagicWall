using System;
using UnityEngine;
using UnityEngine.UI;

public class CrossCardScrollViewContext 
{
    public int SelectedIndex = -1;
    public Action<int> OnCellClicked;
    public Action<Texture> OnScaleClicked; //点击放大
    public Action<string> OnDescriptionChanged;
    public Action<CrossCardCellData> OnPlayVideo;



}
