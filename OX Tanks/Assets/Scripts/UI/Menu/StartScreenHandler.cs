using TMPro;
using UnityEngine;

public class StartScreenHandler : MonoBehaviour
{

    [SerializeField] GameObject[] weaponsPlayer1;
    [SerializeField] GameObject[] weaponsPlayer2;
    [SerializeField] TextMeshProUGUI player1Name;
    [SerializeField] TextMeshProUGUI player2Name;
    [SerializeField] TextMeshProUGUI roundStartCountDownText;
    [SerializeField] TextMeshProUGUI roundHeaderText;
    [SerializeField] TextMeshProUGUI pointsMiddle;
    [SerializeField] TextMeshProUGUI pointsPlayer1;
    [SerializeField] TextMeshProUGUI pointsPlayer2;
    [SerializeField] GameObject roundStartButton;
    [SerializeField] int roundStartCountDown = 1;

    private readonly string mapName = "singleplayer_buro";

    private int p1Index = 0;
    private int p2Index = 1;
    private float counter = 0;
    private readonly string countDownText = "New round starts in...{0}s";
    private readonly string countDownTextNow = "New round starts NOW";
    private readonly string newRoundText = "Round {0} / {1}";

    /// <summary>
    /// The current round numer
    /// </summary>
    public static int CurrentRoundNumber = 1;

    /// <summary>
    /// The amount of rounds to play
    /// </summary>
    public static int RoundsPerGame = 5;

    private void LoadGameScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(mapName);

        string name1 = player1Name == null ||
                       player1Name.text == null ||
                       player1Name.text.Trim() == string.Empty ?
                                  "Player 1" :
                                  player1Name.text;
        string name2 = player2Name == null ||
                       player2Name.text == null ||
                       player2Name.text.Trim() == string.Empty ?
                                  "Player 2" :
                                  player2Name.text;        
        PlayerSpawner.player1 = new PlayerSpawner.Spawn(p1Index, name1, 1);
        PlayerSpawner.player2 = new PlayerSpawner.Spawn(p2Index, name2, 2);
    }

    private int GetPointsPlayer1(){
        //TODO: get the actual points (rounds won) for player 1
        return 0;
    }

    private int GetPointsPlayer2(){
        //TODO: get the actual points (rounds won) for player 2
        return 0;
    }

    void SelectMenuItem(GameObject[] items, int index){
        if (index > weaponsPlayer1.Length) return;

        for (int i = 0; i < weaponsPlayer1.Length; i++){
            var current = items[i];
            var tmp = current.GetComponent<TextMeshProUGUI>();
            if(tmp != null && i == index){                
                tmp.color = Color.yellow;
            }
            else if(tmp != null){
                tmp.color = Color.white;
            }
        }
    }

    /// <summary>
    /// Called by UNITY
    /// </summary>
    void Awake()
    {

        //Header
        if (roundHeaderText != null)
        {
            //Display current round number
            roundHeaderText.text = string.Format(newRoundText, CurrentRoundNumber, RoundsPerGame);
        }


        //Default weapon/tank selection
        SelectMenuItem(weaponsPlayer1, p1Index);
        SelectMenuItem(weaponsPlayer2, p2Index);

        //Enable dissable counter / start button
        if (DoCounting())
        {
            //Between the rounds we count down
            roundStartCountDownText.gameObject.SetActive(true);
            if (roundStartButton != null)
            {
                roundStartButton.SetActive(false);
            }
            counter = roundStartCountDown;
            SetCountDownText(counter);
        }
        else
        {
            //Count down functionality dissabled; This is when the game starts (first round).
            if (roundStartCountDownText != null)
            {
                roundStartCountDownText.gameObject.SetActive(false);

            }
            if (roundStartButton != null)
            {
                roundStartButton.SetActive(true);
            }
        }

        //Display points if the game is running / at least one round was played
        if (CurrentRoundNumber > 1){
            pointsPlayer1.text = GetPointsPlayer1().ToString();
            pointsPlayer2.text = GetPointsPlayer2().ToString();
        }
        else {
            //Hide the points if the game just started
            pointsPlayer1.gameObject.SetActive(false);
            pointsPlayer2.gameObject.SetActive(false);
            pointsMiddle.gameObject.SetActive(false);
        }
	}

    /// <summary>
    /// Sets the count down text.
    /// </summary>
    /// <param name="seconds">current value in seconds</param>
    private void SetCountDownText(float seconds) {
        if(seconds > 1){
            roundStartCountDownText.text = string.Format(countDownText, (int)seconds);
        }
        else{
            roundStartCountDownText.text = countDownTextNow;
        }

    }

    /// <summary>
    /// Whether or not the screen is counting down and automatically loads the next round
    /// </summary>
    /// <returns><c>true</c>, if counting is done, <c>false</c> otherwise.</returns>
    private bool DoCounting() {
        return CurrentRoundNumber > 1 && roundStartCountDown > 0 && roundStartCountDownText != null;
    }

    /// <summary>
    /// Called by Unity
    /// </summary>
    private void Update(){
        if(Input.GetKeyDown(KeyCode.S)){
            if(p1Index < weaponsPlayer1.Length - 1){
                SelectMenuItem(weaponsPlayer1, ++p1Index);    
            }
        }
        if (Input.GetKeyDown(KeyCode.W)){
            if (p1Index > 0){
                SelectMenuItem(weaponsPlayer1, --p1Index);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (p2Index < weaponsPlayer2.Length - 1){
                SelectMenuItem(weaponsPlayer2, ++p2Index);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (p2Index > 0){
                SelectMenuItem(weaponsPlayer2, --p2Index);
            }
        }

        if(DoCounting()){
            //count down the timer
            if (roundStartCountDown > 0)
                counter = counter - Time.deltaTime;
            SetCountDownText(counter);       

            if(counter < 0){
                LoadGameScene();
            }
        }    
	}


	/// <summary>
	/// Event Handler if "BtnFight" is clicked
	/// </summary>
	public void OnFightButtonClicked(){
        LoadGameScene();
    }
}
