using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  物料
///  
///  example - 
///  {type:'video',path:'/uploads/test.mp4',description:'文字描述1',cover:'/uploads/20190704/4a3d48f0e4123e3b2a3ae7132037315f.png'}
/// 
/// 
/// </summary>
public class MWMaterial
{
    string _type;
    string _path;
    string _description;
    string _cover;

    public string type { set { _type = value; } get { return _type; } }
    public string path { set { _path = value; } get { return _path; } }
    public string description { set { _description = value; } get { return _description; } }
    public string cover { set { _cover = value; } get { return _cover; } }

    public MWMaterial ConvertJSONToObject(string str) {

        if (str.Contains("type")) {
            str = str.Replace("type", "'type'");
        }

        if (str.Contains("path"))
        {
            str = str.Replace("path", "'path'");
        }

        if (str.Contains("description"))
        {
            str = str.Replace("description", "'description'");
        }

        if (str.Contains("cover"))
        {
            str = str.Replace("cover", "'cover'");
        }

        //检查左右括号
        if (str.IndexOf('{') < 0) {
            str = '{' + str;
        }

        if (str.IndexOf('}') < 0)
        {
            str = str + '}';
        }


        JsonData data = JsonMapper.ToObject(str);
        MWMaterial mWMaterial = new MWMaterial();
        mWMaterial.type = (string)data["type"];
        mWMaterial.path = (string)data["path"];
        mWMaterial.description = (string)data["description"];
        mWMaterial.cover = (string)data["cover"];

        return mWMaterial;
    }

    public override string ToString()
    {
        string str = "";

        if (type != null) {
            str += "[type] = " + type + " "; 
        }

        if (_path != null)
        {
            str += "[path] = " + _path + " ";
        }

        if (description != null)
        {
            str += "[description] = " + description + " ";
        }

        if (_cover != null)
        {
            str += "[cover] = " + _cover + " ";
        }


        return str;
    }

}
