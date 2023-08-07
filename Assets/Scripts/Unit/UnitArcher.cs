using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitArcher : BaseUnit
{
    public MoveSkillEffect arrowPref;
    public ArrowRain arrowRainPref;
    public override void InitInfo(UnitInfo data, Vector3Int posIdx, Vector3 pos, UnitTeamType teamType)
    {
        base.InitInfo(data, posIdx, pos, teamType);
        AddFindTarget(SetMinDistanceTarget);
    }
    public override void OnAttack()
    {
        //MoveSkillEffect arrow = (MoveSkillEffect)(EffectManager.Instance.Pool(101).Get());
        var arrow = Instantiate(arrowPref);

        arrow.SetPosition(transform.position + new Vector3(0, 0.7f, 0));
        arrow.SetTarget(this, battleTarget);
        arrow.MoveStart();

        battleTarget.OnDamage(this, info.AttackDamage);
    }
    public override void OnSkill()
    {
        //MoveSkillEffect arrow = (MoveSkillEffect)(EffectManager.Instance.Pool(101).Get());
        var arrowRain = Instantiate(arrowRainPref);

        arrowRain.SetPosition(battleTarget.transform.position);
        arrowRain.SetTarget(this, battleTarget);

        battleTarget.OnDamage(this, info.SkillDamage);
    }
}
