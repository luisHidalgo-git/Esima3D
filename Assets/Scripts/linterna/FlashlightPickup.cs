using UnityEngine;

public class FlashlightPickup : MonoBehaviour, IInteractable

{
    public string promptText = "Recoger linterna [E]";
    public bool destroyOnPickup = true;
    public bool startBatteryFull = true;

    public string GetPrompt() => promptText;

    public void Interact(GameObject actor)
    {
        var flashlight = actor.GetComponentInChildren<FlashlightSystem>(true);
        if (flashlight == null) return;

        flashlight.GainFlashlight(startBatteryFull);

        if (destroyOnPickup)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
