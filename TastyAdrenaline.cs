using System;
using BepInEx;
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
        public const string PluginVersion = "1.0.0";

        private void Awake()
        {
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