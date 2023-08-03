using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TextCore.Text;

[Serializable]
public class BattleStartPosLine
{
    public Transform[] line;
}
public class BattleManager : Singleton<BattleManager>
{
    public AssetReference assetLabel;
    private IList<IResourceLocator> _locations;

    [SerializeField]
    BattleStartPosLine[] unitStartPositions; //[0: 앞, 1: 중앙, 2: 뒤][0: 위, 1: 중앙 2: 아래]

    [SerializeField]
    BattleMask battleMask;

    List<BaseUnit> unitList = new();
    Dictionary<Vector3Int, string> unitPos = new Dictionary<Vector3Int, string>();
    public Vector3 GetStartPosition(int line, int pos) => unitStartPositions[line].line[pos].position;

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
        Init(); // 테스트용
        foreach (var unit in unitPos)
        {
            LoadUnit(unit.Value, unit.Key);
        }
    }



    void LoadUnit(string unitName, Vector3Int pos)
    {
        var op = Addressables.InstantiateAsync(unitName, Vector3.zero, Quaternion.identity, null, true);
        op.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            var unit = obj.Result.GetComponent<BaseUnit>();
            unitList.Add(unit);
            Debug.Log("Load End");
            unit.InitInfo(GameManager.Instance.GetUnitInfo(unitName), pos);
        };
    }

}
