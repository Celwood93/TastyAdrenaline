using System.Collections;
using System.Text;
using UnityEngine;

namespace TastyAdrenaline
{
    public class SE_TastyAdrenaline : SE_Stats
    {
        private const int TickIntervalSeconds = 2;
        private bool isRunning;

        public SE_TastyAdrenaline()
        {
            m_ttl = 10f;
            m_name = "SE_TastyAdrenaline";
            m_tooltip = "Slowly gain 1 adrenaline every 2 seconds while active.";
        }

        public override void Setup(Character character)
        {
            base.Setup(character);
            if (!(character is Player player))
            {
                return;
            }

            isRunning = true;
            player.StartCoroutine(GainAdrenalineOverTime(player));
        }

        public override void Stop()
        {
            isRunning = false;
            base.Stop();
        }

        public override string GetTooltipString()
        {
            var tooltip = new StringBuilder(128);
            if (!string.IsNullOrEmpty(m_tooltip))
            {
                tooltip.AppendLine(m_tooltip);
            }

            tooltip.AppendFormat("<color=orange>Gain 1 adrenaline every {0}s for {1}s</color>", TickIntervalSeconds, m_ttl);
            return tooltip.ToString();
        }

        private IEnumerator GainAdrenalineOverTime(Player player)
        {
            var tickCount = Mathf.Max(1, Mathf.FloorToInt(m_ttl / TickIntervalSeconds));
            for (var tick = 0; tick < tickCount && isRunning && player != null; tick++)
            {
                player.AddAdrenaline(1f);
                if (tick < tickCount - 1)
                {
                    yield return new WaitForSeconds(TickIntervalSeconds);
                }
            }

            isRunning = false;
        }
    }
}