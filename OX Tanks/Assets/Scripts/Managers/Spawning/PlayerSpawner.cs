using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

    public class Spawn {
        public Spawn(int num, string name) {
            this.num = num;
            this.name = name;
        }

        public int num { get; private set; }
        public string name { get; private set; }
    }

    private SpawnManager spawnManager;

    public static Spawn player1;
    public static Spawn player2;


    // Use this for initialization
    void Start() {
        if (spawnManager == null) {
            spawnManager = GetComponent<SpawnManager>();
            if (spawnManager == null) Debug.LogError("Cannot acces SpwanManger");
        }

        if (player1 != null) {
            spawnManager.initialiseTankPrefab(player1.num, player1.name);
        }
        if (player2 != null) {
            spawnManager.initialiseTankPrefab(player2.num, player2.name);
        }
    }

}
