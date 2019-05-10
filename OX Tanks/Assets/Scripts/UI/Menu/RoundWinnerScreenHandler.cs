using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundWinnerScreenHandler : MonoBehaviour {


    string text = "{0} won!";
    private const string startScene = "StartScene";
    float delayTimer = 3;

    [SerializeField]
    private TextMeshProUGUI winnerText;

	// Use this for initialization
	void Start () {       
	}


    // Update is called once per frame
    void Update()
    {
        if (RoundManager.instance.roundFinished)
        {
            winnerText.gameObject.SetActive(true);
            delayTimer -= Time.deltaTime;
            winnerText.text = string.Format(text, RoundManager.instance.playerNameWinnerLastRound);
            if(delayTimer < 0){
                UnityEngine.SceneManagement.SceneManager.LoadScene(startScene);    
            }
        }
        else{
            winnerText.gameObject.SetActive(false);
        }
	}
}
