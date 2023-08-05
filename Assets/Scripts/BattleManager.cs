using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public class BattleStartPosLine
{
    public Transform[] line;
}
public class BattleManager : Singleton<BattleManager>
{

    [SerializeField]
    private BattleStartPosLine[] unitStartPositions; //[0: 앞, 1: 중앙, 2: 뒤][0: 위, 2: 아래]

    [SerializeField]
    private BattleMask battleMask;

    private List<BaseUnit> unitList = new();

    private List<BaseUnit> playerUnitList;
    public List<BaseUnit> PlayerUnitList => playerUnitList;

    private List<BaseUnit> enemyUnitList;
    public List<BaseUnit> EnemyUnitList => enemyUnitList;

    public Vector3 GetStartPosition(int line, int idx) => unitStartPositions[line].line[idx].position;

    private int LoadUnitCount;
    private int readyCount;

    private void Start()
    {
        battleMask.gameObject.SetActive(false);
        LoadUnits();
    }

    public void LoadUnits()
    {
        var playerUnits = GameManager.Instance.PlayerUnitField;
        var enemyUnits = GameManager.Instance.EnemyUnitField;

        foreach (var units in playerUnits) foreach (var unit in units.lines) LoadUnitCount++;
        foreach (var units in enemyUnits) foreach (var unit in units.lines) LoadUnitCount++;

        for (int i = 0; i < playerUnits.Count; i++)
        {
            for (int j = 0; j < playerUnits[i].lines.Count; j++)
            {
                var pos = GetPos(i, playerUnits[i].lines.Count, j);
                LoadUnit(playerUnits[i].lines[j], new Vector3Int(i, j), pos, UnitTeamType.Player);
            }
        }

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            for (int j = 0; j < enemyUnits[i].lines.Count; j++)
            {
                var pos = GetPos(i + 3, enemyUnits[i].lines.Count, j);
                LoadUnit(enemyUnits[i].lines[j], new Vector3Int(i, j), pos, UnitTeamType.Enemy);
            }
        }
    }
    Vector3 GetPos(int lintNum, int lineCount, int nowIdx)
    {
        if (lineCount == 3)
        {
            return Vector2.Lerp(GetStartPosition(lintNum, 0), GetStartPosition(lintNum, 1), (nowIdx) * 0.5f);
        }
        else if(lineCount == 2)
        {
            return Vector2.Lerp(GetStartPosition(lintNum, 0), GetStartPosition(lintNum, 1), 0.33f * (nowIdx + 1));
        }
        else if(lineCount == 1)
        {
            return Vector2.Lerp(GetStartPosition(lintNum, 0), GetStartPosition(lintNum, 1), 0.5f);
        }

        Debug.LogError("NotUnit");
        return Vector3.zero;
    }
    private void LoadUnit(string unitName,Vector3Int posIdx, Vector3 pos, UnitTeamType teamType)
    {
        var op = Addressables.InstantiateAsync(unitName, Vector3.zero, Quaternion.identity, null, true);
        op.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            var info = GameManager.Instance.GetUnitInfo(unitName);
            var unit = obj.Result.GetComponent<BaseUnit>();
            unitList.Add(unit);
            Debug.Log("Load End");
            unit.InitInfo(info, posIdx, pos, teamType);
            LoadUnitCount--;

            if (LoadUnitCount == 0)
                UnitLoadEnd();
        };
    }

    private void UnitLoadEnd()
    {
        Debug.Log("UnitLoadEnd");
        playerUnitList = unitList.FindAll(t => t.UnitTeamType == UnitTeamType.Player);
        enemyUnitList = unitList.FindAll(t => t.UnitTeamType == UnitTeamType.Enemy);

        battleMask.gameObject.SetActive(true);
        StartCoroutine(battleMask.FadeIn());

        foreach (var unit in unitList)
        {
            unit.WaitBattle();
        }
    }

    public void UnitReadySucces()
    {
        readyCount++;
        if(readyCount == unitList.Count)
        {
            Debug.Log("BattleStart");
            foreach (var unit in unitList)
            {
                unit.BattleStart();
            }
        }

    }

}
