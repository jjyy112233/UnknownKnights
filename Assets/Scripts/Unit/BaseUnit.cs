using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BaseUnitInfo
{
    string unitName;
    UnitType unitType; // 캐릭터 타입 0: 기사, 1: 궁수, 2: 마법사
    ElementType elementType; //속성 0: 불, 1: 물 2: 땅 3: 바람 4: 빛 5: 어둠 6: 기계  ---  불<물<땅<바람<불

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
        this.hp = this.maxHp = maxHp;
        this.def = def;
        this.speed = speed;
        this.attackCool = atkCool;
        this.skillCool = sklCool;

        nowAtkCool = nowSkillCool = 0f;
    }
}

public class BaseUnit : MonoBehaviour, IAttackable
{
    private BaseUnitInfo info;
    private Animator animator;

    #region Ai
    private Vector3 movePos;
    private float battleWaitSpeed = 3f;
    private BaseUnit battleTarget;
    Action NowUpadte;

    private UnitState unitState;
    public UnitState UnitState
    {
        get
        {
            return unitState;
        }
        set
        {
            if (value == unitState)
                return;

            switch (value)
            {
                case UnitState.None:
                    break;
                case UnitState.Idle:
                    NowUpadte = IdleUpdate;
                    animator.SetFloat("Speed", 0);
                    break;
                case UnitState.Move:
                    NowUpadte = MoveUpdate;
                    animator.SetFloat("Speed", info.Speed);
                    break;
                case UnitState.Battle:
                    NowUpadte = BattleUpdate;
                    BattleState = BattleState.BattleIdle;
                    break;
                case UnitState.Die:
                    animator.SetFloat("Speed", 0);
                    break;
                case UnitState.Count:
                    break;
                default:
                    break;
            }

            unitState = value;
        }
    }

    private BattleState battleState;
    public BattleState BattleState
    {
        get
        {
            return battleState;
        }
        set
        {
            if (value == battleState)
                return;

            switch (value)
            {
                case BattleState.None:
                    break;
                case BattleState.BattleIdle:
                    break;
                case BattleState.MoveToTarget:
                    break;
                case BattleState.NormalAttack:
                    break;
                case BattleState.ActiveSkill:
                    break;
                case BattleState.Die:
                    break;
                case BattleState.Count:
                    break;
                default:
                    break;
            }

            battleState = value;
        }
    }
    #endregion

    private void Awake()
    {
        animator = transform.GetComponentInChildren<Animator>();
    }

    public virtual void InitInfo(UnitInfo data, Vector3Int pos)
    {
        info = new BaseUnitInfo(data.UnitName, data.UnitType, data.ElementType, data.MaxHp, data.Def, data.Speed, data.AttackCool, data.SkillCool);
        transform.localScale = new Vector3(pos.x >= 3 ? 1 : -1, 1, 1);
        movePos = BattleManager.Instance.GetStartPosition(pos.x, pos.y);
        transform.position = (movePos + transform.right * 3 * transform.localScale.x);
        NowUpadte = null;
        UnitState = UnitState.Idle;
    }


    #region Attackable interface
    public void OnAttack(BaseUnit target, int damage) //타겟을 공격
    {
        info.NowSkillCool = 0f;
        target.OnDamage(this, damage);
    }

    public void OnDamage(BaseUnit target, int damage) //피격
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


    private void Update()
    {
        if (NowUpadte != null)
            NowUpadte();
    }
    public void IdleUpdate()
    {

    }
    public void MoveUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePos, battleWaitSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, movePos) < 0.1f)
        {
            transform.position = movePos;
            UnitState = UnitState.Idle;
            BattleManager.Instance.UnitReadySucces();
        }

    }
    public void BattleUpdate()
    {

    }

    private void OnDestroy()
    {
        Addressables.ReleaseInstance(gameObject);
    }

    public void WaitBattle()
    {
        UnitState = UnitState.Move;
    }
}
