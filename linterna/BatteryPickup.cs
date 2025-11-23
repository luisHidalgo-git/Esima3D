using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float rechargeAmount = 25f;

    void OnTriggerEnter(Collider other)
    {
        FlashlightBattery battery = other.GetComponent<FlashlightBattery>();
        if (battery != null)
        {
            battery.RechargeBattery(rechargeAmount);
            AudioManager.Instance?.PlayBatteryPickup();
            Destroy(gameObject);
        }
    }
}
