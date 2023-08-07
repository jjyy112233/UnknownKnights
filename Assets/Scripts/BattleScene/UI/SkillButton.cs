using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private BaseUnit unit;

    [SerializeField]
    private Image profileImage;
    [SerializeField]
    private Image skillCoolImage;
    [SerializeField]
    private TextMeshProUGUI skillCoolTxt;
    [SerializeField]
    private Button skillCoolBtn;

    [SerializeField]
    private TeamHpBar hpBar;

    public void SetUnit(BaseUnit baseUnit)
    {
        unit = baseUnit;
        skillCoolBtn.onClick.AddListener(() => baseUnit.SkillCoolButton());

        Addressables.LoadAssetAsync<Sprite>($"{unit.UnitName}Profile").Completed += SpriteLoaded;
    }

    private void SpriteLoaded(AsyncOperationHandle<Sprite> obj)
    {
        switch (obj.Status)
        {
            case AsyncOperationStatus.Succeeded:
                profileImage.sprite = obj.Result;
                break;

            case AsyncOperationStatus.Failed:
                Debug.Log("스프라이트 로드 실패");
                break;

            default:
                break;
        }
    }
    public void SetCool()
    {
        skillCoolImage.fillAmount = 1 - (unit.NowSkillCool / unit.SkillCool);
        skillCoolTxt.text = $"{(int)unit.SkillCool - (int)unit.NowSkillCool}";
    }
    public void SetDie()
    {
        skillCoolImage.fillAmount = 0;
        profileImage.color = Color.red;
        skillCoolTxt.gameObject.SetActive(false);
        skillCoolBtn.enabled = false;
    }
    public void OnClick()
    {
        unit.OnClickSkill();
    }
    public void SetHp(float value)
    {
        hpBar.MoveValue = value;
    }
}
