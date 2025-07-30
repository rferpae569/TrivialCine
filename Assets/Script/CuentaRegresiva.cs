using UnityEngine;

public class CuentaRegresiva : MonoBehaviour
{
    [SerializeField] private GameObject panelCuentaRegresiva;
    [SerializeField] private GameObject trivialPanel;
    [SerializeField] private TrivialManager trivialManager;
    [SerializeField] private GameObject fondoJuego;

    [SerializeField] private AudioSource fondoCuentaRegresiva;
    [SerializeField] private float startAtSeconds = 2.5f;

    void Start()
    {
        if (fondoCuentaRegresiva != null)
        {
            fondoCuentaRegresiva.time = startAtSeconds; // Empieza desde tiempo deseado
            fondoCuentaRegresiva.Play();
        }
    }

    public void OnCountdownFinished()
    {
        // Desactiva el panel de cuenta regresiva y activa el trivial
        panelCuentaRegresiva.SetActive(false);
        trivialPanel.SetActive(true);
        fondoJuego.SetActive(true);

        // Iniciar el trivial manualmente
        trivialManager.StartGameAfterCountdown();
    }
}
