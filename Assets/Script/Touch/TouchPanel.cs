using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class TouchPanel : MonoBehaviour
{
    [SerializeField] TouchAgent touchAgent;
    [SerializeField] RectTransform context;
    [SerializeField] GraphicRaycaster graphicRaycaster;
    [SerializeField] EventSystem _mEventSystem;



    public void Update() {


        var mPointerEventData = new PointerEventData(_mEventSystem);
        var raycasterList = new List<RaycastResult>(); ;

        //graphicRaycaster.Raycast(mPointerEventData, raycasterList);

        //for (int i = 0; i < raycasterList.Count; i++) {
        //    Debug.Log("Game Object Name:" + raycasterList[i].gameObject.name);
        //}

        //if (raycasterList.Count > 0) {
        //    Debug.Log("Click  !!! ");
        //}


    }


    public void OnClick() {

        Debug.Log("On Click");
    }

    public void OnDrag()
    {

        Debug.Log("On Drag");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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
