using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSkillEffect : SkillEffect
{
    protected BaseUnit myUnit;
    protected BaseUnit target;
    protected Vector3 initPos;
    protected Vector3 targetPos;

    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float time;
    protected float timer = 0;

    public virtual void SetTarget(BaseUnit my ,BaseUnit targetUnit)
    {
        myUnit = my;
        target = targetUnit;
        targetPos = targetUnit.transform.position+ new Vector3(0, 0.7f, 0);

        Vector3 dir = targetPos - transform.position;


        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void MoveStart()
    {
        initPos = transform.position;
        StartCoroutine(MoveToTarget());
    }
    public virtual IEnumerator MoveToTarget()
    {
        while (time >= timer)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            timer += Time.deltaTime;

            if (Vector2.Distance(transform.position, targetPos) < 1f)
            {
                target.OnDamage(myUnit, 10);
                break;
            }

            yield return null;
        }
        DestroyEffect();
    }

    public override void DestroyEffect()
    {
        Destroy(gameObject);
        //effectPool.Release(this);
    }
}
