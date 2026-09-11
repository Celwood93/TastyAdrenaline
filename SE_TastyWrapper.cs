using UnityEngine;

namespace TastyAdrenaline
{
    public class SE_TastyWrapper : StatusEffect
    {
        public StatusEffect m_originalSE;
        public StatusEffect m_extraSE;

        public override void Setup(Character character)
        {
            base.Setup(character);
            if (character == null)
            {
                return;
            }

            if (m_originalSE != null)
            {
                character.GetSEMan().AddStatusEffect(m_originalSE, resetTime: true);
            }

            if (m_extraSE != null)
            {
                character.GetSEMan().AddStatusEffect(m_extraSE, resetTime: true);
            }
        }
    }
}