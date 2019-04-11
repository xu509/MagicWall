using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  配置表映射
//
public class AppConfig
{
    public static string KEY_THEME_ID = "theme_id";

    // 企业 ID
    private string key;
    public string Key { set { key = value; } get { return key; } }

    // 企业的logo
    private string value;
    public string Value { set { this.value = value; } get { return value; } }
    
}





