using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WritePadAgent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("WritePadAgent :On Begin Drag ");
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("WritePadAgent :On Drag ");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("WritePadAgent :On End Drag ");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
