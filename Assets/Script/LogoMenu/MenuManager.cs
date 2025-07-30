using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject panelMenuJuego;
    public GameObject panelControles;

    public TextMeshProUGUI oscarCountText;
    public TextMeshProUGUI globeCountText;
    public TextMeshProUGUI baftaCountText;

    public Button[] categoriaButtons;  // Asigna en el Inspector
    private int categoriaSeleccionada = 0;

    public Color categoriaNormalColor = Color.white;
    public Color categoriaSeleccionadaColor = new Color(1f, 0.8f, 0.3f); // Color dorado claro

    void Start()
    {
        SeleccionarCategoria(0); // Por defecto seleccionamos "Todos"

        int oscarCount = PlayerPrefs.GetInt("OscarCount", 0);
        int globeCount = PlayerPrefs.GetInt("GlobeCount", 0);
        int baftaCount = PlayerPrefs.GetInt("BAFTACount", 0);

        oscarCountText.text = "= " + oscarCount;
        globeCountText.text = "= " + globeCount;
        baftaCountText.text = "= " + baftaCount;
    }


    public void Play()
    {
        SceneManager.LoadScene("Trivial");
    }

    //Activamos el panel de controles y desactivamos el del menu
    public void Controles()
    {
        panelMenuJuego.SetActive(false);
        panelControles.SetActive(true);
    }

    //Activamos el panel del menu y desactivamos el de controles
    public void VolverMenuControles()
    {
        panelControles.SetActive(false);
        panelMenuJuego.SetActive(true);
    }

    public void SeleccionarCategoria(int index)
    {
        categoriaSeleccionada = index;
        PlayerPrefs.SetInt("SelectedCategory", categoriaSeleccionada);

        for (int i = 0; i < categoriaButtons.Length; i++)
        {
            ColorBlock colors = categoriaButtons[i].colors;

            bool esSeleccionado = (i == index);
            Color color = esSeleccionado ? categoriaSeleccionadaColor : categoriaNormalColor;

            colors.normalColor = color;
            colors.highlightedColor = color;
            colors.selectedColor = color;

            categoriaButtons[i].colors = colors;

            if (esSeleccionado)
            {
                categoriaButtons[i].Select();
            }
        }
    }



}
