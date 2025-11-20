using UnityEngine;

public class BatteryPickup : MonoBehaviour, IInteractable
{
    public string promptText = "Tomar batería [E]";
    public float batteryAmount = 25f;
    public bool destroyOnPickup = true;

    public string GetPrompt() => promptText;

    public void Interact(GameObject actor)
    {
        var flashlight = actor.GetComponentInChildren<FlashlightSystem>(true);
        if (flashlight == null || !flashlight.HasFlashlight()) return;

        flashlight.AddBattery(batteryAmount);

        if (destroyOnPickup)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
