using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButtonComponent : MonoBehaviour,MoveBtnObserver
{
    [SerializeField] Sprite btn_move_sprite;
    [SerializeField] Sprite btn_move_sprite_active;
    [SerializeField] RectTransform btn_container;

    [SerializeField] CardAgent _moveSubject;

    private bool isMoving = false;

    private Action _DoClickAction;

    // Start is called before the first frame update
    void Start()
    {
    }


    private void OnDestroy()
    {
        _moveSubject?.RemoveObserver(this);
    }


    public void Init(Action doClickAction , CardAgent cardAgent) {
        Debug.Log("Init");


        _DoClickAction = doClickAction;
        _moveSubject = cardAgent;

        _moveSubject.AddObserver(this);
    }


    public void DoMove() {

        //UpdateComponentStatus();

        _DoClickAction.Invoke();
    }


    void UpdateComponentStatus() {
        if (!isMoving)
        {
            btn_container.GetComponent<Image>().sprite = btn_move_sprite_active;
        }
        else
        {
            btn_container.GetComponent<Image>().sprite = btn_move_sprite;
        }

        isMoving = !isMoving;
    }



    void MoveBtnObserver.Update()
    {
        UpdateComponentStatus();
    }
}
