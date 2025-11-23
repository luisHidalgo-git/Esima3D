using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight; // referencia a la luz
    private bool isOn = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlight.enabled = isOn;

            // 🔊 Llamar al AudioManager
            if (isOn)
                AudioManager.Instance.PlayFlashlightOn();
            else
                AudioManager.Instance.PlayFlashlightOff();
        }
    }
}
