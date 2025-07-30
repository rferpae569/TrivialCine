using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LogoIntro : MonoBehaviour
{
    public float delayAfterFadeIn = 4f;

    void Start()
    {
        // Inicia la rutina para esperar y cambiar de escena
        StartCoroutine(WaitAndLoadMenu());
    }

    private IEnumerator WaitAndLoadMenu()
    {
        // Espera 4 segundos
        yield return new WaitForSeconds(delayAfterFadeIn);

        // Cambia a la escena del menú principal
        SceneManager.LoadScene("MenuPrincipal");
    }
}

