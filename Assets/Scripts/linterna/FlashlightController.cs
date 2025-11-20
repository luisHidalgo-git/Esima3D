using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight; // referencia a la luz
    private bool isOn = false;

    void Update()
    {
        // Al presionar la tecla F, alterna la linterna
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlight.enabled = isOn;
        }
    }
}
