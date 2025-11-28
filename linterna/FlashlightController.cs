using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;
    private bool isOn = false;
    private bool hasUsedFlashlight = false;

    void Update()
    {
        bool canUseFlashlight = GameplayDialogueTriggers.Instance != null && GameplayDialogueTriggers.Instance.CanUseFlashlight();

        if (!canUseFlashlight)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlight.enabled = isOn;

            if (isOn)
                AudioManager.Instance.PlayFlashlightOn();
            else
                AudioManager.Instance.PlayFlashlightOff();

            if (!hasUsedFlashlight)
            {
                hasUsedFlashlight = true;
            }
        }
    }
}
