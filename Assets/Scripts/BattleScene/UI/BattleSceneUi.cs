using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneUi : MonoBehaviour
{
    [SerializeField]
    private BattleTime battleTime;

    [SerializeField]
    private TeamHpBar[] hpBars;

    [SerializeField]
    private SkillButton[] skillButtons;

    [SerializeField]
    private Image autoImage;
    [SerializeField]
    private TextMeshProUGUI autoText;


    public void AddUnit(BaseUnit unit, int idx)
    {
        skillButtons[idx].gameObject.SetActive(true);
        skillButtons[idx].SetUnit(unit);
        unit.SkillCoolButton = skillButtons[idx].SetCool;
        unit.SetDieButton = skillButtons[idx].SetDie;
        unit.SetButtonHp = skillButtons[idx].SetHp;
    }

    public void SetHpBar(UnitTeamType type, float value)
    {
        hpBars[(int)type].MoveValue = value;
    }

    public void SetTimer(float time)
    {
        battleTime.SetTime(time);
    }
    public void StartTimer()
    {
        battleTime.StartTimer();
    }
    public void StopTimer()
    {
        battleTime.StopTimer();
    }

    public void OnClickAutoButton()
    {
        FindAnyObjectByType<BattleManager>().OnClickAuto();
        SetAutoButtonColor(FindAnyObjectByType<BattleManager>().IsAuto);
    }

    public void SetAutoButtonColor(bool state)
    {
        autoImage.color = state ? Color.yellow : Color.gray;
        autoText.color = state ? Color.yellow : Color.gray;
    }
}
