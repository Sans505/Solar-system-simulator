using UnityEngine;

public class MenuInicial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //abrir escena SampleScene
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //exit
    public void Exit()
    {
        //printar mensaje de salida
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
