/// <summary>
/// 移动状态
/// </summary>
public class StateMove : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Move;
        //PECommon.Log("Enter StateMove");

    }

    public void Exit(EntityBase entity, params object[] args)
    {
        //PECommon.Log("Exit StateMove");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        //PECommon.Log("Process StateMove");
        entity.SetBlend(Constants.BlendMove);
    }
}

