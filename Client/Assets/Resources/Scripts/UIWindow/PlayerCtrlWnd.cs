using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家控制界面
/// </summary>
public class PlayerCtrlWnd : WindowRoot
{
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;

    public Transform expPrgTrans;

    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;
    public Vector2 currentDir = Vector2.zero;

    public Text txtSelfHP;
    public Image imgSelfHP;
    private int hpSum;

    protected override void InitWnd()
    {
        base.InitWnd();

        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirPoint, false);

        hpSum = GameRoot.Instance.PlayerData.hp;
        SetText(txtSelfHP, hpSum + "/" + hpSum);
        imgSelfHP.fillAmount = 1f;

        SetBossHPBarState(false);
        RegisterTouchEvts();
        sk1CDTime = resSvc.GetSkillCfg(101).cdTime / 1000f;
        sk2CDTime = resSvc.GetSkillCfg(102).cdTime / 1000f;
        sk3CDTime = resSvc.GetSkillCfg(103).cdTime / 1000f;
        RefreshUI();

    }

    public void RefreshUI()
    {
        PlayerData playerData = GameRoot.Instance.PlayerData;
        SetText(txtLevel, playerData.lv);
        SetText(txtName, playerData.name);

        // Expprg
        int expPrg = (int)((float)playerData.exp / PECommon.GetExpUpValByLv(playerData.lv) * 100);
        SetText(txtExpPrg, expPrg + "%");
        int index = expPrg / 10;
        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = (float)(expPrg % 10) / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }
        //

    }


    public void RegisterTouchEvts()
    {
        OnClickDown(imgTouch.gameObject, e =>
        {
            startPos = e.position;
            SetActive(imgDirPoint);
            imgDirBg.transform.position = e.position;
        });

        OnClickUp(imgTouch.gameObject, e =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirPoint, false);
            imgDirPoint.transform.localPosition = Vector2.zero;
            currentDir = Vector2.zero;

            // 方向信息传递
            BattleSys.Instance.SetMoveDir(currentDir);
        });

        OnDrag(imgTouch.gameObject, e =>
        {
            Vector2 dir = e.position - startPos;
            float len = dir.magnitude;
            if (len > pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else
            {
                imgDirPoint.transform.position = e.position;
            }

            // 方向信息传递
            currentDir = dir.normalized;
            BattleSys.Instance.SetMoveDir(currentDir);
        });
    }

    public void ClickNormalAtk()
    {
        BattleSys.Instance.ReqReleaseSkill(0);
    }

    #region Skill1
    public Image imgSk1CD;
    public Text txtSk1CD;
    private bool isSk1CD = false;
    private float sk1CDTime;
    private int sk1Num;
    private float sk1FillCount = 0;
    private float sk1NumCount = 0;
    #endregion
    public void ClickSkill1Atk()
    {
        if (!isSk1CD && GetCanRlsSkill())
        {
            BattleSys.Instance.ReqReleaseSkill(1);
            isSk1CD = true;
            SetActive(imgSk1CD);
            imgSk1CD.fillAmount = 1;
            sk1Num = (int)sk1CDTime;
            SetText(txtSk1CD, sk1Num);
        }
    }

    #region Skill2
    public Image imgSk2CD;
    public Text txtSk2CD;
    private bool isSk2CD = false;
    private float sk2CDTime;
    private int sk2Num;
    private float sk2FillCount = 0;
    private float sk2NumCount = 0;
    #endregion
    public void ClickSkill2Atk()
    {
        if (!isSk2CD && GetCanRlsSkill())
        {
            BattleSys.Instance.ReqReleaseSkill(2);
            isSk2CD = true;
            SetActive(imgSk2CD);
            imgSk2CD.fillAmount = 1;
            sk2Num = (int)sk2CDTime;
            SetText(txtSk2CD, sk2Num);
        }
    }

    #region Skill3
    public Image imgSk3CD;
    public Text txtSk3CD;
    private bool isSk3CD = false;
    private float sk3CDTime;
    private int sk3Num;
    private float sk3FillCount = 0;
    private float sk3NumCount = 0;
    #endregion
    public void ClickSkill3Atk()
    {
        if (!isSk3CD && GetCanRlsSkill())
        {
            BattleSys.Instance.ReqReleaseSkill(3);
            isSk3CD = true;
            SetActive(imgSk3CD);
            imgSk3CD.fillAmount = 1;
            sk3Num = (int)sk3CDTime;
            SetText(txtSk3CD, sk3Num);
        }
    }

    // Test Reset Data
    public void ClickResetCfgs()
    {
        resSvc.ResetSkillCfgs();
    }

    public void ClickHeadBtn()
    {
        BattleSys.Instance.battleMgr.isPauseGame = true;
        BattleSys.Instance.SetBattleEndWndState(FBEndType.Pause);
    }

    private void Update()
    {
        //Test
        if (Input.GetKeyDown(KeyCode.A))
        {
            ClickNormalAtk();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ClickSkill1Atk();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ClickSkill2Atk();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ClickSkill3Atk();
        }

        float delta = Time.deltaTime;

        #region Skill CD
        if (isSk1CD)
        {
            sk1FillCount += delta;
            if (sk1FillCount >= sk1CDTime)
            {
                isSk1CD = false;
                SetActive(imgSk1CD, false);
                sk1FillCount = 0;
            }
            else
            {
                imgSk1CD.fillAmount = 1 - sk1FillCount / sk1CDTime;
            }
            sk1NumCount += delta;
            if (sk1NumCount >= 1)
            {
                sk1NumCount -= 1;
                sk1Num -= 1;
                SetText(txtSk1CD, sk1Num);
            }
        }

        if (isSk2CD)
        {
            sk2FillCount += delta;
            if (sk2FillCount >= sk2CDTime)
            {
                isSk2CD = false;
                SetActive(imgSk2CD, false);
                sk2FillCount = 0;
            }
            else
            {
                imgSk2CD.fillAmount = 1 - sk2FillCount / sk2CDTime;
            }
            sk2NumCount += delta;
            if (sk2NumCount >= 1)
            {
                sk2NumCount -= 1;
                sk2Num -= 1;
                SetText(txtSk2CD, sk2Num);
            }
        }

        if (isSk3CD)
        {
            sk3FillCount += delta;
            if (sk3FillCount >= sk3CDTime)
            {
                isSk3CD = false;
                SetActive(imgSk3CD, false);
                sk3FillCount = 0;
            }
            else
            {
                imgSk3CD.fillAmount = 1 - sk3FillCount / sk3CDTime;
            }
            sk3NumCount += delta;
            if (sk3NumCount >= 1)
            {
                sk3NumCount -= 1;
                sk3Num -= 1;
                SetText(txtSk3CD, sk3Num);
            }
        }
        #endregion

        if (transBossHPBar.gameObject.activeSelf)
        {
            BlendBossHP();
            imgYellow.fillAmount = currentPrg;
        }
    }

    public void SetSelfHpBarVal(int val)
    {
        SetText(txtSelfHP, val + "/" + hpSum);
        imgSelfHP.fillAmount = val * 1.0f / hpSum;
    }

    public bool GetCanRlsSkill()
    {
        return BattleSys.Instance.battleMgr.CanRlsSkill();
    }

    public Transform transBossHPBar;
    public Image imgRed;
    public Image imgYellow;
    private float currentPrg = 1f;
    private float targetPrg = 1f;

    public void SetBossHPBarVal(int oldVal, int newVal, int sumVal)
    {
        currentPrg = oldVal * 1.0f / sumVal;
        targetPrg = newVal * 1.0f / sumVal;
        imgRed.fillAmount = targetPrg;
    }

    private void BlendBossHP()
    {
        if (Mathf.Abs(currentPrg - targetPrg) < Constants.AccelerHPSpeed * Time.deltaTime)
        {
            currentPrg = targetPrg;
        }
        else if (currentPrg > targetPrg)
        {
            currentPrg -= Constants.AccelerHPSpeed * Time.deltaTime;
        }
        else
        {
            currentPrg += Constants.AccelerHPSpeed * Time.deltaTime;
        }
    }

    public void SetBossHPBarState(bool state, float prg = 1)
    {
        SetActive(transBossHPBar, state);
        imgRed.fillAmount = prg;
        imgYellow.fillAmount = prg;
    }
}
