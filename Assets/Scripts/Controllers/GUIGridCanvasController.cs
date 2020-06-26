using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIGridCanvasController : MonoBehaviour
{
    public Text[] timerText;
    public Text[] scoreText;
    public Text[] bestScoreText;
    public Text currentScoreGamePaused;
    public Text timelapsedGamePaused;
    private float startTime;
    private float temporalStopTime;
    private float totalTimeLapsedOfAllPauses;
    public Button pauseGameButton;
    public Button finishGameButton;
    public Button continueGameButton;
    public Canvas gamePausedCanvas;
    public GameObject lines;

    public Canvas canvas;


    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.Score = 0;
        startTime = UnityEngine.Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.NumberOfPlayers == 1)
        {
            timerText[1].gameObject.SetActive(false);
            scoreText[1].gameObject.SetActive(false);
            bestScoreText[1].gameObject.SetActive(false);

        }
        if (GameController.Instance.GamePausedCanvasEnabled == false)
        {
            float time = UnityEngine.Time.time - startTime - totalTimeLapsedOfAllPauses;
            var minutes = (int)time / 60;
            var seconds = (int)time % 60;
            var formatMinutes = minutes <= 9 ? "0" + minutes : minutes + "";
            var formatSeconds = seconds <= 9 ? "0" + seconds : seconds + "";

            foreach (var text in timerText)
            {
                text.text = formatMinutes + ":" + formatSeconds;
            }
            foreach (var score in scoreText)
            {
                score.text = "Spielstand: " + GameController.Instance.Score;
            }
            foreach (var bestScore in bestScoreText)
            {
                bestScore.text = "bester Spielstand: " + GameController.Instance.BestScore;
            }
            currentScoreGamePaused.text = "Aktueller Score: " + GameController.Instance.Score;
            timelapsedGamePaused.text = "Zeitablauf: " + formatMinutes + ":" + formatSeconds;
            gamePausedCanvas.gameObject.SetActive(false);
            lines.SetActive(true);
        }
        else
        {
            gamePausedCanvas.gameObject.SetActive(true);
            lines.SetActive(false);
        }

        if (GameController.Instance.GUICanvasEnabled)
        {
            canvas.gameObject.SetActive(true);
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }
    }

    public void PauseGame()
    {
        temporalStopTime = UnityEngine.Time.time;
        GameController.Instance.GamePausedCanvasEnabled = true;
        GameController.Instance.GUICanvasEnabled = false;
    }

    public void ResumeGame()
    {
        var loquesea = UnityEngine.Time.time - temporalStopTime;
        totalTimeLapsedOfAllPauses += loquesea;
        GameController.Instance.GamePausedCanvasEnabled = false;
        GameController.Instance.GUICanvasEnabled = true;
    }

    public void GoToStartScreen(int sceneIndex)
    {
        GameController.Instance.GameIOController.SaveNewGameData();
        SceneManager.LoadSceneAsync(sceneIndex);
        GameController.Instance.GamePausedCanvasEnabled = false;
    }
}
