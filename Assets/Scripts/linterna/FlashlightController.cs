using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;
    private bool isOn = false;
    private bool hasUsedFlashlight = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlight.enabled = isOn;

            if (isOn)
                AudioManager.Instance.PlayFlashlightOn();
            else
                AudioManager.Instance.PlayFlashlightOff();

            // 🗨️ Mostrar diálogo la primera vez
            if (!hasUsedFlashlight)
            {
                GameplayDialogueTriggers.Instance.OnFlashlightTutorial();
                hasUsedFlashlight = true;
            }
        }
    }
}
