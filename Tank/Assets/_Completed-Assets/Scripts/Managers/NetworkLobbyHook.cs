using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Complete {
	public class NetworkLobbyHook : Prototype.NetworkLobby.LobbyHook {
		public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
			if (lobbyPlayer == null) {
				return;
			}

			Prototype.NetworkLobby.LobbyPlayer lp = lobbyPlayer.GetComponent<Prototype.NetworkLobby.LobbyPlayer>();

			if (lp != null) {
				NetworkGameManager.AddPlayer(gamePlayer, lp.slot, lp.playerColor, lp.nameInput.text);
			}
		}
	}
}