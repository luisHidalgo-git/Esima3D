using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class FlashlightSystem : MonoBehaviour
{
    [Header("Referencias")]
    public Light flashlightLight;
    public AudioSource audioSource;

    [Header("Batería")]
    public float maxBattery = 100f;
    [SerializeField] private float currentBattery = 0f;
    public float drainRate = 5f;
    public float lowBatteryThreshold = 15f;
    public bool autoEnableOnPickup = true;

    [Header("Control")]
    public KeyCode toggleKey = KeyCode.F;
    [SerializeField] private bool hasFlashlight = false;
    [SerializeField] private bool isOn = false;

    [Header("Eventos")]
    public UnityEvent<float, float> onBatteryChanged;
    public UnityEvent onFlashlightGained;
    public UnityEvent<bool> onFlashlightToggled;
    public UnityEvent onBatteryDepleted;
    public UnityEvent onLowBattery;

    private bool lowNotified = false;

    void Awake()
    {
        if (flashlightLight != null) flashlightLight.enabled = false;
    }

    void Update()
    {
        HandleToggleInput();
        DrainIfOn();
        NotifyLowBatteryIfNeeded();
    }

    private void HandleToggleInput()
    {
        if (!hasFlashlight) return;
        if (Input.GetKeyDown(toggleKey))
        {
            if (!isOn && currentBattery <= 0f)
            {
                PlaySfx("noPower");
                return;
            }

            isOn = !isOn;
            if (flashlightLight) flashlightLight.enabled = isOn;
            onFlashlightToggled?.Invoke(isOn);
            PlaySfx(isOn ? "clickOn" : "clickOff");
        }
    }

    private void DrainIfOn()
    {
        if (!hasFlashlight || !isOn) return;

        currentBattery -= drainRate * Time.deltaTime;
        if (currentBattery <= 0f)
        {
            currentBattery = 0f;
            if (isOn)
            {
                isOn = false;
                if (flashlightLight) flashlightLight.enabled = false;
                onBatteryDepleted?.Invoke();
                onFlashlightToggled?.Invoke(false);
                PlaySfx("depleted");
            }
        }
        onBatteryChanged?.Invoke(currentBattery, maxBattery);
    }

    private void NotifyLowBatteryIfNeeded()
    {
        if (!hasFlashlight) return;
        if (currentBattery > 0f && currentBattery <= lowBatteryThreshold)
        {
            if (!lowNotified)
            {
                lowNotified = true;
                onLowBattery?.Invoke();
            }
        }
        else
        {
            lowNotified = false;
        }
    }

    public void GainFlashlight(bool startFull = true)
    {
        hasFlashlight = true;
        onFlashlightGained?.Invoke();

        if (startFull)
            currentBattery = maxBattery;

        if (autoEnableOnPickup && currentBattery > 0f)
        {
            isOn = true;
            if (flashlightLight) flashlightLight.enabled = true;
            onFlashlightToggled?.Invoke(true);
        }

        onBatteryChanged?.Invoke(currentBattery, maxBattery);
    }

    public void AddBattery(float amount)
    {
        if (!hasFlashlight) return;
        currentBattery = Mathf.Clamp(currentBattery + amount, 0f, maxBattery);
        onBatteryChanged?.Invoke(currentBattery, maxBattery);
        PlaySfx("pickupBattery");
    }

    public float GetBatteryPercent() => maxBattery > 0f ? currentBattery / maxBattery : 0f;
    public bool HasFlashlight() => hasFlashlight;

    private void PlaySfx(string key)
    {
        if (!audioSource) return;
        // Aquí puedes conectar tu AudioManager o usar PlayOneShot según 'key'
    }
}
