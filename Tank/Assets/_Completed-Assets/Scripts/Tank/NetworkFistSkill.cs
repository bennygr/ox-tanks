﻿using System;
using UnityEngine;
using UnityEngine.Networking;

namespace AssemblyCSharp.Assets._CompletedAssets.Scripts.Tank {
    class NetworkFistSkill : AbstractNetworkSkill {
        public Rigidbody m_Fist;
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.

        private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool m_Fired;

        [ClientCallback]
        void Update() {
            if (!isLocalPlayer) {
                return;
            }
            if (Input.GetButtonDown(m_FireButton) && CanTrigger()) {
                CmdFire();
                if (m_ShootingAudio) {
                    m_ShootingAudio.Play();
                }

                Triggered();
            }

        }

        [Command]
        private void CmdFire() {
            Rigidbody fistInstance = Instantiate(m_Fist, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            fistInstance.velocity = 40f * m_FireTransform.forward;
            NetworkServer.Spawn(fistInstance.gameObject);
        }
    }
}