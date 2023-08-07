using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleTime : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timerTxt;
    BattleManager battleManager;
    float nowTime;
    float NowTime
    {
        get { return nowTime; }
        set {
            nowTime = value; 
            timerTxt.text = $"{(int)(nowTime / 60)}:{(int)(nowTime % 60)}";
        }
    }

    private void Awake()
    {
        battleManager = GameObject.FindAnyObjectByType<BattleManager>();
    }
    public void SetTime(float time)
    {
        NowTime = time;
    }

    public void StartTimer() => StartCoroutine(Timer());
    public void StopTimer() => StopCoroutine(Timer());

    IEnumerator Timer()
    {
        Debug.Log("test");
        float sec = 0f;

        while (true)
        {
            sec += Time.deltaTime;

            if (sec >= 1f)
            {
                sec = 0f;

                NowTime--;

                if (NowTime <= 0f)
                {
                    NowTime = 0f;
                    break;
                }
            }

            if(NowTime <= 0)
            {
                battleManager.BattleTimeEnd();
            }

            yield return null;
        }
    }
}
