using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliceView : UIComponentBase
{
    [Header("Auto-Bound References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Configure(WheelSliceData data, int zoneIndex)
    {
        if (data.sliceType == SliceType.Bomb)
        {
            ConfigureAsBomb(data);
        }
        else
        {
            ConfigureAsReward(data, zoneIndex);
        }
    }

    protected override void AutoBindReferences()
    {

    }

    private void ConfigureAsBomb(WheelSliceData data)
    {
        if (iconImage != null)
        {
            iconImage.sprite = data.reward.icon;
            iconImage.gameObject.SetActive(data.reward.icon != null);
        }

        if (amountText != null)
            amountText.gameObject.SetActive(false); // bombada miktar gösterilmez
    }

    private void ConfigureAsReward(WheelSliceData data, int zoneIndex)
    {
        if (iconImage != null)
        {
            Sprite sprite = data.reward.icon != null ? data.reward.icon : data.reward?.icon;
            iconImage.sprite = sprite;
            iconImage.gameObject.SetActive(sprite != null);
        }

        if (amountText != null && data.reward != null)
        {
            amountText.gameObject.SetActive(true);
            int value = data.reward.GetValueForZone(zoneIndex);
            amountText.text = $"x{value}";
        }
    }
}