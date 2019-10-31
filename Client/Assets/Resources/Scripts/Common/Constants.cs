using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 常量配置
/// </summary>
public class Constants
{
    private const string ColorRed = "<color=#FF0000FF>"; 
    private const string ColorGreen = "<color=#00FF00FF>"; 
    private const string ColorBlue = "<color=#00B4FFFF>"; 
    private const string ColorYellow = "<color=#FFFF00FF>"; 
    private const string ColorEnd = "</color>"; 

    public static string Color(string str,TextColor color)
    {
        string res = "";
        switch (color)
        {
            case TextColor.Red:
                res = ColorRed + str + ColorEnd;
                break;
            case TextColor.Green:
                res = ColorGreen + str + ColorEnd;
                break;
            case TextColor.Blue:
                res = ColorBlue + str + ColorEnd;
                break;
            case TextColor.Yellow:
                res = ColorYellow + str + ColorEnd;
                break;
        }
        return res;
    }
    
    // AutoGuideNPC
    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;

    // 场景名称/ID
    public const string SceneLogin = "SceneLogin";
    public const int MainCityMapID = 10000;
    //public const string SceneMainCity = "SceneMainCity";

    // 音效名称
    public const string BGLogin = "bgLogin";
    public const string BGMainCity = "bgMainCity";
    public const string BGHuangYe = "bgHuangYe";
    public const string AssassinHit = "assassin_Hit";

    // 登录按钮音效
    public const string UILoginBtn = "uiLoginBtn";
    
    // 常规UI点击音效
    public const string UIClickBtn = "uiClickBtn";
    public const string UIOpenPage = "uiOpenPage";
    public const string FBItemEnter = "fbitem";

    public const string FBLose = "fblose";
    public const string FBLogoEnter = "fbwin";

    // 主城菜单按钮点击音效
    public const string UIExtenBtn = "uiExtenBtn";

    // 屏幕标准宽高
    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeight = 750;

    // 摇杆标准距离
    public const int ScreenOPDis = 90;

    // Action触发参数
    public const int ActionDefault = -1;
    public const int ActionBorn = 0;
    public const int ActionDie = 100;
    public const int ActionHit = 101;

    public const int DieAinLength = 5000;
    
    // 混合参数
    public const int BlendIdle = 0;
    public const int BlendMove = 1;

    // 角色移动速度
    public const float PlayerMoveSpeed = 8f;
    public const float MonsterMoveSpeed = 3f;

    // 运动平滑加速度
    public const float AccelerSpeed = 5f;
    public const float AccelerHPSpeed = 0.3f;

    // 普攻连招有效间隔
    public const int ComboSpace = 500;
}

public enum TextColor
{
    Red,
    Green,
    Blue,
    Yellow
}

public enum DamageType
{
    None,
    AD = 1, // 物理伤害
    AP = 2, // 魔法伤害
}

public enum EntityType
{
    None,
    Player,
    Monster
}

public enum EntityState
{
    None,
    BatiState, // 霸体状态: 不可控制，可受伤害
}

public enum MonsterType
{
    None,
    Normal = 1,
    Boss = 2,
}