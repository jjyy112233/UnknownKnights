using System;

public enum UnitType // ĳ���� Ÿ�� 0: ���, 1: �ü�, 2: ������
{
    None = -1, Knight, Archer, Magcian, Count
}

//�� pyro �� hydro ���� cryo ���� electro �ٶ� anemo ���� geo
public enum ElementType    //�Ӽ� 0: ��, 1: �� 2: �� 3: �ٶ� 4: �� 5: ��� 6: ���  ---  ��<��<��<�ٶ�<��
{
    None = -1, Fire, Water, Geo, Wind, Light, Dark, Machine, Count
}

public enum UnitState
{
    None = -1, Idle, Move, Battle, Die, Count
}
public enum BattleState
{
    None = -1, BattleIdle, MoveToTarget, NormalAttack, ActiveSkill, Die, Count
}