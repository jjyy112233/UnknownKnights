using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TextCore.Text;

[Serializable]
public class BattleStartPosLine
{
    public Transform[] line;
}
public class BattleManager : Singleton<BattleManager>
{
    [SerializeField]
    BattleStartPosLine[] unitStartPositions; //[0: ��, 1: �߾�, 2: ��][0: ��, 1: �߾� 2: �Ʒ�]

    [SerializeField]
    Transform units;

    List<BaseUnit> unitList = new();
    Dictionary<Vector3Int, string> unitPos = new Dictionary<Vector3Int, string>();

    public void Init()
    {
        unitPos[new Vector3Int(0, 0)] = "UnitArcher";
        unitPos[new Vector3Int(1, 1)] = "UnitKnight";
        unitPos[new Vector3Int(2, 2)] = "UnitMagician";

        unitPos[new Vector3Int(3, 0)] = "Enemy0";
        unitPos[new Vector3Int(4, 1)] = "Enemy1";
        unitPos[new Vector3Int(5, 2)] = "Enemy2";
    }
    private void Start()
    {
        Init(); // �׽�Ʈ��
        foreach (var unit in unitPos)
        {
            SpawnUnit(unit.Value, unit.Key);
        }
    }
    public Vector3 GetStartPosition(int line, int pos)
    {
        return unitStartPositions[line].line[pos].position;
    }

    void SpawnUnit(string unitName, Vector3Int pos)
    {
        var op = Addressables.InstantiateAsync(unitName, Vector3.zero, Quaternion.identity, null, true);
        op.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            var unit = obj.Result.GetComponent<BaseUnit>();
            unitList.Add(unit);
            Debug.Log(unitName);
            Debug.Log(pos);
            Debug.Log(unit);
            Debug.Log(GameManager.Instance);
            unit.InitInfo(GameManager.Instance.GetUnitInfo(unitName), pos);
        };
    }

}