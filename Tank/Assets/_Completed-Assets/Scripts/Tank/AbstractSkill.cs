using System;
using UnityEngine;

namespace AssemblyCSharp.Assets._CompletedAssets.Scripts.Tank
{
    public abstract class AbstractSkill : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players.        
        public float cooldown = 0;                    //Cooldown of the skill in seconds
        protected String m_FireButton;

        private DateTime lastTriggered;

        protected void Start()
        {
            m_FireButton = "Skill" + m_PlayerNumber;
        }

        protected void Triggered()
        {
            lastTriggered = System.DateTime.Now;
        }

        protected bool CanTrigger()
        {
            if (cooldown == 0) return true;
            return (System.DateTime.Now - lastTriggered).Seconds > cooldown;
        }
    }
}
