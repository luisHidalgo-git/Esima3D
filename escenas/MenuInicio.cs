using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void Opciones()
    {
        Debug.Log("Opciones abiertas");
    }
}
