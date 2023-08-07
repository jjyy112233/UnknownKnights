using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class BaseUnitInfo
{
    [SerializeField] string unitName;
    [SerializeField] UnitType unitType; // 캐릭터 타입 0: 기사, 1: 궁수, 2: 마법사
    [SerializeField] ElementType elementType; //속성 0: 불, 1: 물 2: 땅 3: 바람 4: 빛 5: 어둠 6: 기계  ---  불<물<땅<바람<불

    [SerializeField] int maxHp;
    [SerializeField] int hp;
    [SerializeField] int def;
    [SerializeField] int speed;

    [SerializeField] float attackCool;
    [SerializeField] float nowAttackCool;

    [SerializeField] float skillCool;
    [SerializeField] float nowSkillCool;

    [SerializeField] float attackDis;

    [SerializeField]
    private int attackDamage;

    [SerializeField]
    private int skillDamage;

    public int AttackDamage { get { return attackDamage; } set { attackDamage = value; } }
    public int SkillDamage { get { return skillDamage; } set { skillDamage = value; } }

    public string UnitName { get { return unitName; } set { unitName = value; } }
    public UnitType UnitType { get { return unitType; } set { unitType = value; } }
    public ElementType ElementType { get { return elementType; } set { elementType = value; } }
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    public int Hp { get { return hp; } set { hp = value; } }
    public int Def { get { return def; } set { def = value; } }
    public int Speed { get { return speed; } set { speed = value; } }

    public float AttackCool { get { return attackCool; } set { attackCool = value; } }
    public float NowAttackCool
    {
        get { return nowAttackCool; }
        set
        {
            nowAttackCool = Mathf.Min(AttackCool, value);
        }
    }
    public bool IsAttack => NowAttackCool >= AttackCool;
    public float SkillCool { get { return skillCool; } set { skillCool = value; } }
    public float NowSkillCool
    {
        get { return nowSkillCool; }
        set
        {
            nowSkillCool = Mathf.Min(SkillCool, value);
        }
    }
    public bool IsSkill => NowSkillCool >= SkillCool;

    public float AttackDis { get { return attackDis; } set { attackDis = value; } }

    public void AddDeltaTime(float delta)
    {
        NowAttackCool += delta;
        NowSkillCool += delta;

    }

    public bool IsDie => hp <= 0;
    public float GetHpRatio => (float)Hp / (float)MaxHp;

    public bool AddAttackCool(float delta)
    {
        NowAttackCool += delta;
        return AttackCool <= NowAttackCool;
    }

    public bool AddSkillCool(float delta, Action skillCoolButton)
    {
        NowSkillCool += delta;
        skillCoolButton();
        return SkillCool <= NowSkillCool;
    }

    public BaseUnitInfo(string name, UnitType uType, ElementType eleType, int maxHp, int def, 
        int speed, float atkCool, float sklCool, float attackDis, int attackDamage, int skillDamage)
    {
        this.UnitName = name;
        this.UnitType = uType;
        this.ElementType = eleType;
        this.Hp = this.maxHp = maxHp;
        this.Def = def;
        this.Speed = speed;
        this.AttackCool = atkCool;
        this.SkillCool = sklCool;
        this.AttackDis = attackDis;

        this.AttackDamage = attackDamage;
        this.SkillDamage = skillDamage;

        this.NowAttackCool = this.NowSkillCool = 0f;
    }
}

public class BaseUnit : MonoBehaviour, IAttackable
{
    BattleManager battleManager;
    [SerializeField]
    protected BaseUnitInfo info;
    protected List<BaseUnit> teamUnits;
    protected List<BaseUnit> enemyUnits;

    protected Animator animator;

    [SerializeField]
    private int startLine; 
    [SerializeField]
    private int startLineNum;
    private UnitTeamType unitTeamType;
    public UnitTeamType UnitTeamType  { get { return unitTeamType; } }

    public int GetLine => startLine;
    public int GetLineNum => startLineNum;

    private Vector3 movePos;
    private float battleWaitSpeed = 3f;

    protected BaseUnit battleTarget;
    private Action<float> NowUpadte;
    private Action FindTarget;
    protected void AddFindTarget(Action evnet) => FindTarget += evnet;

    private Action AttackEnd;
    private Action SkillEnd;

    private UnitAnimation unitAnimation;
    private HpBar unitHpBar;
    public void AttackAniEnd() => AttackEnd();
    public void SkillAniEnd() => SkillEnd();
    public int SkillDamage => info.SkillDamage;


    private bool auto = false;
    public bool Auto
    {
        get
        {
            return auto;
        }
        set
        {
            auto = value;
        }
    }
    public bool IsDie => info.IsDie;
    public float GetHp => info.Hp;
    public float GetMaxHp => info.MaxHp;
    public float SkillCool => info.SkillCool;
    public float NowSkillCool => info.NowSkillCool;
    private Action skillCoolButton;
    public Action SkillCoolButton { get { return skillCoolButton; } set { skillCoolButton = value; } }

    private Action setDieButton;
    public Action SetDieButton { get { return setDieButton; } set { setDieButton = value; } }

    private Action<float> setButtonHp;
    public Action<float> SetButtonHp { get { return setButtonHp; } set { setButtonHp = value; } }

    public string UnitName => info.UnitName;

    #region 상태패턴
    [SerializeField]
    private UnitState unitState;
    protected UnitState UnitState
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
                    SetStartTarget();
                    info.NowAttackCool = info.AttackCool; 


                    break;
                case UnitState.Count:
                    break;
                default:
                    break;
            }

            unitState = value;
        }
    }

    [SerializeField]
    private BattleState battleState;
    protected BattleState BattleState
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
                    animator.SetFloat("Speed", 0);
                    break;
                case BattleState.MoveToTarget:
                    animator.SetFloat("Speed", info.Speed);
                    break;
                case BattleState.Attack:
                    info.NowAttackCool = 0f;
                    animator.SetTrigger("Attack");
                    break;
                case BattleState.Skill:
                    info.NowSkillCool = 0f;
                    animator.SetTrigger("Skill");
                    break;
                case BattleState.Die:
                    info.Hp = 0;
                    animator.SetFloat("Speed", 0);
                    animator.SetTrigger("Die");
                    battleManager.DeadUnit(this);
                    unitHpBar.gameObject.SetActive(false);
                    if(UnitTeamType == UnitTeamType.Player)
                       SetDieButton();
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
        battleManager = GameObject.FindAnyObjectByType<BattleManager>();
    }

    public void BattleStart()
    {
        teamUnits = unitTeamType == UnitTeamType.Player ? battleManager.PlayerUnitList : battleManager.EnemyUnitList;
        enemyUnits = unitTeamType == UnitTeamType.Player ? battleManager.EnemyUnitList : battleManager.PlayerUnitList;

        UnitState = UnitState.Battle;
    }
    public void BattleEnd()
    {
        UnitState = UnitState.Idle;
    }


    public virtual void InitInfo(UnitInfo data,Vector3Int posIdx, Vector3 pos, UnitTeamType teamType)
    {
        info = new BaseUnitInfo(data.UnitName, data.UnitType, data.ElementType, data.MaxHp,
            data.Def, data.Speed, data.AttackCool, data.SkillCool, data.AttackDis, data.AttackDamage, data.SkillDamage);

        transform.localScale = new Vector3(teamType == UnitTeamType.Player  ? -1 : 1, 1, 1);
        movePos = pos;
        transform.position = (movePos + transform.right * 3 * transform.localScale.x);
        NowUpadte = null;
        UnitState = UnitState.Idle;
        startLine = posIdx.x;
        startLineNum = posIdx.y;

        unitTeamType = teamType;

        unitAnimation = transform.GetChild(0).AddComponent<UnitAnimation>();
        unitHpBar = transform.GetComponentInChildren<HpBar>();
        unitHpBar.SetColor(UnitTeamType);

        AttackEnd += delegate
        {
            BattleState = BattleState.BattleIdle;
        };
        SkillEnd += delegate
        {
            BattleState = BattleState.BattleIdle;
        };
    }


    #region Attackable interface
    public virtual void OnAttack() //타겟을 공격
    {
        battleTarget.OnDamage(this, info.AttackDamage);
    }

    public virtual void OnDamage(BaseUnit target, int damage) //피격
    {
        info.Hp -= damage;
        unitHpBar.MoveValue = info.GetHpRatio;
        if (IsDie)
        {
            BattleState = BattleState.Die;
        }
        battleManager.SetHpBar(unitTeamType);
        if(UnitTeamType == UnitTeamType.Player)
            SetButtonHp(info.GetHpRatio);
    }
    public virtual void OnSkill()
    {
        battleTarget.OnDamage(this, info.SkillDamage);
    }
    public virtual void OnDead()
    {

    }

    #endregion


    private void Update()
    {
        if (NowUpadte != null)
            NowUpadte(Time.deltaTime);
    }
    protected void IdleUpdate(float delta)
    {

    }
    protected void MoveUpdate(float delta)
    {
        transform.position = Vector3.MoveTowards(transform.position, movePos, battleWaitSpeed * delta);
        if(Vector2.Distance(transform.position, movePos) < 0.1f)
        {
            transform.position = movePos;
            UnitState = UnitState.Idle;
            battleManager.UnitReadySucces();
        }

    }
    protected void BattleUpdate(float delta)
    {
        info.AddDeltaTime(delta);
        if(UnitTeamType == UnitTeamType.Player)
            SkillCoolButton();

        switch (BattleState)
        {
            case BattleState.None:
                break;
            case BattleState.BattleIdle:

                if(battleTarget.IsDie && FindTarget != null)
                    FindTarget();
                else 
                {
                    if (IsTargetAround())
                    {
                        if ((Auto || UnitTeamType == UnitTeamType.Enemy) && info.IsSkill)
                            BattleState = BattleState.Skill;
                        else if (info.IsAttack)
                            BattleState = BattleState.Attack;
                    }
                    else
                        BattleState = BattleState.MoveToTarget;
                }

                break;
            case BattleState.MoveToTarget:

                if (battleTarget.IsDie)
                    FindTarget();
                else
                {
                    if (IsTargetAround())
                        BattleState = BattleState.BattleIdle;
                    else
                    {
                        transform.localScale = new Vector3(battleTarget.transform.position.x > transform.position.x ? -1 : 1, 1, 1);
                        transform.position = Vector3.MoveTowards(transform.position, battleTarget.transform.position, info.Speed * delta);
                    }
                }

                break;
            case BattleState.Attack:
                break;
            case BattleState.Skill:
                break;
            case BattleState.Die:
                break;
            case BattleState.Count:
                break;
            default:
                break;
        }

    }

    #region Targeting
    protected bool IsTargetAround()
    {
        var dis = Vector2.Distance(transform.position, battleTarget.transform.position);

        return dis <= info.AttackDis;
    }
    protected void SetMinDistanceTarget()
    {
        var min = float.MaxValue;
        BaseUnit nowMinUnit = null;

        foreach (var unit in enemyUnits)
        {
            if (unit.IsDie)
                continue;

            var dis = Vector2.Distance(unit.transform.position, transform.position);
            if (dis < min)
            {
                min = dis;
                nowMinUnit = unit;
            }
        }

        battleTarget = nowMinUnit;
    }
    protected void SetMaxDistanceTarget()
    {
        var max = 0f;
        BaseUnit nowMaxUnit = null;

        foreach (var unit in enemyUnits)
        {
            var dis = Vector2.Distance(unit.transform.position, transform.position);
            if (dis > max)
            {
                max = dis;
                nowMaxUnit = unit;
            }
        }

        battleTarget = nowMaxUnit;
    }

    protected void SetStartTarget()
    {
        List<BaseUnit> targetLine = null;
        var frontSide = enemyUnits.Where(t => t.GetLine == 0).ToList();
        if (frontSide.Count != 0)  targetLine = frontSide;
        else
        {
            var center = enemyUnits.Where(t => t.GetLine == 1).ToList();
            if (center.Count != 0)  targetLine = center;
            else
            {
                var backSide = enemyUnits.Where(t => t.GetLine == 2).ToList();
                if (backSide.Count != 0)  targetLine = backSide;
            }
        }

        switch (targetLine.Count)
        {
            case 1:
                battleTarget = targetLine[0];
                break;
            case 2:
                if(GetLineNum == 2)
                {
                    battleTarget = targetLine.Find(t => t.GetLineNum == 1);
                }
                else
                {
                    battleTarget = targetLine.Find(t => t.GetLineNum == 0);
                }
                break;
            case 3:
                battleTarget = targetLine.Find(t=>t.GetLineNum == 1);
                break;
            default:
                break;
        }

        Debug.Log($"{info.UnitName} -> {battleTarget.info.UnitName}");

    }

    #endregion

    public void WaitBattle()
    {
        UnitState = UnitState.Move;
    }
    public void OnClickSkill()
    {
        if(info.IsSkill)
            BattleState = BattleState.Skill;
    }
    private void OnDestroy()
    {
        Addressables.ReleaseInstance(gameObject);
    }

}
