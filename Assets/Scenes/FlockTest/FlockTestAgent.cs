using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlockTestAgent : MonoBehaviour {

	public RectTransform TarAgent; 	// 目标 Agent

	[Range(0,300)]
	public float TheDistance;	// 影响距离
	[Range(0,20)]
	public float MoveFactor; // 移动因素
	[Range(0,20)]
	public float ScaleFactor; // 缩放因素

	public Text text,text1,text2;

	private Vector2 ori_transform; // 原位



	void Start(){
		Debug.Log ("START!");
		ori_transform = GetComponent<RectTransform> ().anchoredPosition;
	}


	// Update is called once per frame
	void FixedUpdate () {
		RectTransform m_transform = GetComponent<RectTransform> ();

		// 根据目标物进行调整
		float distance = Vector2.Distance(ori_transform,TarAgent.anchoredPosition);
		text.text = "Distance : " + distance.ToString ();

		float offset = TheDistance - distance;
		text1.text = "Offset : " + offset.ToString ();

		if (distance < TheDistance) {
			// 进入影响范围
			if (offset > 0) {
//				Debug.Log (1f / 255f);
				float m_scale = -(1f / 255f) * offset + 1f;
				text2.text = "SCALE: " + m_scale.ToString();

				if (ori_transform.y > TarAgent.anchoredPosition.y) {
                    float to = ori_transform.y + MoveFactor * offset;
                    Vector2 toy = new Vector2(ori_transform.x, to);
					m_transform.DOAnchorPos (toy, Time.deltaTime);
				} else if (ori_transform.y < TarAgent.anchoredPosition.y){
					float to = ori_transform.y - MoveFactor * offset;
                    Vector2 toy = new Vector2(ori_transform.x, to);
                    m_transform.DOAnchorPos (toy, Time.deltaTime);
				}

				m_transform.DOScale (m_scale, Time.deltaTime);
			} 
		}

	}
}
