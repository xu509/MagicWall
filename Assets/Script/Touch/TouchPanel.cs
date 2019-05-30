using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class TouchPanel : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler,IPointerClickHandler
{
    [SerializeField] TouchAgent touchAgent;
    [SerializeField] RectTransform context;

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");

        Vector2 position = eventData.position;

        Vector2 pointerStartLocalPosition = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out pointerStartLocalPosition);


        TouchAgent agent = Instantiate(touchAgent, context);
        agent.GetComponent<RectTransform>().anchoredPosition = pointerStartLocalPosition;


    }

    //public void DoClick() {
    //    Debug.Log("OnPointerClick");

    //    Vector2 position = eventData.position;

    //    Vector2 pointerStartLocalPosition = Vector2.zero;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        GetComponent<RectTransform>(),
    //        eventData.position,
    //        eventData.pressEventCamera,
    //        out pointerStartLocalPosition);


    //    TouchAgent agent = Instantiate(touchAgent, context);
    //    agent.GetComponent<RectTransform>().anchoredPosition = pointerStartLocalPosition;

    //}

}
