using System;
using UnityEngine;
using UnityEngine.Networking;

namespace AssemblyCSharp.Assets._CompletedAssets.Scripts.Tank {

    public class NetworkMineSkill : AbstractNetworkSkill {
    
        public Transform m_MineTransformation;     // A child of the tank where the shells are spawned.
        public Rigidbody m_Mine;                   // Prefab of the shell.    

        [ClientCallback]
        void Update() {
            if (!isLocalPlayer) {
                return;
            }
            if (Input.GetButtonDown(m_FireButton) && CanTrigger()) {
                CmdFire();
                Triggered();
            }
        }

        [Command]
        private void CmdFire() {
            Rigidbody fistInstance = Instantiate(m_Mine, m_MineTransformation.position, m_MineTransformation.rotation) as Rigidbody;
            NetworkServer.Spawn(fistInstance.gameObject);
        }
    }
}
