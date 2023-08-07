using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Unit/UnitData")]

public class UnitInfo : ScriptableObject
{
    [SerializeField]
    private string unitName;
    [SerializeField]
    private UnitType unitType; // ĳ���� Ÿ�� 0: ���, 1: �ü�, 2: ������
    [SerializeField]
    private ElementType elementType; //�Ӽ� 0: ��, 1: �� 2: �� 3: �ٶ� 4: �� 5: ��� 6: ���  ---  ��<��<��<�ٶ�<��

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
    public UnitType UnitType { get { return unitType; } } // ĳ���� Ÿ�� 0: ���, 1: �ü�, 2: ������
    public ElementType ElementType { get { return elementType; } } //�Ӽ� 0: ��, 1: �� 2: �� 3: �ٶ� 4: �� 5: ��� 6: ���  ---  ��<��<��<�ٶ�<��

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
