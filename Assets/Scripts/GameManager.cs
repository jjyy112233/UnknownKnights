using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    List<UnitInfo> unitInfos = new List<UnitInfo>();


    public UnitInfo GetUnitInfo(string name)
    {
        var t = unitInfos.Find(t => t.UnitName.CompareTo(name) == 0);

        return t;
    }


    public void Start()
    {
    }
}
