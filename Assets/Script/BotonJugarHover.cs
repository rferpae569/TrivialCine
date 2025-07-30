using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BotonJugarHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform botonTransform;

    public Vector3 escalaOriginal = Vector3.one;
    public Vector3 escalaHover = new Vector3(1.1f, 1.1f, 1f);
    public float velocidadEscalado = 5f;
    [SerializeField] Animator animator;

    private bool estaEncima = false;

    //Todo este codigo que se enucnetra aqui sirve para que, al pasar
    //el raton, el boton lo detecte y se vuelva grande haciendo una especie de zoom

    void Start()
    {
        if (botonTransform == null)
            botonTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        estaEncima = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        estaEncima = false;
    }

    void Update()
    {
        Vector3 escalaObjetivo = estaEncima ? escalaHover : escalaOriginal;
        botonTransform.localScale = Vector3.Lerp(botonTransform.localScale, escalaObjetivo, Time.deltaTime * velocidadEscalado);
    }

    public void mouseOpenButton()
    {
        animator.SetBool("IsOpen", true); 
    }

    public void mouseCloseButton()
    {
        animator.SetBool("IsOpen", false);
    }
}



