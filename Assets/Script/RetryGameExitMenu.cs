using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryGameExitMenu : MonoBehaviour
{

    public void RetryGame()
    {
        // Detener todos los audios en la escena
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Stop();
        }

        // NO resetear el flag, para que no se reproduzca el video de nuevo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void VolverMenu()
    {
        // Detener todos los audios en la escena
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Stop();
        }
        PlayerPrefs.DeleteKey("SelectedCategory");
        SceneManager.LoadScene("MenuPrincipal");
    }
}
