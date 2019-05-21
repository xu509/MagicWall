using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestScript1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// 缩小
		RectTransform rt = GetComponent<RectTransform>();

		//缩小至右下
		rt.DOScale (0.5f, Time.deltaTime);
		Vector2 v = new Vector2 (25, -25);
		rt.DOAnchorPos (rt.anchoredPosition + v,Time.deltaTime);
		BoxCollider2D collider = GetComponent<BoxCollider2D> ();
		collider.edgeRadius = collider.edgeRadius / 2;


//		Debug.Log (r.height);
//
//		r.height = 100;
//		r.width = 100;
//


//		float w = rt.rect.width;
//		float h = rt.rect.height;
//
//		rt.rect.width = w / 2;
//		rt.rect.height = h / 2;

			



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
