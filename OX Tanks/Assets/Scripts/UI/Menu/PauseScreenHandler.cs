using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenHandler : MonoBehaviour {

    [SerializeField] GameObject pauseScreen;
    [SerializeField] TextMeshProUGUI resume;
    [SerializeField] TextMeshProUGUI exit;

    private int menuIndex = 0;


    /// <summary>
    /// Called by unity
    /// </summary>
    private void Awake() {
        pauseScreen.SetActive(false);
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

	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(pauseScreen != null){
                pauseScreen.SetActive(!pauseScreen.active);
            }
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
                pauseScreen.SetActive(false);
            }
            else if(menuIndex == 1){
                QuitRound();
            }
        }
	}
}
