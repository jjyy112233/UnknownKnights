using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Line
{
    public List<string> lines;
}
public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private List<UnitInfo> unitInfos = new List<UnitInfo>(); //모든 캐릭터 정보

    [SerializeField]
    private List<Line> playerUnitField;
    public List<Line> PlayerUnitField { get { return playerUnitField; } }

    [SerializeField]
    private List<Line> enemyUnitField;
    public List<Line> EnemyUnitField { get { return enemyUnitField; } }


    public UnitInfo GetUnitInfo(string name)
    {
        var t = unitInfos.Find(t => t.UnitName.CompareTo(name) == 0);

        return t;
    }


    public void Start()
    {
    }
}
