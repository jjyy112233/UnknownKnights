using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Unit/UnitData")]

public class UnitInfo : ScriptableObject
{
    [SerializeField]
    private string unitName;
    [SerializeField]
    private UnitType unitType; // 캐릭터 타입 0: 기사, 1: 궁수, 2: 마법사
    [SerializeField]
    private ElementType elementType; //속성 0: 불, 1: 물 2: 땅 3: 바람 4: 빛 5: 어둠 6: 기계  ---  불<물<땅<바람<불

    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int def;
    [SerializeField]
    private int speed;

    [SerializeField]
    private float attackCool;

    [SerializeField]
    private float skillCool;

    [SerializeField]
    private float attackDis;

    [SerializeField]
    private List<int> effectList;

    public string UnitName { get { return unitName; } }
    public UnitType UnitType { get { return unitType; } } // 캐릭터 타입 0: 기사, 1: 궁수, 2: 마법사
    public ElementType ElementType { get { return elementType; } } //속성 0: 불, 1: 물 2: 땅 3: 바람 4: 빛 5: 어둠 6: 기계  ---  불<물<땅<바람<불

    public int MaxHp { get { return maxHp; } }
    public int Def { get { return def; } }
    public int Speed { get { return speed; } }

    public float AttackCool { get { return attackCool; } }  

    public float SkillCool { get { return skillCool; } }

    public float AttackDis { get { return attackDis; } }

    [SerializeField]
    private int attackDamage;

    [SerializeField]
    private int skillDamage;

    public int AttackDamage { get { return attackDamage; } }
    public int SkillDamage { get { return skillDamage; } }
    public List<int> EffectList { get { return effectList; } }
}
