using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DaoUtil
{
    /// <summary>
    ///     根据Json字符串，获取物流实例
    /// </summary>
    /// <param name="jsonstr"></param>
    /// <returns>ary： List<MWMaterial> / obj ： MWMaterial</returns>
    public static object ConvertMaterialJson(string jsonstr) {
        if (jsonstr == null)
            return null;



        // 如果含有 '[',']'标志，则解析为数组
        bool isArray = false;

        if (jsonstr.Contains("[") && jsonstr.Contains("]")) {
            isArray = true;
        }

        if (isArray)
        {
            return ConvertMaterialAry(jsonstr);
        }
        else {
            return ConvertMaterialObject(jsonstr);

        }


    }

    private static List<MWMaterial> ConvertMaterialAry(string jsonstr) {
        jsonstr = jsonstr.Replace("[","");
        jsonstr = jsonstr.Replace("]","");
        string[] datas = Regex.Split(jsonstr, "},{", RegexOptions.IgnoreCase);

        List<MWMaterial> mWMaterials = new List<MWMaterial>();
        if (datas.Length > 0) {
            for (int i = 0; i < datas.Length; i++) {
                mWMaterials.Add(ConvertMaterialObject(datas[i]));
            }
        }
        return mWMaterials;
    }

    private static MWMaterial ConvertMaterialObject(string jsonstr)
    {
        MWMaterial mWMaterial = new MWMaterial();
        return mWMaterial.ConvertJSONToObject(jsonstr);
    }

}
