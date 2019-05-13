using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour {

    public UnityEvent triggerDarkMode;

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

    private static bool darkMode = false;
    private static bool created = false;
    private static RoundManager _instance;
    public static RoundManager instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<RoundManager>();
            }
            return _instance;
        }
    }

    /// <summary>
    ///	    true IF the game is actually running and tanks are fighting, false if
    ///	    in the menu
    /// </summary>
    [SerializeField]
    public bool roundRunning;
    public bool roundFinished;

    public string playerNameWinnerLastRound = "";

    /// <summary>
    ///	    The list of players active and alive in the current round
    /// </summary>
    private List<TankVitals> activePlayers = new List<TankVitals>();

    //mapping player-Ids to current points
    private static Dictionary<int, int> playerPoints = new Dictionary<int, int>();


    //---------------------------------------------------------------------------------------------h

    //"Spawns" can be set from other scenes
    //These get automatically spawned if the round starts
    public static Spawn player1;
    public static Spawn player2;

    //Set to true to request a new round
    public static bool newRound;

    //The current round
    [SerializeField]
    public static int round = 1;
    //The amount of rounds to play for whole game
    public static int roundsPerGame = 5;

    public static int PointsForPlayer(int playerNumber) {
        if (playerPoints.ContainsKey(playerNumber)) {
            return playerPoints[playerNumber];
        }
        return 0;
    }


    //---------------------------------------------------------------------------------------------h

    void Awake() {
        if (!created) {
            Object.DontDestroyOnLoad(this);
            created = true;
        } else {
            Destroy(gameObject);
        }
    }

    private SpawnManager FindSpawnManager() {
        return FindObjectOfType<SpawnManager>();
    }

    // Called by UNITY
    void Start() {

    }

    // Called by UNITY
    void Update() {
        if (newRound) {
            StartRound();
        }
        if (roundRunning && activePlayers != null) {
            //The round ends, if there is only one player left
            if (activePlayers.Count == 1) {
                var winner = activePlayers[0];
                Debug.Log("Round finished. " + winner.PlayerName + " won!");
                playerNameWinnerLastRound = winner.PlayerName;
                playerPoints[winner.PlayerNumber]++;
                roundRunning = false;
                roundFinished = true;
                FindObjectOfType<PowerUpManager>().enabled = false;
                round++;
            }
        }
    }

    /// <summary>
    ///	    Spawns a new player
    /// </summary>
    private void SpawnPlayer(Spawn spawn) {
        if (spawn != null) {
            var spawnManager = FindSpawnManager();
            var player = spawnManager.initialiseTankPrefab(spawn.num, spawn.name, spawn.playerNumber);
            activePlayers.Add(player.GetComponent<TankVitals>());
            //init points if not done yet
            if (!playerPoints.ContainsKey(spawn.playerNumber)) {
                playerPoints[spawn.playerNumber] = 0;
            }
            if (string.Compare(spawn.name, "Ioannis", System.StringComparison.OrdinalIgnoreCase) == 1) {
                Debug.Log("Dark Mode enabled");
                darkMode = true;
            }
            Debug.Log(player.name + " entered the battlefield");
        } else {
            Debug.Log("Cannot spawn player; 'spawn' must not be null");
        }
    }

    /// <summary>
    ///	    Starts a new round - respawning all players
    /// </summary>
    private void StartRound() {
        darkMode = false;
        activePlayers = new List<TankVitals>();
        SpawnPlayer(player1);
        SpawnPlayer(player2);
        roundRunning = true;
        newRound = false;
        roundFinished = false;
        if (darkMode) {
            triggerDarkMode.Invoke();
        }
        FindObjectOfType<PowerUpManager>().enabled = true;
    }

    /// <summary>
    ///     Removes a player from the current round, because he was killed
    /// </summary>
    public void RemovePlayer(int playerNumber) {
        if (activePlayers != null) {
            var player = activePlayers.Find(p => p.PlayerNumber == playerNumber);
            if (player != null) {
                if (activePlayers.Remove(player)) {
                    Debug.Log("OH NOOOOO: " + player.PlayerName + " just died :-(");
                }
            }
        }
    }

    public void Reset() {
        roundRunning = false;
        newRound = false;
        round = 1;
        activePlayers.Clear();
        playerPoints.Clear();
    }
}
