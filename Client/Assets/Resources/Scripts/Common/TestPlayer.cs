using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : Controller
{
    public GameObject daggerskill1fx;

    public CharacterController ctrl;

    private Transform camTrans;
    private Vector3 camOffset;

    private float targetBlend;
    private float currentBlend;


    private void Start()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;
    }

    private void Update()
    {

        #region Input

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 _dir = new Vector2(h, v).normalized;
        if (_dir != Vector2.zero)
        {
            Dir = _dir;
            SetBlend(Constants.BlendMove);
        }
        else
        {
            Dir = Vector2.zero;
            SetBlend(Constants.BlendIdle);
        }

        #endregion

        if (currentBlend != targetBlend)
            UpdateMixBlend();

        if (isMove)
        {
            // 设置方向
            SetDir();
            // 产生移动
            SetMove();
            // 相机跟随
            SetCam();
        }
    }

    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    public void SetCam()
    {
        if (camTrans != null)
        {
            camTrans.position = transform.position - camOffset;
        }
    }

    public override void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend)
        {
            currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += Constants.AccelerSpeed * Time.deltaTime;
        }
        ani.SetFloat("Blend", currentBlend);
    }


    public void ClickSkill1Btn()
    {
        ani.SetInteger("Action", 1);
        daggerskill1fx.SetActive(true);
        StartCoroutine(Dealy());
    }

    IEnumerator Dealy()
    {
        yield return new WaitForSeconds(0.993f);
        ani.SetInteger("Action", -1);
        daggerskill1fx.SetActive(false);
    }
}
