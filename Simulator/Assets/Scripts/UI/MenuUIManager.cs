using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelMisSistemas;

    public void IrAMisSistemas()
    {
        panelMenuPrincipal.SetActive(false);
        panelMisSistemas.SetActive(true);
    }

    public void VolverAlMenu()
    {
        panelMenuPrincipal.SetActive(true);
        panelMisSistemas.SetActive(false);
    }
}