using System;
using UnityEngine;

namespace AssemblyCSharp.Assets._CompletedAssets.Scripts.Tank
{
    public class FistSkill : AbstractSkill
    {
        public GameObject m_Instance;
        public UnityEngine.Object originalFist;

        protected new void Start()
        {
            base.Start();
            //Load prefab by code:
            //Prefab needs to be in the "Resources" subfolder
            originalFist = Resources.Load("Skills/Fist");
        }

        void Update()
        {
            //HOWTO use the input-manager?
            if (Input.GetButtonDown(m_FireButton))
            {
                Rigidbody fistInstance =
                    Instantiate(originalFist,
                                m_FireTransform.position,
                                m_FireTransform.rotation) as Rigidbody;
            }
        }
    }
}
