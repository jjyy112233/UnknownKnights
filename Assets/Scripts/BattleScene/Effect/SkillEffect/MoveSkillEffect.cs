using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSkillEffect : SkillEffect
{
    private BaseUnit target;
    private Vector3 initPos;
    private Vector3 targetPos;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float time;
    private float timer = 0;

    public void SetTarget(BaseUnit unit)
    {
        target = unit;
        targetPos = unit.transform.position + new Vector3(0,1f,0);

        Vector3 dir = targetPos - transform.position;


        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void MoveStart()
    {
        initPos = transform.position;
        StartCoroutine(MoveToTarget());
    }
    public IEnumerator MoveToTarget()
    {
        while (time >= timer && (Vector2.Distance(transform.position, targetPos) > 1f))
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            timer += Time.deltaTime;

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
