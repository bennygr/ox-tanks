using System;
using UnityEngine;
using UnityEngine.Networking;

namespace AssemblyCSharp.Assets._CompletedAssets.Scripts.Tank {
    
    public abstract class AbstractNetworkSkill : NetworkBehaviour {
        public int m_PlayerNumber = 1;              // Used to identify the different players.        
        public float cooldown = 0;                    //Cooldown of the skill in seconds
        protected String m_FireButton;

        private DateTime lastTriggered;

        protected void Start() {
            m_FireButton = "Skill1";
        }

        protected void Triggered() {
            lastTriggered = System.DateTime.Now;
        }

        protected bool CanTrigger() {
            if (cooldown == 0) {
                Debug.Log("Skill triggered");
                return true;
            }
            Debug.Log("Skill on coolddown");
            return (System.DateTime.Now - lastTriggered).Seconds > cooldown;
        }
    }
}
