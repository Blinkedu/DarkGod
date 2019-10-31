using System;
using System.Collections.Generic;
using System.Xml;
/// <summary>
/// 配置数据服务
/// </summary>
public class CfgSvc
{
    private static CfgSvc instance = null;
    public static CfgSvc Instance
    {
        get
        {
            if (instance == null)
                instance = new CfgSvc();
            return instance;
        }
    }
    private CfgSvc() { }

    public void Init()
    {
        InitGuideCfg();
        InitStrongCfg();
        InitTaskRewardCfg();
        InitMapCfg();
        PECommon.Log("CfgSvc Init Done.");
    }

    #region 自动引导配置
    private Dictionary<int, GuideCfg> guideDic = new Dictionary<int, GuideCfg>();
    private void InitGuideCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"F:\UnityProjects\DarkGod\Assets\Resources\ResCfgs\guide.xml");
        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;
            if (ele.GetAttributeNode("ID") == null)
                continue;
            int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            GuideCfg gc = new GuideCfg
            {
                ID = id
            };
            foreach (XmlElement e in nodLst[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "coin":
                        gc.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        gc.exp = int.Parse(e.InnerText);
                        break;
                }
            }
            guideDic.Add(id, gc);
        }
        PECommon.Log("GuideCfg Init Done.");
    }

    public GuideCfg GetGuideCfg(int id)
    {
        GuideCfg gc = null;
        if (guideDic.TryGetValue(id, out gc))
        {
            return gc;
        }
        return null;
    }
    #endregion

    #region 强化升级配置
    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();

    private void InitStrongCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"F:\UnityProjects\DarkGod\Assets\Resources\ResCfgs\strong.xml");
        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;
            if (ele.GetAttributeNode("ID") == null)
                continue;
            int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            StrongCfg sc = new StrongCfg
            {
                ID = id
            };
            foreach (XmlElement e in nodLst[i].ChildNodes)
            {
                int val = int.Parse(e.InnerText);
                switch (e.Name)
                {
                    case "pos":
                        sc.pos = val;
                        break;
                    case "starlv":
                        sc.starlv = val;
                        break;
                    case "addhp":
                        sc.addhp = val;
                        break;
                    case "addhurt":
                        sc.addhurt = val;
                        break;
                    case "adddef":
                        sc.adddef = val;
                        break;
                    case "minlv":
                        sc.minlv = val;
                        break;
                    case "coin":
                        sc.coin = val;
                        break;
                    case "crystal":
                        sc.crystal = val;
                        break;
                }
            }

            Dictionary<int, StrongCfg> dic = null;
            if (strongDic.TryGetValue(sc.pos, out dic))
            {
                dic.Add(sc.starlv, sc);
            }
            else
            {
                dic = new Dictionary<int, StrongCfg>
                    {
                        { sc.starlv, sc }
                    };
                strongDic.Add(sc.pos, dic);
            }
        }
        PECommon.Log("StrongCfg Init Done.");
    }

    public StrongCfg GetStrongCfg(int pos, int starlv)
    {
        StrongCfg sc = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongDic.TryGetValue(pos, out dic))
        {
            if (dic.TryGetValue(starlv, out sc))
            {
                return sc;
            }
        }
        return null;
    }
    #endregion

    #region 任务奖励配置
    private Dictionary<int, TaskRewardCfg> taskRewardDic = new Dictionary<int, TaskRewardCfg>();
    private void InitTaskRewardCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"F:\UnityProjects\DarkGod\Assets\Resources\ResCfgs\taskreward.xml");
        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;
            if (ele.GetAttributeNode("ID") == null)
                continue;
            int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            TaskRewardCfg trc = new TaskRewardCfg
            {
                ID = id
            };
            foreach (XmlElement e in nodLst[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "count":
                        trc.count = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        trc.exp = int.Parse(e.InnerText);
                        break;
                    case "coin":
                        trc.coin = int.Parse(e.InnerText);
                        break;
                }
            }
            taskRewardDic.Add(id, trc);
        }
        PECommon.Log("TaskRewardCfg Init Done.");
    }

    public TaskRewardCfg GetTaskRewardCfg(int id)
    {
        TaskRewardCfg trc = null;
        if (taskRewardDic.TryGetValue(id, out trc))
        {
            return trc;
        }
        return null;
    }
    #endregion

    #region 地图配置
    private Dictionary<int, MapCfg> mapDict = new Dictionary<int, MapCfg>();
    private void InitMapCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"F:\UnityProjects\DarkGod\Assets\Resources\ResCfgs\map.xml");
        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;
            if (ele.GetAttributeNode("ID") == null)
                continue;
            int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            MapCfg mc = new MapCfg
            {
                ID = id
            };
            foreach (XmlElement e in nodLst[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "power":
                        mc.power = int.Parse(e.InnerText);
                        break;
                    case "coin":
                        mc.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        mc.exp = int.Parse(e.InnerText);
                        break;
                    case "crystal":
                        mc.crystal = int.Parse(e.InnerText);
                        break;
                }
            }
            mapDict.Add(id, mc);
        }
        PECommon.Log("MapCfg Init Done.");
    }

    public MapCfg GetMapCfg(int id)
    {
        MapCfg mc = null;
        if (mapDict.TryGetValue(id, out mc))
        {
            return mc;
        }
        return null;
    }
    #endregion

}

/// <summary>
/// 配置数据类
/// </summary>
public class BaseData<T>
{
    public int ID;
}

public class GuideCfg : BaseData<GuideCfg>
{
    public int coin;
    public int exp;
}

public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int starlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}

public class TaskRewardCfg : BaseData<StrongCfg>
{
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}

public class MapCfg : BaseData<MapCfg>
{
    public int power;
    public int coin;
    public int exp;
    public int crystal;
}