using System;

public enum UnitType // 캐릭터 타입 0: 기사, 1: 궁수, 2: 마법사
{
    None = -1, Knight, Archer, Magcian, Count
}

//불 pyro 물 hydro 얼음 cryo 번개 electro 바람 anemo 바위 geo
public enum ElementType    //속성 0: 불, 1: 물 2: 땅 3: 바람 4: 빛 5: 어둠 6: 기계  ---  불<물<땅<바람<불
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