using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Complete {
	public class NetworkTankConfig : NetworkBehaviour {

		[Header("Network")]
		[Space]
		[SyncVar]
		public Color color;

		//this is the player number in all of the players
		[SyncVar]
		public int playerNumber;

		[SyncVar]
		public string playerName;

		[SyncVar]
		public bool isReady = false;

        [Header("UI")]
        public Text nameTag;

		public override void OnStartClient() {
			base.OnStartClient();
			if (!isServer) {
				NetworkGameManager.AddPlayer(gameObject, playerNumber, color, playerName);
			}

			GameObject m_TankRenderers = transform.Find("TankRenderers").gameObject;

			// Get all of the renderers of the tank.
			Renderer[] renderers = m_TankRenderers.GetComponentsInChildren<Renderer>();

			// Go through all the renderers...
			for (int i = 0; i < renderers.Length; i++) {
				// ... set their material color to the color specific to this tank.
				renderers[i].material.color = color;
			}

			if (m_TankRenderers) {
				//m_TankRenderers.SetActive(false);
			}
            Debug.Log(playerName);
			nameTag.text = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + playerName + "</color>";
		}

		[ClientCallback]
		public void Update() {
			if (!isLocalPlayer) {
				return;
			}

			if (NetworkGameManager.INSTANCE.m_GameIsFinished && !isReady) {
				if (Input.GetButtonDown("Fire1")) {
					CmdSetReady();
				}
			}
		}

		[Command]
		public void CmdSetReady() {
			isReady = true;
		}

		public override void OnNetworkDestroy() {
			NetworkGameManager.INSTANCE.RemovePlayer(gameObject);
		}
	}
}