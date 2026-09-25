using HarmonyLib;

namespace TastyAdrenaline
{
    internal static class AdrenalinePatch
    {
        private static bool isMultiplierActive;
        private static bool isPotionGainActive;
        private static Harmony harmony;

        internal static void Apply()
        {
            harmony = new Harmony(TastyAdrenalinePlugin.PluginGuid);
            harmony.PatchAll(typeof(AdrenalinePatch).Assembly);
        }

        internal static void SetIsActive(bool isActive)
        {
            isMultiplierActive = isActive;
        }

        internal static void AddPotionGain(float v)
        {
            isPotionGainActive = true;
            try
            {
                Player.m_localPlayer.AddAdrenaline(v);
            }
            finally
            {
                isPotionGainActive = false;
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.AddAdrenaline))]
        private static class PlayerAddAdrenalinePatch
        {
            private static void Prefix(ref float v)
            {
                if (!isMultiplierActive || isPotionGainActive || v <= 0f)
                {
                    return;
                }

                v *= TastyAdrenalinePlugin.AdrenalineMultiplier.Value;
            }
        }
    }
}