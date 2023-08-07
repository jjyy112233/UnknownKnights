using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TeamHpBar : MonoBehaviour
{
    Image hpBar;
    [SerializeField]
    float moveValue = 1f;
    float nowValue = 1f;
    public float MoveValue
    {
        set
        {
            moveValue = value;
        }
    }

    private void Awake()
    {
        hpBar = transform.GetComponent<Image>();
    }
    private void Start()
    {
        StartCoroutine(MoveHp());
    }

    IEnumerator MoveHp()
    {
        while (true)
        {
            if (nowValue <= 0)
            {
                nowValue = 0f;
                break;
            }

            var max = Mathf.Max(nowValue, moveValue);
            var min = Mathf.Min(nowValue, moveValue);

            nowValue = Mathf.Lerp(max, min, Time.deltaTime * 2f);
            hpBar.fillAmount = nowValue;

            if (nowValue < moveValue)
            {
                nowValue = moveValue;
            }

            yield return null;
        }
    }

}
