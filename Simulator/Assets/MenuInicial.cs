using UnityEngine;

public class menuInicial : MonoBehaviour
{
    public void nuevoSistema()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestScene");
    }

    public GameObject miPopup; 
    public void MostrarPopup()
    {
        miPopup.SetActive(true); // Esto lo hace visible
    }
    public void OcultarPopup()
    {
        miPopup.SetActive(false); // Esto lo esconde
    }

    public void salirJuego()
    {
        print("Saliendo del juego...");
        Application.Quit();
    }
}
