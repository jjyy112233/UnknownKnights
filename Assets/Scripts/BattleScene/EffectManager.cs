using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;


[Serializable]
public struct EffectData
{
    [SerializeField]
    private int id;
    public int Id => id;

    [SerializeField]
    private SkillEffect effect;
    public SkillEffect Effect => effect;
    public int maxSize;

}

public class EffectManager : Singleton<EffectManager>
{
    //[SerializeField]
    //private List<EffectData> effectDatas;

    //private Dictionary<int, SkillEffect> effects = new Dictionary<int, SkillEffect>();
    //private Dictionary<int, IObjectPool<SkillEffect>> effectPools = new Dictionary<int, IObjectPool<SkillEffect>>();
    //public IObjectPool<SkillEffect> Pool(int id) => effectPools[id];

    //private Func<SkillEffect> test;

    //[ContextMenu("init")]
    //public void Init()
    //{
    //    foreach (var data in effectDatas)
    //    {
    //        test = () => CreateEffect(data.Id);
    //        effectPools[data.Id] = new ObjectPool<SkillEffect>
    //            (
    //            test,
    //            OnGet,
    //            OnRelease,
    //            OnDestroyEffect,
    //            maxSize: data.maxSize
    //            );

    //        effects[data.Id] = data.Effect;
    //    }
    //}


    //private SkillEffect CreateEffect(int id)
    //{
    //    SkillEffect effect = Instantiate(effects[id]);
    //    effect.SetPool(effectPools[id]);
    //    return effect;
    //}

    //private void OnGet(SkillEffect effect)
    //{
    //    effect.gameObject.SetActive(true);
    //}
    //private void OnRelease(SkillEffect effect)
    //{
    //    effect.gameObject.SetActive(false);
    //}
    //private void OnDestroyEffect(SkillEffect effect) 
    //{
    //    Destroy(effect.gameObject);
    //}

    //private void LoadUnit(int id)
    //{
    //    var op = Addressables.InstantiateAsync(id, Vector3.zero, Quaternion.identity, null, true);
    //    op.Completed += (AsyncOperationHandle<GameObject> obj) =>
    //    {
    //        var unit = obj.Result.GetComponent<BaseUnit>();
    //    };
    //}
}
