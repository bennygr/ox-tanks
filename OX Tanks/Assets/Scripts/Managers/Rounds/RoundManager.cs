using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {

    public class Spawn {

        public Spawn(int num, string name, int playerNumber) {
            this.num = num;
            this.name = name;
            this.playerNumber = playerNumber;
        }

        public int num { get; private set; }
        public string name { get; private set; }
        public int playerNumber { get; private set; }
    }

    /// <summary>
    ///	    Reference to the SpawnManager
    /// </summary>
    private SpawnManager spawnManager;

    /// <summary>
    ///	    The amount of rounds to play for one game
    /// </summary>
    private int roundPerGame = 5;

    /// <summary>
    ///	    The current round
    /// </summary>
    private int round;

    /// <summary>
    ///	    The list of players active and alive in the current round
    /// </summary>
    private IList<TankVitals> players = new List<TankVitals>();

    //"Spawns" can be set from other scenes
    //These get automatically spawned if the round starts
    public static Spawn player1;
    public static Spawn player2;

    // Called by UNITY
    void Start () {
        if (spawnManager == null) {
            spawnManager = GetComponent<SpawnManager>();
            if (spawnManager == null) Debug.LogError("Cannot access SpawnManager");
        }
        //Start a new Game
        StartGame();
    }

    // Called by UNITY
    void Update () {

    }

    private GameObject SpawnPlayer(Spawn player){
        return spawnManager.initialiseTankPrefab(player.num, player.name, player.playerNumber);
    }

    public void StartGame(){
       round = 0;
       StartRound();
    }

    public void StartRound(){
       players = new List<TankVitals>();
       if(player1 != null){
            GameObject p1 = SpawnPlayer(player1);
            GameObject p2 = SpawnPlayer(player2);
            Debug.Log(player1);
            Debug.Log(player1);
       }
    }
}
