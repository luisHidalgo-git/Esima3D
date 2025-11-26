using UnityEngine;
using System.Collections;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;

    private bool lockInput = false; // 🔒 para bloquear el ratón temporalmente

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (lockInput) return; // si está bloqueado, no mover con ratón

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // 🔥 Método para forzar la cámara hacia un objetivo
    public void LookAtTarget(Transform target, float duration = 0.3f, float verticalOffset = 0f)
    {
        StartCoroutine(LookAtTargetCoroutine(target, duration, verticalOffset));
    }

    private IEnumerator LookAtTargetCoroutine(Transform target, float duration, float verticalOffset)
    {
        lockInput = true;

        Quaternion startRotCam = transform.rotation;
        Quaternion startRotBody = playerBody.rotation;

        // Dirección hacia el objetivo + offset vertical
        Vector3 dir = (target.position + Vector3.up * verticalOffset - transform.position).normalized;
        Quaternion targetRotCam = Quaternion.LookRotation(dir);

        Vector3 flatDir = new Vector3(dir.x, 0, dir.z);
        Quaternion targetRotBody = Quaternion.LookRotation(flatDir);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotCam, targetRotCam, elapsed / duration);
            playerBody.rotation = Quaternion.Slerp(startRotBody, targetRotBody, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotCam;
        playerBody.rotation = targetRotBody;

        lockInput = false; // devolver control al jugador
    }
}
