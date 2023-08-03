using System;
using System.Collections.Generic;
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
    BattleStartPosLine[] unitStartPositions; //[0: ¾Õ, 1: Áß¾Ó, 2: µÚ][0: À§, 1: Áß¾Ó 2: ¾Æ·¡]

    [SerializeField]
    BattleMask battleMask;

    List<BaseUnit> unitList = new();
    public Vector3 GetStartPosition(int line, int pos) => unitStartPositions[line].line[pos].position;

    private int LoadUnitCount;
    private int readyCount;

    private void Start()
    {
        battleMask.gameObject.SetActive(false);
        LoadUnitDatas();
        LoadUnitData();
    }

    Dictionary<string, Vector3Int> loadUnitPos = new Dictionary<string, Vector3Int>();
    public void LoadUnitDatas()
    {
        loadUnitPos["UnitArcher"] = new Vector3Int(0, 0);
        loadUnitPos["UnitKnight"] = new Vector3Int(1, 1);
        loadUnitPos["UnitMagician"] = new Vector3Int(2, 2);
        loadUnitPos["Enemy0"] = new Vector3Int(3, 0);
        loadUnitPos["Enemy1"] = new Vector3Int(4, 1);
        loadUnitPos["Enemy2"] = new Vector3Int(5, 2);
    }
    public void LoadUnitData()
    {
        LoadUnitCount = loadUnitPos.Count;
        foreach (var unit in loadUnitPos)
        {
            LoadUnit(unit.Key, unit.Value);
        }
    }
    private void LoadUnit(string unitName, Vector3Int pos)
    {
        var op = Addressables.InstantiateAsync(unitName, Vector3.zero, Quaternion.identity, null, true);
        op.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            var unit = obj.Result.GetComponent<BaseUnit>();
            unitList.Add(unit);
            Debug.Log("Load End");
            unit.InitInfo(GameManager.Instance.GetUnitInfo(unitName), pos);
            LoadUnitCount--;

            if (LoadUnitCount == 0)
                UnitLoadEnd();
        };
    }

    private void UnitLoadEnd()
    {
        battleMask.gameObject.SetActive(true);
        StartCoroutine(battleMask.FadeIn());
        Debug.Log("UnitLoadEnd");

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
        }
    }
}
