using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMagician : BaseUnit
{
    public override void InitInfo(UnitInfo data, Vector3Int posIdx, Vector3 pos, UnitTeamType teamType)
    {
        base.InitInfo(data, posIdx, pos, teamType);
        AddFindTarget(SetMinDistanceTarget);
    }
}
