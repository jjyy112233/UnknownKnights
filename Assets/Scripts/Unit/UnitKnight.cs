using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitKnight : BaseUnit
{
    public override void InitInfo(UnitInfo data, Vector3Int posIdx, Vector3 pos, UnitTeamType teamType)
    {
        base.InitInfo(data, posIdx, pos, teamType);
        AddFindTarget(SetMinDistanceTarget);
    }

    public override void OnAttack()
    {
        battleTarget.OnDamage(this, 10);
        Debug.Log("Knight Attack");
    }
}
