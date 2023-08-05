using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class SkillEffect : MonoBehaviour, IEffectObject
{
    [SerializeField]
    protected IObjectPool<SkillEffect> effectPool;
    public List<BaseUnit> targetList;

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void SetPosition(Transform tr)
    {
        transform.position = tr.position;
    }
    public void SetPosition(BaseUnit unit)
    {
        transform.position = unit.transform.position;
    }    

    public abstract void DestroyEffect();

    //public void SetPool(IObjectPool<SkillEffect> pool)
    //{
    //    effectPool = pool;
    //}

    //private void OnBecameInvisible()
    //{
    //    effectPool.Release(this);
    //}
}
