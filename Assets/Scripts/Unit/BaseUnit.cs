using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BaseUnitInfo
{
    string unitName;
    UnitType unitType; // ĳ���� Ÿ�� 0: ���, 1: �ü�, 2: ������
    ElementType elementType; //�Ӽ� 0: ��, 1: �� 2: �� 3: �ٶ� 4: �� 5: ��� 6: ���  ---  ��<��<��<�ٶ�<��

    int maxHp;
    int hp;
    int def;
    int speed;

    float attackCool;
    float nowAtkCool;

    float skillCool;
    float nowSkillCool;

    public string UnitName { get { return unitName; } set { unitName = value; } }
    public UnitType UnitType { get { return unitType; } set { unitType = value; } }
    public ElementType ElementType { get { return elementType; } set { elementType = value; } }
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    public int Hp { get { return hp; } set { hp = value; } }
    public int Def { get { return def; } set { def = value; } }
    public int Speed { get { return speed; } set { speed = value; } }

    public float AttackCool { get { return attackCool; } set { attackCool = value; } }
    public float NowAttackCool { get { return nowAtkCool; } set { nowAtkCool = value; } }
    public float SkillCool { get { return skillCool; } set { skillCool = value; } }
    public float NowSkillCool { get { return nowSkillCool; } set { nowSkillCool = value; } }

    public bool IsDie() => hp <= 0;
    public float GetHpRatio => (float)Hp / (float)MaxHp;

    public bool AddAttackCool(float delta)
    {
        NowAttackCool += delta;
        return AttackCool <= NowAttackCool;
    }

    public bool AddSkillCool(float delta)
    {
        NowSkillCool += delta;
        return SkillCool <= NowSkillCool;
    }

    public BaseUnitInfo(string name, UnitType uType, ElementType eleType, int maxHp, int def, int speed, float atkCool, float sklCool)
    {
        this.unitName = name;
        this.unitType = uType;
        this.elementType = eleType;
        this.hp =  this.maxHp = maxHp;
        this.def = def;
        this.speed = speed;
        this.attackCool = atkCool;
        this.skillCool = sklCool;

        nowAtkCool = nowSkillCool = 0f;
    }
    public void Init()
    {
        Hp = MaxHp;
        NowAttackCool = NowSkillCool = 0f;
    }
}

public class BaseUnit : MonoBehaviour, IAttackable
{
    BaseUnitInfo info;
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void InitPosition(int line, int pos)
    {
        transform.position =  BattleManager.Instance.GetStartPosition(line, pos);
    }

    public virtual void InitInfo(UnitInfo data, Vector3Int pos)
    {
        Debug.Log(data);
        info = new BaseUnitInfo(data.UnitName, data.UnitType, data.ElementType, data.MaxHp, data.Def, data.Speed, data.AttackCool, data.SkillCool);
        InitPosition(pos.x, pos.y);
        transform.localScale = new Vector3(pos.x >= 3 ? 1 : -1, 1, 1);
    }

    #region Attackable interface
    public void OnAttack(BaseUnit target, int damage) //Ÿ���� ����
    {
        info.NowSkillCool = 0f;
        target.OnDamage(this, damage);
    }

    public void OnDamage(BaseUnit target, int damage) //�ǰ�
    {
        info.Hp -= damage;
    }

    public void OnDead()
    {

    }

    public void OnSkill(BaseUnit target, int damage)
    {
        info.NowSkillCool = 0f;
    }
    #endregion


    private void OnDestroy()
    {
        Addressables.ReleaseInstance(gameObject);
    }

}
