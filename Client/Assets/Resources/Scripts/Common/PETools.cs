using UnityEngine;
using System.Collections;

/// <summary>
/// 工具类
/// </summary>
public class PETools
{
    public static int RDInt(int min, int max, System.Random rd = null)
    {
        if (rd == null)
            rd = new System.Random();
        int val = rd.Next(min, max + 1);
        return val;
    }
}