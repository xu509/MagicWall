using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(MockSceneConfig))]
public class MockDaoServiceEditor : Editor
{
    //public SceneConfig[] _sceneConfigs;
    private static float _effect_width = 100f;
    private static float _type_width = 80f;
    private static float _durtime_width = 50f;
    private static float _tool_width = 50f;


    public override void OnInspectorGUI()
    {
        MockSceneConfig cb = (MockSceneConfig)target;
        //CopyValue(cb);

        //Debug.Log("Mock Scene Config is NULL : " + cb == null);


        var _sceneConfigs = cb.sceneConfigs;


        cb.data =  EditorGUILayout.FloatField(cb.data, GUILayout.Width(_durtime_width));

        //Rect r = EditorGUILayout.BeginHorizontal();
        //r.height = EditorGUIUtility.singleLineHeight * 1.2f;

        if (_sceneConfigs == null || _sceneConfigs.Count == 0)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("No behaviors in array.", MessageType.Warning);
            EditorGUILayout.EndHorizontal();
            //r = EditorGUILayout.BeginHorizontal();
            //r.height = EditorGUIUtility.singleLineHeight;
        }
        else
        {


            Rect r = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("effect", GUILayout.Width(_effect_width));
            EditorGUILayout.LabelField("type", GUILayout.Width(_type_width));
            EditorGUILayout.LabelField("durtime", GUILayout.Width(_durtime_width));
            EditorGUILayout.LabelField("tool", GUILayout.Width(_tool_width));
            EditorGUILayout.LabelField("up", GUILayout.Width(_tool_width));
            EditorGUILayout.LabelField("down", GUILayout.Width(_tool_width));


            EditorGUILayout.EndHorizontal();


            for (int i = 0; i < _sceneConfigs.Count; i++)
            {
                var config = _sceneConfigs[i];

                r = EditorGUILayout.BeginHorizontal();
                r.width = 150f;
                r.height = EditorGUIUtility.singleLineHeight * 1.2f;
                config.sceneType = (SceneTypeEnum)EditorGUILayout.EnumPopup(config.sceneType, GUILayout.Width(_effect_width));
                config.dataType = (DataType)EditorGUILayout.EnumPopup(config.dataType, GUILayout.Width(_type_width));
                config.durtime = EditorGUILayout.FloatField(config.durtime, GUILayout.Width(_durtime_width));

                if (GUILayout.Button("DEL",GUILayout.Width(_tool_width)))
                {
                    Del(cb, i);
                    EditorUtility.SetDirty(cb);
                }

                // 显示向上移动
                if (i > 0)
                {
                    if (GUILayout.Button("UP", GUILayout.Width(_tool_width)))
                    {
                        Up(cb, i);
                        EditorUtility.SetDirty(cb);
                    }
                }
                else {
                    GUILayout.Space(_tool_width + 5);
                }

                // 显示向下移动
                if (i < _sceneConfigs.Count - 1) {
                    if (GUILayout.Button("down", GUILayout.Width(_tool_width)))
                    {
                        Down(cb, i);
                        EditorUtility.SetDirty(cb);
                    }
                }
                else
                {
                    GUILayout.Space(_tool_width);
                }



                EditorGUILayout.EndHorizontal();
            }
        }

            EditorGUILayout.LabelField("", GUILayout.Width(150f));


        if (GUILayout.Button("Add")) {
            Add(cb);
            EditorUtility.SetDirty(cb);
        }

        //EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());


        if (GUI.changed) {
            EditorUtility.SetDirty(cb);
        }

        

    }

    void Add(MockSceneConfig cb)
    {
        var configs = cb.sceneConfigs;


        int oldCount = (configs != null) ? configs.Count : 0;

        SceneConfig[] nsceneConfig = new SceneConfig[oldCount + 1];

        for (int i = 0; i < oldCount; i++)
        {
            nsceneConfig[i] = configs[i];
            //newWeights[i] = cb.weights[i];
        }

        SceneConfig n = new SceneConfig(SceneTypeEnum.CurveStagger, DataType.activity, 5f);

        configs.Add(n);


        //nsceneConfig[nsceneConfig.Length - 1] = n;

        //cb.sceneConfigs = nsceneConfig;

        cb.sceneConfigs = configs;
    }



    void Del(MockSceneConfig cb,int index)
    {
        var configs = cb.sceneConfigs;
        configs.RemoveAt(index);
        //ArrayUtility.RemoveAt(ref configs, index);
        cb.sceneConfigs = configs;
    }


    /// <summary>
    ///     向上移动
    /// </summary>
    /// <param name="cb"></param>
    /// <param name="index"></param>
    void Up(MockSceneConfig cb, int index)
    {
        var configs = cb.sceneConfigs;
        int to = index - 1;

        var temp = configs[to];
        configs[to] = configs[index];
        configs[index] = temp;
       
        cb.sceneConfigs = configs;
    }


    /// <summary>
    ///     向下移动
    /// </summary>
    /// <param name="cb"></param>
    /// <param name="index"></param>
    void Down(MockSceneConfig cb, int index)
    {
        var configs = cb.sceneConfigs;
        int to = index + 1;

        var temp = configs[to];
        configs[to] = configs[index];
        configs[index] = temp;

        cb.sceneConfigs = configs;
    }



}
