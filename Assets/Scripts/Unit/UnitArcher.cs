using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitArcher : BaseUnit
{
    public MoveSkillEffect arrowPref;
    public override void InitInfo(UnitInfo data, Vector3Int posIdx, Vector3 pos, UnitTeamType teamType)
    {
        base.InitInfo(data, posIdx, pos, teamType);
        AddFindTarget(SetMinDistanceTarget);
    }
    public override void OnAttack()
    {
        //MoveSkillEffect arrow = (MoveSkillEffect)(EffectManager.Instance.Pool(101).Get());
        var arrow = Instantiate(arrowPref);

        arrow.SetPosition(transform.position + new Vector3(0, 1, 0));
        arrow.SetTarget(battleTarget);
        arrow.MoveStart();

        //battleTarget.OnDamage(this, 10);
        Debug.Log("Knight Attack");
    }
}
