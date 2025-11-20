using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlashlightUI : MonoBehaviour
{
    public FlashlightSystem flashlight;
    public Slider batterySlider;
    public TMP_Text batteryText;
    public CanvasGroup group;

    void Start()
    {
        if (group) group.alpha = 0f;
        flashlight.onBatteryChanged.AddListener(UpdateBattery);
        flashlight.onFlashlightGained.AddListener(ShowUI);
    }

    private void UpdateBattery(float current, float max)
    {
        float percent = (max > 0f) ? current / max : 0f;
        if (batterySlider) batterySlider.value = percent;
        if (batteryText) batteryText.text = Mathf.RoundToInt(percent * 100f) + "%";
    }

    private void ShowUI()
    {
        if (group) group.alpha = 1f;
        UpdateBattery(flashlight.GetBatteryPercent() * flashlight.maxBattery, flashlight.maxBattery);
    }
}
