using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void OnAttack();
    void OnSkill();
    void OnDamage(BaseUnit target, int damage);
    void OnDead();
    
}
