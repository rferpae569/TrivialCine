
using UnityEngine;

public class ChooseAnswer : MonoBehaviour
{
    [SerializeField] TrivialManager trivialManager;

    //Para detectar la respuesta seleccionada
    public void ChooserAnswerButton(int number)
    {
        trivialManager.AnswerSelected(number);
    }
}

