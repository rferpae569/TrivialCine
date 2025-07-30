using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrivialManager : MonoBehaviour
{
    public int questionCounter = 0;
    public int lives = 3;
    public int score = 0;
    public int highScore = 0;
    private bool canAnswer = false;


    public QuestionScriptable[] questionsTodos;
    public QuestionScriptable[] questionsADO;
    public QuestionScriptable[] questionsTitulos;
    public QuestionScriptable[] questionsPersonDat;

    private QuestionScriptable[] questions; // Final, el que usará el juego


    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Button[] answerButtons;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI PrizeText;
    [SerializeField] TextMeshProUGUI PrizeText2;
    [SerializeField] Image prizeImage;
    [SerializeField] Sprite prizeBAFTA;
    [SerializeField] Sprite prizeGlobe; 
    [SerializeField] Sprite prizeOscar; 
    [SerializeField] private GameObject trivialPanel;
    [SerializeField] private GameObject panelCuentaRegresiva;
    [SerializeField] private GameObject fondoJuego;
    [SerializeField] TextMeshProUGUI highScoreText;


    [SerializeField] private Image[] lifeIcons;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timePerQuestion = 15f;

    private Coroutine countdownCoroutine;
    private bool hasAnswered = false;
    private int correctStreak = 0;

    void Start()
    {
       
        // Obtener la categoría seleccionada
        int selectedCategoryIndex = PlayerPrefs.GetInt("SelectedCategory", 0);
        QuestionCategorie selectedCategory = (QuestionCategorie)PlayerPrefs.GetInt("SelectedCategory", 0);
        string highScoreKey = GetHighScoreKey(selectedCategory);
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreText.text = "Record: " + highScore;

        // Seleccionar el array correcto según la categoría
        switch (selectedCategory)
        {
            case QuestionCategorie.TODOS:
                questions = questionsTodos;
                break;
            case QuestionCategorie.ADO:
                questions = questionsADO;
                break;
            case QuestionCategorie.TITULOS:
                questions = questionsTitulos;
                break;
            case QuestionCategorie.PERSDAT:
                questions = questionsPersonDat;
                break;
            default:
                Debug.LogWarning("Categoría desconocida. Cargando preguntas de 'Todos'.");
                questions = questionsTodos;
                break;
        }


        ShuffleQuestions(questions);

        panelCuentaRegresiva.SetActive(true);
        trivialPanel.SetActive(false);
        fondoJuego.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    void ShuffleQuestions(QuestionScriptable[] questionsTemp)
    {
        for (int t = 0; t < questionsTemp.Length; t++)
        {
            QuestionScriptable tmp = questionsTemp[t];
            int r = UnityEngine.Random.Range(t, questionsTemp.Length);
            questionsTemp[t] = questionsTemp[r];
            questionsTemp[r] = tmp;
        }
    }

    //Cargamos la pregunta siempre y cuando sigamos teniendo vidas
    public void LoadQuestion()
    {
        if (lives <= 0 || questionCounter >= questions.Length)
        {
            GameOver();
            return;
        }

        StopAllCoroutines();
        hasAnswered = false;
        timerText.text = "";

        StartCoroutine(TypeQuestionText(questions[questionCounter].enunciate));

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI btnText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = questions[questionCounter].answers[i];

            answerButtons[i].interactable = true;
            SetButtonColor(answerButtons[i], Color.white);
        }
    }

    //Declaramos si es la respuesta correcto o no, y paramos el sonido del reloj
    public void AnswerSelected(int selectedIndex)
    {

        if (!canAnswer) return; //Ignora si aún no terminó de escribirse la pregunta

        if (hasAnswered)
            return;

        hasAnswered = true;

        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            AudioManager.Instance.StopLoopingSFX(); // parar tic-tac
        }

        StartCoroutine(HandleAnswerFeedback(selectedIndex));
        canAnswer = false; // Bloquea nuevos clics hasta que se cargue la siguiente pregunta
    }

    //Funcion que se encarga de actualizar los puntos
    void UpdateUI()
    {
        scoreText.text = "Puntos: " + score;

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = i < lives;
        }
    }

    //Cuando nos quedemos sin vidas, activamos el panel "GameOver"
    //Dependiendo del resultado se activara un sonido distinto.
    void GameOver()
    {
        Debug.Log("Fin del juego");
        trivialPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Puntuacion final: " + score;

        // Obtener categoría seleccionada
        QuestionCategorie currentCategory = (QuestionCategorie)PlayerPrefs.GetInt("SelectedCategory", 0);
        string highScoreKey = GetHighScoreKey(currentCategory);
        int savedHighScore = PlayerPrefs.GetInt(highScoreKey, 0);

        if (score > savedHighScore)
        {
            PlayerPrefs.SetInt(highScoreKey, score);
            PlayerPrefs.Save();
            highScore = score;
        }
        else
        {
            highScore = savedHighScore;
        }

        highScoreText.text = "Record: " + highScore;


        // Definir umbrales según la categoría
        int baftaThreshold = 20;
        int globeThreshold = 50;
        int oscarThreshold = 80;

        // Si es la categoría TODOS, ajustamos los umbrales proporcionalmente a 303 preguntas
        if (currentCategory == QuestionCategorie.TODOS)
        {
            int totalQuestions = 303;
            baftaThreshold = Mathf.FloorToInt(totalQuestions * 0.20f);  // 60
            globeThreshold = Mathf.FloorToInt(totalQuestions * 0.50f);  // 150
            oscarThreshold = Mathf.FloorToInt(totalQuestions * 0.80f);  // 240
        }

        // Otorgar premios según umbrales
        if (score >= oscarThreshold)
        {
            int oscarCount = PlayerPrefs.GetInt("OscarCount", 0);
            PlayerPrefs.SetInt("OscarCount", oscarCount + 1);

            PrizeText.text = "Genial, has ganado un...";
            prizeImage.sprite = prizeOscar;
            PrizeText2.text = "oscar";
            AudioManager.Instance.PlaySFX(AudioManager.Instance.applause3Clip);
        }
        else if (score >= globeThreshold)
        {
            int globeCount = PlayerPrefs.GetInt("GlobeCount", 0);
            PlayerPrefs.SetInt("GlobeCount", globeCount + 1);

            PrizeText.text = "Genial, has ganado un...";
            prizeImage.sprite = prizeGlobe;
            PrizeText2.text = "Globo de oro";
            AudioManager.Instance.PlaySFX(AudioManager.Instance.applause2Clip);
        }
        else if (score >= baftaThreshold)
        {
            int baftaCount = PlayerPrefs.GetInt("BAFTACount", 0);
            PlayerPrefs.SetInt("BAFTACount", baftaCount + 1);

            PrizeText.text = "Genial, has ganado un...";
            prizeImage.sprite = prizeBAFTA;
            PrizeText2.text = "BAFTA";
            AudioManager.Instance.PlaySFX(AudioManager.Instance.applause1Clip);
        }
        else
        {
            PrizeText.text = "Lo siento, no has ganado nada";
            prizeImage.gameObject.SetActive(false);
            PrizeText2.gameObject.SetActive(false);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.ohNoClip);
        }

        PlayerPrefs.Save(); // Guardar premios obtenidos
    }


    //Codigo para que la pregunta vaya apareciendo poco a poco.
    IEnumerator TypeQuestionText(string fullText)
    {
        questionText.text = "";
        canAnswer = false; // Esto bloqueara las respuestas para que no podamos contestar mientras se escribe la pregunta

        // Empezar tecleo continuo
        AudioManager.Instance.StartTypingSFX();

        foreach (char c in fullText)
        {
            questionText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        // Detener sonido al terminar
        AudioManager.Instance.StopTypingSFX();

        canAnswer = true; //Lo pasamos a true para contestar una vez escrita la pregunta

        countdownCoroutine = StartCoroutine(StartCountdown());
    }


    //Aqui tenemos el funcionamiento de la respuesta seleccionada
    //Si es correcta nos sumara un punto
    //Si fallamas nos quitara una vida,y si acertamos hasta conseguir una racha de 
    //3 o de 5 (dependiendo del caso, nos adara 1 vida
    IEnumerator HandleAnswerFeedback(int selectedIndex)
    {
        foreach (var btn in answerButtons)
            btn.interactable = false;

        int correctIndex = questions[questionCounter].correctAnswer;

        if (selectedIndex == correctIndex)
        {
            score++;
            correctStreak++;
            Debug.Log("Respuesta correcta");
            Debug.Log("Racha actual: " + correctStreak);
            Debug.Log("Vidas actuales: " + lives);

            SetButtonColor(answerButtons[selectedIndex], Color.green);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.correctAnswerClip);

            // Verificar si gana una vida por racha
            if (lives < 3)
            {
                if (lives == 2 && correctStreak >= 5)
                {
                    lives++;
                    correctStreak = 0;
                    Debug.Log("¡Has ganado una vida por racha de 5 respuestas!");
                }
                else if (lives == 1 && correctStreak >= 3)
                {
                    lives++;
                    correctStreak = 0;
                    Debug.Log("¡Has ganado una vida por racha de 3 respuestas!");
                }
            }
        }
        else
        {
            lives--;
            correctStreak = 0;
            Debug.Log("Respuesta incorrecta");
            Debug.Log("Racha reiniciada. Vidas ahora: " + lives);

            SetButtonColor(answerButtons[selectedIndex], Color.red);
            SetButtonColor(answerButtons[correctIndex], Color.green);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.wrongAnswerClip);
        }

        UpdateUI();
        yield return new WaitForSeconds(3f); //Esperamos 3 segundos entre pregunta y pregunta

        questionCounter++;
        LoadQuestion();
    }

    //Aqui tenemos el codigo de descuento de tiempo y el cambio de color
    //Empieza en verde, cuanod llega a 10 amarillo, y cuando llega a 5 rojo
    IEnumerator StartCountdown()
    {
        float remainingTime = timePerQuestion;

        // Comienza tic-tac
        AudioManager.Instance.PlayLoopingSFX(AudioManager.Instance.countdownClip);

        while (remainingTime > 0)
        {
            timerText.text = Mathf.CeilToInt(remainingTime).ToString();

            if (remainingTime > 10)
            {
                timerText.color = Color.green;
            }
            else if (remainingTime > 5)
            {
                timerText.color = Color.yellow;
            }
            else
            {
                timerText.color = Color.red;
            }

            yield return new WaitForSeconds(1f);
            remainingTime--;

            if (hasAnswered)
            {
                AudioManager.Instance.StopLoopingSFX(); // parar tic-tac
                yield break;
            }
        }


        timerText.text = "0";

        Debug.Log("Tiempo agotado");
        AudioManager.Instance.StopLoopingSFX(); // parar tic-tac

        lives--;
        correctStreak = 0; // Reiniciar racha por tiempo agotado
        UpdateUI();

        int correctIndex = questions[questionCounter].correctAnswer;
        SetButtonColor(answerButtons[correctIndex], Color.green);

        foreach (var btn in answerButtons)
            btn.interactable = false;

        yield return new WaitForSeconds(3f);

        questionCounter++;
        LoadQuestion();
    }

    //Para cambiar el color del boton seleccionado dependiendo de si es correcto o no
    void SetButtonColor(Button btn, Color color)
    {
        Image img = btn.GetComponent<Image>();
        if (img != null)
        {
            color.a = 1f;
            img.color = color;
        }
    }

    public void StartGameAfterCountdown()
    {
        UpdateUI();
        LoadQuestion();
    }

    private string GetHighScoreKey(QuestionCategorie category)
    {
        return "HighScore_" + category.ToString(); // Ej: HighScore_TITULOS
    }
}



