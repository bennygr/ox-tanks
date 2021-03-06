using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets._CompletedAssets.Scripts.Tank;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

namespace Complete {
    public class NetworkGameManager : NetworkBehaviour {

        static public NetworkGameManager INSTANCE;
        static public List<NetworkTankManager> tanks = new List<NetworkTankManager>();

        public int m_NumRoundsToWin = 5;            // The number of rounds a single player has to win to win the game.
        public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
        public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
        public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
        public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
        //public PowerUpManager m_PowerUpManager;

        public Transform[] m_SpawnPoint;

        [HideInInspector]
        [SyncVar]
        public bool m_GameIsFinished = false;

        private int m_RoundNumber;                  // Which round the game is currently on.
        private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
        private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
        private NetworkTankManager m_RoundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
        private NetworkTankManager m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.


        /// <summary>
        /// Singleton, set the instance
        /// </summary>
        void Awake() {
            INSTANCE = this;
        }

        [ServerCallback]
        private void Start() {
            // Create the delays so they only have to be made once.
            m_StartWait = new WaitForSeconds(m_StartDelay);
            m_EndWait = new WaitForSeconds(m_EndDelay);

            // Once the tanks have been created and the camera is using them as targets, start the game.
            StartCoroutine(GameLoop());
        }

        /// <summary>
        /// Adds the player.
        /// </summary>
        /// <param name="player">Player.</param>
        /// <param name="playerNumber">Player number.</param>
        /// <param name="color">Color.</param>
        /// <param name="name">Name.</param>
        static public void AddPlayer(GameObject player, int playerNumber, Color color, string name) {
            Debug.LogFormat("Player '{0}' joined the game.", playerNumber);
            NetworkTankManager tankManager = new NetworkTankManager();
            tankManager.instance = player;
            tankManager.playerNumber = playerNumber;
            tankManager.playerColor = color;
            tankManager.playerName = name;
            tankManager.Setup();

            tanks.Add(tankManager);
        }

        /// <summary>
        /// Removes the player.
        /// </summary>
        /// <param name="player">Player.</param>
        public void RemovePlayer(GameObject player) {
            NetworkTankManager toRemove = null;
            foreach (var tank in tanks) {
                if (tank.instance == player) {
                    toRemove = tank;
                    break;
                }
            }

            if (toRemove != null) {
                tanks.Remove(toRemove);
                Debug.LogFormat("Player '{0}' leaves the game.", player);
            }
        }


        // This is called from start and will run each phase of the game one after another.
        private IEnumerator GameLoop() {
            // Wait for at least 2 players
            while (tanks.Count < 2) {
                yield return null;
            }

            // Wait for 2 seconds before starting
            yield return new WaitForSeconds(2.0f);

            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
            yield return StartCoroutine(RoundStarting());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine(RoundPlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            yield return StartCoroutine(RoundEnding());

            // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
            if (m_GameWinner != null) {
                // If there is a game winner, restart the level.
                m_GameIsFinished = true;
                float leftWaitTime = 15.0f;
                bool allAreReady = false;
                int flooredWaitTime = 15;

                while (leftWaitTime > 0.0f && !allAreReady) {
                    yield return null;

                    allAreReady = true;
                    foreach (var tmp in tanks) {
                        allAreReady &= tmp.IsReady();
                    }

                    leftWaitTime -= Time.deltaTime;

                    int newFlooredWaitTime = Mathf.FloorToInt(leftWaitTime);

                    if (newFlooredWaitTime != flooredWaitTime) {
                        flooredWaitTime = newFlooredWaitTime;
                        string message = EndMessage(flooredWaitTime);
                        RpcUpdateMessage(message);
                    }
                }

                LobbyManager.s_Singleton.ServerReturnToLobby();
            } else {
                // If there isn't a winner yet, restart this coroutine so the loop continues.
                // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
                StartCoroutine(GameLoop());
            }
        }


        private IEnumerator RoundStarting() {
            // Notify all clients to start a new round
            RpcStartRound();

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_StartWait;
        }

        [ClientRpc]
        void RpcStartRound() {
            // As soon as the round starts reset the tanks and make sure they can't move.
            ResetAllTanks();
            DisableTankControl();

            // Snap the camera's zoom and position to something appropriate for the reset tanks.
            //m_CameraControl.SetStartPositionAndSize(); FIXME: Maybe not needed anymore since we will be implementing a chase camera

            // Increment the round number and display text showing the players what round it is.
            m_RoundNumber++;
            m_MessageText.text = "ROUND " + m_RoundNumber;

            StartCoroutine(ClientRoundStartingFade());
        }

        // TODO implement fade in
        private IEnumerator ClientRoundStartingFade() {
            float elapsedTime = 0.0f;
            float wait = m_StartDelay - 0.5f;

            yield return null;

            while (elapsedTime < wait) {
                elapsedTime += Time.deltaTime;

                //sometime, synchronization lag behind because of packet drop, so we make sure our tank are reseted
                if (elapsedTime / wait < 0.5f) {
                    ResetAllTanks();
                }

                yield return null;
            }
        }


        private IEnumerator RoundPlaying() {
            // Notify all clients to start playing
            RpcPlayRound();

            // While there is not one tank left...
            while (!OneTankLeft()) {
                // ... return on the next frame.
                yield return null;
            }
        }

        [ClientRpc]
        void RpcPlayRound() {
            // As soon as the round begins playing let the players control the tanks.
            EnableTankControl();

            //Disable powerup spawning
            //m_PowerUpManager.Enabled = true;

            // Clear the text from the screen.
            m_MessageText.text = string.Empty;
        }


        private IEnumerator RoundEnding() {
            // Clear the winner from the previous round.
            m_RoundWinner = null;

            // See if there is a winner now the round is over.
            m_RoundWinner = GetRoundWinner();

            // If there is a winner, increment their score.
            if (m_RoundWinner != null) {
                m_RoundWinner.wins++;
            }

            // Now the winner's score has been incremented, see if someone has one the game.
            m_GameWinner = GetGameWinner();

            // Get a message based on the scores and whether or not there is a game winner and display it.
            RpcUpdateMessage(EndMessage(0));

            //notify client they should disable tank control
            RpcEndRound();

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_EndWait;
        }

        [ClientRpc]
        void RpcEndRound() {
            // Stop tanks from moving.
            DisableTankControl();

            //Disable powerup spawning
            //m_PowerUpManager.Enabled = false;

            StartCoroutine(ClientRoundEndingFade());
        }


        [ClientRpc]
        private void RpcUpdateMessage(string msg) {
            m_MessageText.text = msg;
        }

        //TODO implement fade out
        private IEnumerator ClientRoundEndingFade() {
            float elapsedTime = 0.0f;
            float wait = m_EndDelay;
            while (elapsedTime < wait) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }


        // This is used to check if there is one or fewer tanks remaining and thus the round should end.
        private bool OneTankLeft() {
            // Start the count of tanks left at zero.
            int numTanksLeft = 0;

            // Go through all the tanks...
            for (int i = 0; i < tanks.Count; i++) {
                // Check if there are tanks above 0 health
                if (tanks[i].tankHealth.m_CurrentHealth > 0) {
                    numTanksLeft++;
                }
            }

            // If there are one or fewer tanks remaining return true, otherwise return false.
            return numTanksLeft <= 1;
        }


        // This function is to find out if there is a winner of the round.
        // This function is called with the assumption that 1 or fewer tanks are currently active.
        private NetworkTankManager GetRoundWinner() {
            // Go through all the tanks...
            for (int i = 0; i < tanks.Count; i++) {
                if (tanks[i].tankHealth.m_CurrentHealth > 0) {
                    return tanks[i];
                }
            }

            // If none of the tanks are active it is a draw so return null.
            return null;
        }


        // This function is to find out if there is a winner of the game.
        private NetworkTankManager GetGameWinner() {
            // Go through all the tanks...
            for (int i = 0; i < tanks.Count; i++) {
                // ... and if one of them has enough rounds to win the game, return it.
                if (tanks[i].wins == m_NumRoundsToWin) {
                    return tanks[i];
                }
            }

            // If no tanks have enough rounds to win, return null.
            return null;
        }


        // Returns a string message to display at the end of each round.
        private string EndMessage(int waitTime) {
            // By default when a round ends there are no winners so the default end message is a draw.
            string message = "DRAW!";

            // If there is a game winner set the message to say which player has won the game.
            if (m_GameWinner != null) {
                message = "<color=#" + ColorUtility.ToHtmlStringRGB(m_GameWinner.playerColor) + ">" + m_GameWinner.playerName + "</color> WINS THE GAME!";
            } else if (m_RoundWinner != null) {
                // Or just display the round winner
                message = "<color=#" + ColorUtility.ToHtmlStringRGB(m_RoundWinner.playerColor) + ">" + m_RoundWinner.playerName + "</color> WINS THE ROUND!";
            }

            // Add some line breaks after the initial message.
            message += "\n\n\n\n";

            // Go through all the tanks and add each of their scores to the message.
            for (int i = 0; i < tanks.Count; i++) {
                message += "<color=#" + ColorUtility.ToHtmlStringRGB(tanks[i].playerColor) + ">" + tanks[i].playerName + "</color>: " + tanks[i].wins + " WINS " + (tanks[i].IsReady() ? "<size=15>READY</size>" : "") + " \n";
            }

            // If there is a game winner, change the entire message to reflect that.
            if (m_GameWinner != null) {
                message += "\n\n<size=20 > Return to lobby in " + waitTime + "\nPress Fire to get ready</size>";
            }
            return message;
        }


        // This function is used to turn all the tanks back on and reset their positions and properties.
        private void ResetAllTanks() {
            for (int i = 0; i < tanks.Count; i++) {
                tanks[i].spawnPoint = m_SpawnPoint[tanks[i].tankConfig.playerNumber];
                tanks[i].Reset();
            }
        }


        private void EnableTankControl() {
            for (int i = 0; i < tanks.Count; i++) {
                tanks[i].EnableControl();
            }
        }


        private void DisableTankControl() {
            for (int i = 0; i < tanks.Count; i++) {
                tanks[i].DisableControl();
            }
        }
    }
}