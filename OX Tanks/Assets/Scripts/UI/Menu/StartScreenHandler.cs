using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartScreenHandler : MonoBehaviour
{

    [SerializeField] GameObject[] weaponsPlayer1;
    [SerializeField] GameObject[] weaponsPlayer2;
    [SerializeField] TextMeshProUGUI roundStartCountDownText;
    [SerializeField] GameObject roundStartButton;
    [SerializeField] int roundStartCountDown = 1;

    private int p1Index = 0;
    private int p2Index = 1;
    private float counter = 0;
    private readonly string countDownText = "New round starts in...{0}s";
    private readonly string countDownTextNow = "New round starts NOW";


    void LoadGameScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Sandbox");
        PlayerSpawner.player1 = new PlayerSpawner.Spawn(p1Index, "Benny");
        PlayerSpawner.player2 = new PlayerSpawner.Spawn(p2Index, "Klaus");
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
    void Awake(){
        SelectMenuItem(weaponsPlayer1, p1Index);
        SelectMenuItem(weaponsPlayer2, p2Index);
        if(DoCounting()){
            //Between the rounds we count down
            roundStartCountDownText.gameObject.SetActive(true);
            if (roundStartButton != null){
                roundStartButton.SetActive(false);
            }
            counter = roundStartCountDown;
            SetCountDownText(counter);
        }
        else{
            //Count down functionality dissabled; This is when the game starts (first round).
            if(roundStartCountDownText != null){
                roundStartCountDownText.gameObject.SetActive(false);    

            }
            if(roundStartButton != null){
                roundStartButton.SetActive(true);    
            }
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
    /// Whether or not the screen is countingn down and automatically loads the next round
    /// </summary>
    /// <returns><c>true</c>, if counting is done, <c>false</c> otherwise.</returns>
    private bool DoCounting() {
        return roundStartCountDown > 0 && roundStartCountDownText != null;
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
