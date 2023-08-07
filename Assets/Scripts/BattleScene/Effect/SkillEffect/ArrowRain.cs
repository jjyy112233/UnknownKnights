using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRain : SkillEffect
{
    protected BaseUnit myUnit;
    protected BaseUnit target;

    [SerializeField]
    Animator animator;

    public virtual void SetTarget(BaseUnit my, BaseUnit targetUnit)
    {
        myUnit = my;
        target = targetUnit;
    }
    
    public void SetDamage()
    {
        target.OnDamage(target, 10);
    }

    public override void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
