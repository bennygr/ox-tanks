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

		public Color m_PlayerColor;                             // This is the color this tank will be tinted.
		public Transform m_SpawnPoint;                          // The position and direction the tank will have when it spawns.
		[HideInInspector] public int m_PlayerNumber;            // This specifies which player this the manager for.
		[HideInInspector] public string m_ColoredPlayerText;    // A string that represents the player with their number colored to match their tank.
		[HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.
		[HideInInspector] public int m_Wins;                    // The number of wins this player has so far.
		[HideInInspector] public GameObject m_TankRenderers;        // The transform that is a parent of all the tank's renderers.  This is deactivated when the tank is dead.
		[HideInInspector] public string m_PlayerName;           // The player name set in the lobby (TODO: Lobby)

		public NetworkTankConfig m_TankConfig;
		public NetworkTankMovement m_Movement;                        // Reference to tank's movement script, used to disable and enable control.
		public NetworkTankShooting m_Shooting;                        // Reference to tank's shooting script, used to disable and enable control.
		public NetworkTankHealth m_Health;                            // Reference to tank's health script
		public AbstractSkill m_Skill;                              // Reference to tank's skill script
		private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.


		public void Setup() {
			// Get references to the components.
			m_Movement = m_Instance.GetComponent<NetworkTankMovement>();
			m_Shooting = m_Instance.GetComponent<NetworkTankShooting>();
			m_Health = m_Instance.GetComponent<NetworkTankHealth>();
			m_Skill = m_Instance.GetComponent<AbstractSkill>();
			m_TankConfig = m_Instance.GetComponent<NetworkTankConfig>();
			m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

			// Set the player numbers to be consistent across the scripts.
			m_Movement.m_PlayerNumber = m_PlayerNumber;
			m_Shooting.m_PlayerNumber = m_PlayerNumber;
			m_Skill.m_PlayerNumber = m_PlayerNumber;

			m_TankConfig.color = m_PlayerColor;
			m_TankConfig.playerName = m_PlayerName;
			m_TankConfig.playerNumber = m_PlayerNumber;

			// Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
			m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

			// Get all of the renderers of the tank.
			MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

			// Go through all the renderers...
			for (int i = 0; i < renderers.Length; i++) {
				// ... set their material color to the color specific to this tank.
				renderers[i].material.color = m_PlayerColor;
			}
		}

		public string GetName() {
			return m_TankConfig.playerName;
        }

		public bool IsReady() {
			return m_TankConfig.isReady;
        }

		// Used during the phases of the game where the player shouldn't be able to control their tank.
		public void DisableControl() {
			m_Movement.enabled = false;
			m_Shooting.enabled = false;
			m_Skill.enabled = false;
		}


		// Used during the phases of the game where the player should be able to control their tank.
		public void EnableControl() {
			m_Movement.enabled = true;
			m_Shooting.enabled = true;
			m_Skill.enabled = true;

			m_CanvasGameObject.SetActive(true);
		}


		// Used at the start of each round to put the tank into it's default state.
		public void Reset() {
			m_Movement.SetDefaults();
			m_Shooting.SetDefaults();
			m_Health.SetDefaults();

			if (m_Movement.hasAuthority) {
				m_Instance.transform.position = m_SpawnPoint.position;
				m_Instance.transform.rotation = m_SpawnPoint.rotation;
			}
		}
	}
}