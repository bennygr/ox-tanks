using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenHandler : MonoBehaviour {



    private void LoadGameScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Sandbox");
    }

    /// <summary>
    /// Event Handler if "BtnFight" is clicked
    /// </summary>
    public void OnFightButtonClicked(){
        Debug.Log("FIGHT button clicked");
        LoadGameScene();
    }
}
