using System.Collections;
using System.Text;
using UnityEngine;

namespace TastyAdrenaline
{
    public class SE_TastyAdrenaline : SE_Stats
    {
        private const float TickIntervalSeconds = 1.95f;
        private bool isRunning;

        public SE_TastyAdrenaline()
        {
            m_ttl = 10f;
            m_name = "SE_TastyAdrenaline";
            m_tooltip = "Slowly gain adrenaline instantly and every 2 seconds while active.";
        }

        public override void Setup(Character character)
        {
            base.Setup(character);
            if (!(character is Player player) || player != Player.m_localPlayer)
            {
                return;
            }

            AdrenalinePatch.SetIsActive(true);
            isRunning = true;
            player.StartCoroutine(GainAdrenalineOverTime(player));
        }

        public override void Stop()
        {
            isRunning = false;
            AdrenalinePatch.SetIsActive(false);
            base.Stop();
        }

        public override string GetTooltipString()
        {
            var tooltip = new StringBuilder(128);
            if (!string.IsNullOrEmpty(m_tooltip))
            {
                tooltip.AppendLine(m_tooltip);
            }

            tooltip.AppendFormat(
                "<color=orange>Gaining adrenaline over {3}s</color>",
                m_ttl);
            return tooltip.ToString();
        }

        private float GetAdrenalineAmountForTick(int tickIndex, int tickCount)
        {
            if (tickIndex == tickCount - 1)
            {
                if (TastyAdrenalinePlugin.FinalAdrenalineAmount.Value > 0f)
                {
                    return TastyAdrenalinePlugin.FinalAdrenalineAmount.Value;
                }
            }

            if (tickIndex == 0)
            {
                if (TastyAdrenalinePlugin.InitialAdrenalineAmount.Value > 0f)
                {
                    return TastyAdrenalinePlugin.InitialAdrenalineAmount.Value;
                }
            }

            return TastyAdrenalinePlugin.AdrenalinePerTick.Value;
        }

        private IEnumerator GainAdrenalineOverTime(Player player)
        {
            var tickCount = Mathf.Max(1, Mathf.CeilToInt(m_ttl / TickIntervalSeconds));
            for (var tick = 0; tick < tickCount && isRunning && player != null; tick++)
            {
                AdrenalinePatch.AddPotionGain(GetAdrenalineAmountForTick(tick, tickCount));
                if (tick < tickCount-1)
                {
                    yield return new WaitForSeconds(TickIntervalSeconds);
                }
            }

            isRunning = false;
        }
    }
}