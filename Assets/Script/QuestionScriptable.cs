using UnityEngine;

public enum QuestionCategorie
{
    TODOS,
    ADO,
    TITULOS,
    PERSDAT
}

[CreateAssetMenu]

public class QuestionScriptable : ScriptableObject
{
    //Contenido del Scriptable
    public int questionId;
    [TextAreaAttribute] public string enunciate;
    //public string questionName;
    public string[] answers;
    public int correctAnswer;

    public QuestionCategorie questionCategorie;
}
