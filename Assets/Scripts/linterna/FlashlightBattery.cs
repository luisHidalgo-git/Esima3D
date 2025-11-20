using UnityEngine;
using UnityEngine.UI;

public class FlashlightBattery : MonoBehaviour
{
    [Header("Lógica de batería")]
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float drainRate = 10f; // por segundo
    public bool flashlightOn = false;

    [Header("Referencias")]
    public Light flashlight;
    public Image batteryBar; // UI tipo barra o caja

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentBattery > 0f)
        {
            flashlightOn = !flashlightOn;
            flashlight.enabled = flashlightOn;

            if (flashlightOn)
                AudioManager.Instance.PlayFlashlightOn();
            else
                AudioManager.Instance.PlayFlashlightOff();
        }

        if (flashlightOn)
        {
            currentBattery -= drainRate * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

            if (currentBattery <= 0f)
            {
                flashlightOn = false;
                flashlight.enabled = false;
                AudioManager.Instance.PlayFlashlightOff();
            }
        }

        UpdateBatteryUI();
    }

    public void RechargeBattery(float amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }

    void UpdateBatteryUI()
    {
        if (batteryBar != null)
            batteryBar.fillAmount = currentBattery / maxBattery;
    }
}
