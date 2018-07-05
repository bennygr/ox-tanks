using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankHealth : AbstractGameHealth
    {
        public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
        public Slider m_ShieldSlider;                             // The slider to represent how much health the tank currently has.
        public Image m_FillImage;                           // The image component of the slider.
        public Image m_FillShieldImage;                           // The image component of the slider.
        public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
        public Color m_ZeroHealthColor = Color.red;
        public Color m_FullShieldColor = Color.blue;
        public Color m_ZeroShieldColor = Color.clear;

        private void Awake()
        {
            base.Init();
        }

        private void OnEnable()
        {
            base.Enable();
            SetHealthUI();
        }

        public void TakeDamage(float amount)
        {
            base.ApplyDamage(amount);
            SetHealthUI();
            base.CalculateDeath();
        }

        public void Heal(float amount)
        {
            CurrentHealth = Mathf.Min(m_StartingHealth, CurrentHealth + amount);
            SetHealthUI();
        }

        public void AddShield(float amount)
        {
            m_CurrentShield = Mathf.Min(m_MaxShield, m_CurrentShield + amount);
            SetHealthUI();
        }

        private void OnDeath()
        {
            base.ApplyDeath();
        }

        private void SetHealthUI()
        {
            // Set the slider's value appropriately.
            m_Slider.value = CurrentHealth;

            // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
            m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, CurrentHealth / m_StartingHealth);


            if (m_ShieldSlider != null)
            {
                m_ShieldSlider.value = m_CurrentShield;
            }

            if (m_FillShieldImage != null)
            {
                m_FillShieldImage.color = Color.Lerp(m_ZeroShieldColor, m_FullShieldColor, m_StartingShield);
            }
        }
    }
}