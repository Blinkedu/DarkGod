using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 资源加载服务
/// </summary>
public class ResSvc : MonoBehaviour
{
    public static ResSvc Instance = null;

    public void InitSvc()
    {
        Instance = this;
        InitRDNameCfg(PathDefine.RDNameCfg);
        InitMonsterCfg(PathDefine.MonsterCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitGuideCfg(PathDefine.GuideCfg);
        InitStrongCfg(PathDefine.StrongCfg);
        InitTaskReward(PathDefine.TaskRewardCfg);
        InitSkillCfg(PathDefine.SkillCfg);
        InitSkillMoveCfg(PathDefine.SkillMoveCfg);
        InitSkillActionCfg(PathDefine.SkillActionCfg);
        PECommon.Log("Init ResSvc...");
    }


    private Action prgCB = null;
    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        GameRoot.Instance.loadingWnd.SetWndState();
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        prgCB = () =>
        {
            float val = sceneAsync.progress;
            GameRoot.Instance.loadingWnd.SetProgress(val);
            if (val == 1)
            {
                if (loaded != null)
                    loaded();
                prgCB = null;
                sceneAsync = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };
    }

    private void Update()
    {
        if (prgCB != null)
            prgCB();
    }

    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path, bool cache = false)
    {
        AudioClip clip = null;
        if (!adDic.TryGetValue(path, out clip))
        {
            clip = Resources.Load<AudioClip>(path);
            if (cache)
                adDic.Add(path, clip);
        }
        return clip;
    }

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache == true)
            {
                goDic.Add(path, prefab);
            }
        }
        GameObject go = null;
        if (prefab != null)
        {
            go = GameObject.Instantiate(prefab);
        }
        return go;
    }

    private Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path, bool cache = false)
    {
        Sprite sprite = null;
        if (!spriteDic.TryGetValue(path, out sprite))
        {
            sprite = Resources.Load<Sprite>(path);
            if (cache == true)
            {
                spriteDic.Add(path, sprite);
            }
        }
        return sprite;
    }

    #region InitCfgs
    #region 随机名字
    private List<string> surnameLst = new List<string>();
    private List<string> manLst = new List<string>();
    private List<string> womanLst = new List<string>();
    private void InitRDNameCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml == null)
        {
            PECommon.Log("xml flie:" + PathDefine.RDNameCfg + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                    continue;
                //int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname":
                            surnameLst.Add(e.InnerText);
                            break;
                        case "man":
                            manLst.Add(e.InnerText);
                            break;
                        case "woman":
                            womanLst.Add(e.InnerText);
                            break;
                    }
                }
            }
        }
    }

    public string GetRDNameData(bool man = true)
    {
        System.Random rd = new System.Random();
        string rdName = surnameLst[PETools.RDInt(0, surnameLst.Count - 1, rd)];
        if (man)
        {
            rdName += manLst[PETools.RDInt(0, manLst.Count - 1, rd)];
        }
        else
        {
            rdName += womanLst[PETools.RDInt(0, womanLst.Count - 1, rd)];
        }
        return rdName;
    }
    #endregion

    #region 地图
    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
    private void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml == null)
        {
            PECommon.Log("xml flie:" + PathDefine.RDNameCfg + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MapCfg mc = new MapCfg
                {
                    ID = id,
                    monsterLst = new List<MonsterData>()
                };
                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mapName":
                            mc.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mc.sceneName = e.InnerText;
                            break;
                        case "power":
                            mc.power = int.Parse(e.InnerText);
                            break;
                        case "mainCamPos":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.mainCamPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "mainCamRote":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.mainCamRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "playerBornPos":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.playerBornPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "playerBornRote":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.playerBornRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "monsterLst":
                            {
                                string[] valArr = e.InnerText.Split('#');
                                for (int waveIndex = 0; waveIndex < valArr.Length; waveIndex++)
                                {
                                    if (waveIndex == 0) continue;
                                    string[] tempArr = valArr[waveIndex].Split('|');
                                    for (int j = 0; j < tempArr.Length; j++)
                                    {
                                        if (j == 0) continue;
                                        string[] arr = tempArr[j].Split(',');
                                        MonsterData md = new MonsterData
                                        {
                                            ID = int.Parse(arr[0]),
                                            mWave = waveIndex,
                                            mIndex = j,
                                            mCfg = GetMonsterCfg(int.Parse(arr[0])),
                                            mBornPos = new Vector3(float.Parse(arr[1]), float.Parse(arr[2]), float.Parse(arr[3])),
                                            mBornRote = new Vector3(0, float.Parse(arr[4]), 0),
                                            mLevel = int.Parse(arr[5])
                                        };
                                        mc.monsterLst.Add(md);
                                    }
                                }
                            }
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
                mapCfgDataDic.Add(id, mc);
            }
        }
    }
    public MapCfg GetMapCfg(int id)
    {
        MapCfg data;
        if (mapCfgDataDic.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }
    #endregion

    #region 怪物
    private Dictionary<int, MonsterCfg> monsterCfgDataDic = new Dictionary<int, MonsterCfg>();
    private void InitMonsterCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml == null)
        {
            PECommon.Log("xml flie:" + PathDefine.RDNameCfg + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MonsterCfg mc = new MonsterCfg
                {
                    ID = id,
                    bps = new BattleProps()
                };
                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mName":
                            mc.mName = e.InnerText;
                            break;
                        case "mType":
                            if (e.InnerText.Equals("1"))
                            {
                                mc.mType = MonsterType.Normal;
                            }
                            else if (e.InnerText.Equals("2"))
                            {
                                mc.mType = MonsterType.Boss;
                            }
                            break;
                        case "isStop":
                            mc.isStop = int.Parse(e.InnerText) == 1;
                            break;
                        case "resPath":
                            mc.resPath = e.InnerText;
                            break;
                        case "skillID":
                            mc.skillID = int.Parse(e.InnerText);
                            break;
                        case "atkDis":
                            mc.atkDis = float.Parse(e.InnerText);
                            break;
                        case "hp":
                            mc.bps.hp = int.Parse(e.InnerText);
                            break;
                        case "ad":
                            mc.bps.ad = int.Parse(e.InnerText);
                            break;
                        case "ap":
                            mc.bps.ap = int.Parse(e.InnerText);
                            break;
                        case "addef":
                            mc.bps.addef = int.Parse(e.InnerText);
                            break;
                        case "apdef":
                            mc.bps.apdef = int.Parse(e.InnerText);
                            break;
                        case "dodge":
                            mc.bps.dodge = int.Parse(e.InnerText);
                            break;
                        case "pierce":
                            mc.bps.pierce = int.Parse(e.InnerText);
                            break;
                        case "critical":
                            mc.bps.critical = int.Parse(e.InnerText);
                            break;
                    }
                }
                monsterCfgDataDic.Add(id, mc);
            }
        }
    }
    public MonsterCfg GetMonsterCfg(int id)
    {
        MonsterCfg data;
        if (monsterCfgDataDic.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }
    #endregion

    #region 自动引导配置
    private Dictionary<int, AutoGuideCfg> guideTaskDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml == null)
        {
            PECommon.Log("xml flie:" + PathDefine.RDNameCfg + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                AutoGuideCfg agc = new AutoGuideCfg
                {
                    ID = id
                };
                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID":
                            agc.npcID = int.Parse(e.InnerText);
                            break;
                        case "dilogArr":
                            agc.dilogArr = e.InnerText;
                            break;
                        case "actID":
                            agc.actID = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            agc.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            agc.exp = int.Parse(e.InnerText);
                            break;
                    }
                }
                guideTaskDic.Add(id, agc);
            }
        }
    }

    public AutoGuideCfg GetAutoGuideCfg(int id)
    {
        AutoGuideCfg agc = null;
        if (guideTaskDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return null;
    }
    #endregion

    #region 强化升级配置
    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();

    private void InitStrongCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml == null)
        {
            PECommon.Log("xml flie:" + PathDefine.RDNameCfg + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
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
        }
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

    public int GetPropAddValPreLv(int pos, int starlv, int type)
    {
        Dictionary<int, StrongCfg> posDic = null;
        int val = 0;
        if (strongDic.TryGetValue(pos, out posDic))
        {
            for (int i = 0; i < starlv; i++)
            {
                StrongCfg sd;
                if (posDic.TryGetValue(i, out sd))
                {
                    switch (type)
                    {
                        case 1://hp
                            val += sd.addhp;
                            break;
                        case 2:// hurt
                            val += sd.addhurt;
                            break;
                        case 3://def
                            val += sd.adddef;
                            break;
                    }
                }
            }
        }
        return val;
    }
    #endregion

    #region 任务奖励配置
    private Dictionary<int, TaskRewardCfg> taskRewardDic = new Dictionary<int, TaskRewardCfg>();
    private void InitTaskReward(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml == null)
        {
            PECommon.Log("xml flie:" + PathDefine.RDNameCfg + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
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
                        case "taskName":
                            trc.taskName = e.InnerText;
                            break;
                        case "count":
                            trc.count = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            trc.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            trc.exp = int.Parse(e.InnerText);
                            break;
                    }
                }
                taskRewardDic.Add(id, trc);
            }
        }
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

    #region 技能配置
    private Dictionary<int, SkillCfg> skillDic = new Dictionary<int, SkillCfg>();
    private void InitSkillCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml == null)
        {
            PECommon.Log("xml flie:" + PathDefine.RDNameCfg + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                SkillCfg sc = new SkillCfg
                {
                    ID = id,
                    skillMoveLst = new List<int>(),
                    skillActionLst = new List<int>(),
                    skillDamageLst = new List<int>()
                };
                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "skillName":
                            sc.skillName = e.InnerText;
                            break;
                        case "cdTime":
                            sc.cdTime = int.Parse(e.InnerText);
                            break;
                        case "skillTime":
                            sc.skillTime = int.Parse(e.InnerText);
                            break;
                        case "aniAction":
                            sc.aniAction = int.Parse(e.InnerText);
                            break;
                        case "fx":
                            sc.fx = e.InnerText;
                            break;
                        case "isCombo":
                            sc.isCombo = e.InnerText.Equals("1");
                            break;
                        case "isCollide":
                            sc.isCollide = e.InnerText.Equals("1");
                            break;
                        case "isBreak":
                            sc.isBreak = e.InnerText.Equals("1");
                            break;
                        case "dmgType":
                            sc.dmgType = (DamageType)int.Parse(e.InnerText);
                            break;
                        case "skillMoveLst":
                            string[] skMoveArr = e.InnerText.Split('|');
                            for (int j = 0; j < skMoveArr.Length; j++)
                            {
                                if (skMoveArr[j] != "")
                                {
                                    sc.skillMoveLst.Add(int.Parse(skMoveArr[j]));
                                }
                            }
                            break;
                        case "skillActionLst":
                            string[] skActionArr = e.InnerText.Split('|');
                            for (int j = 0; j < skActionArr.Length; j++)
                            {
                                if (skActionArr[j] != "")
                                {
                                    sc.skillActionLst.Add(int.Parse(skActionArr[j]));
                                }
                            }
                            break;
                        case "skillDamageLst":
                            string[] skDamageArr = e.InnerText.Split('|');
                            for (int j = 0; j < skDamageArr.Length; j++)
                            {
                                if (skDamageArr[j] != "")
                                {
                                    sc.skillDamageLst.Add(int.Parse(skDamageArr[j]));
                                }
                            }
                            break;
                    }
                }
                skillDic.Add(id, sc);
            }
        }
    }

    public SkillCfg GetSkillCfg(int id)
    {
        SkillCfg sc = null;
        if (skillDic.TryGetValue(id, out sc))
        {
            return sc;
        }
        return null;
    }
    #endregion

    #region 技能移动配置
    private Dictionary<int, SkillMoveCfg> skillMoveDic = new Dictionary<int, SkillMoveCfg>();
    private void InitSkillMoveCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml == null)
        {
            PECommon.Log("xml flie:" + PathDefine.RDNameCfg + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                SkillMoveCfg smc = new SkillMoveCfg
                {
                    ID = id
                };
                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "delayTime":
                            smc.delayTime = int.Parse(e.InnerText);
                            break;
                        case "moveTime":
                            smc.moveTime = int.Parse(e.InnerText);
                            break;
                        case "moveDis":
                            smc.moveDis = float.Parse(e.InnerText);
                            break;
                    }
                }
                skillMoveDic.Add(id, smc);
            }
        }
    }

    public SkillMoveCfg GetSkillMoveCfg(int id)
    {
        SkillMoveCfg smc = null;
        if (skillMoveDic.TryGetValue(id, out smc))
        {
            return smc;
        }
        return null;
    }
    #endregion

    #region 技能范围配置
    private Dictionary<int, SkillActionCfg> skillActionDic = new Dictionary<int, SkillActionCfg>();
    private void InitSkillActionCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml == null)
        {
            PECommon.Log("xml flie:" + PathDefine.RDNameCfg + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                SkillActionCfg sac = new SkillActionCfg
                {
                    ID = id
                };
                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "delayTime":
                            sac.delayTime = int.Parse(e.InnerText);
                            break;
                        case "radius":
                            sac.radius = float.Parse(e.InnerText);
                            break;
                        case "angle":
                            sac.angle = int.Parse(e.InnerText);
                            break;
                    }
                }
                skillActionDic.Add(id, sac);
            }
        }
    }

    public SkillActionCfg GetSkillActionCfg(int id)
    {
        SkillActionCfg sac = null;
        if (skillActionDic.TryGetValue(id, out sac))
        {
            return sac;
        }
        return null;
    }
    #endregion

    #endregion


    // Test
    public void ResetSkillCfgs()
    {
        skillDic.Clear();
        skillMoveDic.Clear();
        InitSkillCfg(PathDefine.SkillCfg);
        InitSkillMoveCfg(PathDefine.SkillMoveCfg);
        PECommon.Log("Reset SkillCfgs ...");
    }
}
