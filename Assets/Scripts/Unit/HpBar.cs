using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    SpriteRenderer backSprite;
    SpriteRenderer hpSprite;

    [SerializeField]
    float moveValue = 1f;
    float nowScale = 1f;
    public float MoveValue
    {
        set
        {
            moveValue = value;
        }
    }

    private void Awake()
    {
        backSprite = GetComponent<SpriteRenderer>();
        hpSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    public void SetColor(UnitTeamType type) =>
        hpSprite.color = type == UnitTeamType.Player ? Color.green : Color.red;

    private void Start()
    {
        StartCoroutine(MoveHp());
    }

    IEnumerator MoveHp()
    {
        while(true)
        {
            if (nowScale <= 0)
            {
                nowScale = 0f;
                break;
            }

            var max = Mathf.Max(nowScale, moveValue);
            var min = Mathf.Min(nowScale, moveValue);

            nowScale = Mathf.Lerp(max, min, Time.deltaTime * 2f);
            hpSprite.transform.localScale = new Vector3(nowScale, 1, 1);

            if (nowScale < moveValue)
            {
                nowScale = moveValue;
            }

            yield return null;
        }
    }

}
