using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void OnAttack(BaseUnit target, int damage);
    void OnSkill(BaseUnit target, int damage);
    void OnDamage(BaseUnit target, int damage);
    void OnDead();
    
}
