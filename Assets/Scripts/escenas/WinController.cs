using UnityEngine;

public class WinController : MonoBehaviour
{
    void Start()
    {
        Debug.Log("WinController activo");
    }

    public void Jugar()
    {
        GameManager.Instance.LoadScene("Gameplay");
    }

    public void IrAlMenu()
    {
        GameManager.Instance.LoadScene("MainMenu");
    }

    public void Salir()
    {
        GameManager.Instance.QuitGame();
    }
}
