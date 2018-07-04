using System;
using UnityEngine;

namespace AssemblyCSharp.Assets._CompletedAssets.Scripts.Tank
{
    public abstract class AbstractSkill : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players.        
        protected String m_FireButton;

        protected void Start()
        {
            m_FireButton = "Skill" + m_PlayerNumber;
        }

        /*
        protected void Update()
        {
            //TODO: Handle some basic stuff here in order to remove code duplciation in conrete skills           
        }
        */
    }
}
