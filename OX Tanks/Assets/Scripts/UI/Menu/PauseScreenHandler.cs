using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenHandler : MonoBehaviour {

    [SerializeField] GameObject pauseScreen;
    [SerializeField] TextMeshProUGUI resume;
    [SerializeField] TextMeshProUGUI exit;

    private int menuIndex = 0;

    public static bool IsPaused { get; private set; }


    /// <summary>
    /// Called by unity
    /// </summary>
    private void Awake() {
        Pause(false);
	}

	void SelectMenuItem(int index)
    {
        if (index == 0){
            resume.color = Color.yellow;
            exit.color = Color.white;
        }
        else if(index == 1){
            resume.color = Color.white;
            exit.color = Color.yellow;
        }
    }

    private void QuitRound(){
        SceneManager.LoadScene("StartScene");
    }

    private void Pause(bool pause){                
        pauseScreen.SetActive(pause);
        IsPaused = pause;
    }

	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Pause(!IsPaused);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            menuIndex = (int) Mathf.Max(0, menuIndex - 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            menuIndex = (int)Mathf.Min(1, menuIndex + 1);
        }
        SelectMenuItem(menuIndex);

        if(Input.GetKeyDown(KeyCode.Return)){
            if(menuIndex == 0){
                Pause(false);
            }
            else if(menuIndex == 1){
                QuitRound();
            }
        }
	}
}
