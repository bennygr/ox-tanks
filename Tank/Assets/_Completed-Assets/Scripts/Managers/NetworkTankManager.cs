using System;
using AssemblyCSharp.Assets._CompletedAssets.Scripts.Tank;
using UnityEngine;

namespace Complete {
	[Serializable]
	public class NetworkTankManager {
		// This class is to manage various settings on a tank.
		// It works with the GameManager class to control how the tanks behave
		// and whether or not players have control of their tank in the 
		// different phases of the game.

		public Color playerColor;                             // This is the color this tank will be tinted.
		public Transform spawnPoint;                          // The position and direction the tank will have when it spawns.
		[HideInInspector] public int playerNumber;            // This specifies which player this the manager for.
		[HideInInspector] public GameObject instance;         // A reference to the instance of the tank when it is created.
		[HideInInspector] public int wins;                    // The number of wins this player has so far.
        [HideInInspector] public GameObject tankRenderers;        // The transform that is a parent of all the tank's renderers.  This is deactivated when the tank is dead.
		[HideInInspector] public string playerName;           // The player name set in the lobby (TODO: Lobby)

		public NetworkTankConfig tankConfig;
        public NetworkTankMovement tankMovement;                        // Reference to tank's movement script, used to disable and enable control.
        public NetworkTankShooting tankShooting;                        // Reference to tank's shooting script, used to disable and enable control.
		public NetworkTankHealth tankHealth;                            // Reference to tank's health script
        public AbstractNetworkSkill tankSkill;                              // Reference to tank's skill script
        private GameObject healthCanvas;                  // Used to disable the world space UI during the Starting and Ending phases of each round.


		public void Setup() {
            Debug.LogFormat("Setting up player '{0}' as player {1}", playerName, playerNumber);
			// Get references to the components.
			tankMovement = instance.GetComponent<NetworkTankMovement>();
			tankShooting = instance.GetComponent<NetworkTankShooting>();
			tankHealth = instance.GetComponent<NetworkTankHealth>();
			tankSkill = instance.GetComponent<AbstractNetworkSkill>();
			tankConfig = instance.GetComponent<NetworkTankConfig>();
			healthCanvas = instance.GetComponentInChildren<Canvas>().gameObject;

			// Get references to the child objects.
            tankRenderers = tankHealth.m_TankRenderers;

			//Set a reference to that amanger in the health script, to disable control when dying
			tankHealth.m_TankManager = this;

			// Set the player numbers to be consistent across the scripts.
			tankMovement.m_PlayerNumber = playerNumber;
			tankShooting.m_PlayerNumber = playerNumber;
			tankSkill.m_PlayerNumber = playerNumber;

			tankConfig.color = playerColor;
			tankConfig.playerName = playerName;
			tankConfig.playerNumber = playerNumber;
		}

		public string GetName() {
			return tankConfig.playerName;
        }

		public bool IsReady() {
			return tankConfig.isReady;
        }

		// Used during the phases of the game where the player shouldn't be able to control their tank.
		public void DisableControl() {
			tankMovement.enabled = false;
			tankShooting.enabled = false;
			tankSkill.enabled = false;
		}


		// Used during the phases of the game where the player should be able to control their tank.
		public void EnableControl() {
			tankMovement.enabled = true;
			tankShooting.enabled = true;
			tankSkill.enabled = true;

			tankMovement.ReEnableParticles();
			healthCanvas.SetActive(true);
		}


		// Used at the start of each round to put the tank into it's default state.
		public void Reset() {
			tankMovement.SetDefaults();
			tankShooting.SetDefaults();
			tankHealth.SetDefaults();

            if (tankMovement.hasAuthority) {
                Debug.LogFormat("Authoriative respawn from client '{0}' for player '{1}' in spawn position '{2}'", tankMovement.netId, playerNumber, spawnPoint.position);
				instance.transform.position = spawnPoint.position;
				instance.transform.rotation = spawnPoint.rotation;
			}
		}
	}
}