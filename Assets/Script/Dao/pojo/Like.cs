using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     喜欢
/// </summary>

namespace MagicWall
{
    [System.Serializable]
    public class Like
    {
        private int _number;    // 喜欢数
        private string _path;   // 文件地址 ， 相对路径

        public int Number { get => _number; set => _number = value; }
        public string Path { get => _path; set => _path = value; }
    }
}