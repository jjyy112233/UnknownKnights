using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    BaseUnit baseUnit;
    private void Awake()
    {
        baseUnit = transform.parent.GetComponent<BaseUnit>();
    }

    public void AttackAniEnd() => baseUnit.AttackAniEnd();
    public void SkillAniEnd() => baseUnit.SkillAniEnd();
    public void OnAttack() => baseUnit.OnAttack();
    public void OnSkill() => baseUnit.OnSkill();
}
