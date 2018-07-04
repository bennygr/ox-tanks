using System;
using UnityEngine;

namespace AssemblyCSharp.Assets._CompletedAssets.Scripts.Tank
{
    public class MineSkill : AbstractSkill
    {
        public Transform m_MineTransformation;     // A child of the tank where the shells are spawned.
        public Rigidbody m_Mine;                   // Prefab of the shell.    

        void Update()
        {
            if (Input.GetButtonDown(m_FireButton) && CanTrigger())
            {
                Rigidbody fistInstance =
                    Instantiate(m_Mine, m_MineTransformation.position, m_MineTransformation.rotation) as Rigidbody;
                Triggered();
            }
        }
    }
}
