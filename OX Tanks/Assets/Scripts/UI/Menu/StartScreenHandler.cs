using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartScreenHandler : MonoBehaviour
{

    [SerializeField] GameObject[] weaponsPlayer1;
    [SerializeField] GameObject[] weaponsPlayer2;


    int p1Index = 0;
    int p2Index = 1;

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
	}


	/// <summary>
	/// Event Handler if "BtnFight" is clicked
	/// </summary>
	public void OnFightButtonClicked(){
        LoadGameScene();
    }
}
