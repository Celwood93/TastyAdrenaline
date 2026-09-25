using System;
using BepInEx;
using BepInEx.Configuration;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;

namespace TastyAdrenaline
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    internal class TastyAdrenalinePlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.celwood.tastyadrenaline";
        public const string PluginName = "Tasty Adrenaline";
        public const string PluginVersion = "1.0.1";

        internal static ConfigEntry<float> AdrenalinePerTick { get; private set; }
        internal static ConfigEntry<float> InitialAdrenalineAmount { get; private set; }
        internal static ConfigEntry<float> FinalAdrenalineAmount { get; private set; }
        internal static ConfigEntry<float> AdrenalineMultiplier { get; private set; }

        private void Awake()
        {
            AdrenalinePerTick = Config.Bind(
                "General",
                "AdrenalinePerTick",
                1f,
                "Amount of adrenaline gained on each regular tick.");

            InitialAdrenalineAmount = Config.Bind(
                "General",
                "InitialAdrenalineAmount",
                0f,
                "Amount of adrenaline gained on use of Tasty Mead. Defaults to 0, which uses AdrenalinePerTick.");

            FinalAdrenalineAmount = Config.Bind(
                "General",
                "FinalAdrenalineAmount",
                0f,
                "Amount of adrenaline gained when the effects of Tasty Mead fade. Defaults to 0, which uses AdrenalinePerTick.");

            AdrenalineMultiplier = Config.Bind(
                "General",
                "AdrenalineMultiplier",
                1f,
                "Multiplier applied to other positive adrenaline gains while Tasty Mead is active. This does not affect the adrenaline gained from the Tasty Adrenaline status effect.");

            AdrenalinePatch.Apply();
            Jotunn.Logger.LogInfo("Tasty Adrenaline loaded");
            PrefabManager.OnVanillaPrefabsAvailable += PatchTastyMead;
        }

        private void PatchTastyMead()
        {
            try
            {
                var adrenalineEffect = ScriptableObject.CreateInstance<SE_TastyAdrenaline>();
                adrenalineEffect.name = "SE_TastyAdrenaline";
                ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(adrenalineEffect, false));

                var meadPrefab = PrefabManager.Instance.GetPrefab("MeadTasty");
                if (meadPrefab == null)
                {
                    Jotunn.Logger.LogWarning("[TastyAdrenaline] Tasty mead prefab not found.");
                    return;
                }

                var itemDrop = meadPrefab.GetComponent<ItemDrop>();
                if (itemDrop == null)
                {
                    Jotunn.Logger.LogWarning($"[TastyAdrenaline] {meadPrefab.name} has no ItemDrop component.");
                    return;
                }

                var originalEffect = itemDrop.m_itemData.m_shared.m_consumeStatusEffect;
                var wrapperEffect = ScriptableObject.CreateInstance<SE_TastyWrapper>();
                wrapperEffect.name = "SE_TastyWrapper_Adrenaline";
                wrapperEffect.m_originalSE = originalEffect;
                wrapperEffect.m_extraSE = adrenalineEffect;
                wrapperEffect.m_ttl = originalEffect != null ? originalEffect.m_ttl : adrenalineEffect.m_ttl;

                ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(wrapperEffect, false));
                itemDrop.m_itemData.m_shared.m_consumeStatusEffect = wrapperEffect;
                Jotunn.Logger.LogInfo($"[TastyAdrenaline] Attached adrenaline wrapper to tasty mead: {meadPrefab.name}");
            }
            catch (Exception exception)
            {
                Jotunn.Logger.LogError($"Error patching tasty mead: {exception}");
            }
            finally
            {
                PrefabManager.OnVanillaPrefabsAvailable -= PatchTastyMead;
            }
        }
    }
}