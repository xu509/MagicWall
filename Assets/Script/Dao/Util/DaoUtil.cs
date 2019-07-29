using System.Collections;
using System.Collections.Generic;

public class DaoUtil
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="jsonstr"></param>
    /// <returns></returns>
    public object ConvertMaterialJson(string jsonstr) {
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

    public List<MWMaterial> ConvertMaterialAry(string jsonstr) {





        return null;
    }

    public MWMaterial ConvertMaterialObject(string jsonstr)
    {
        MWMaterial mWMaterial = new MWMaterial();
        return mWMaterial.ConvertJSONToObject(jsonstr);
    }

}
