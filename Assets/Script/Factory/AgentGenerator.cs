using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Manager/AgentGenerator")]
public class AgentGenerator : ScriptableObject{

	//	预制体
	public FlockAgent prefab;

	//	生成物的 panel
	public Transform generatorPanel;

	#region Construct
	private static AgentGenerator instance;
	public static AgentGenerator GetInstance(){
		if (instance == null) {
			return new AgentGenerator ();
		} else {
			return instance; 
		}
	}
	#endregion


	//
	//	originPosition : item 应该身处的位置
	//	generatorPosition : item 生成的位置
	//
	public FlockAgent generator (int row_index,int column_index, Vector2 generatorPosition, Vector2 originPosition, MagicWallManager manager){

		// 创建实体
		FlockAgent newAgent = Instantiate(
			prefab,
			generatorPanel
		);
		newAgent.name = "Agent(" + row_index + "," + column_index + ")";

		// 创建初始化位置
		Vector2 postion = generatorPosition;
		newAgent.GetComponent<RectTransform>().anchoredPosition = postion;

		// 创建目标位置
		Vector2 ori_position = originPosition;

		// 初始化实体内容
		//newAgent.Initialize(manager, ori_position);

		manager.Agents.Add(newAgent);

		return newAgent;
	}

}
