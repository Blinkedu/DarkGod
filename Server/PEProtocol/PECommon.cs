
using PENet;
using PEProtocol;
/// <summary>
/// 客户端服务端共用工具类
/// </summary>
public enum LogType
{
    Log = 0,
    Warning = 1,
    Error = 2,
    Info =  3
} 

public class PECommon
{
    public const int PowerAddSpace = 5;// 分钟
    public const int PowerAddCount = 2;// 分钟

    public static void Log(string msg = "",LogType tp = LogType.Log)
    {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }

    public static int GetFightByProps(PlayerData pd)
    {
        return pd.lv * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

    public static int GetPowerLimit(int lv)
    {
        return ((lv - 1) / 10) * 150 + 150;
    }

    public static int GetExpUpValByLv(int lv)
    {
        return 100 * lv * lv;
    }

    public static void CalcExp(PlayerData pd, int addExp)
    {
        int curtLv = pd.lv;
        int curtExp = pd.exp;
        int addRestExp = addExp;

        while (true)
        {
            int upNeedExp = GetExpUpValByLv(curtLv) - curtExp;
            if (addRestExp >= upNeedExp)
            {
                curtLv++;
                curtExp = 0;
                addExp -= upNeedExp;
            }
            else
            {
                pd.lv = curtLv;
                pd.exp = curtExp + addRestExp;
                break;
            }
        }
    }

}
