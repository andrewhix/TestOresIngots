using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace TestOresIngots;

internal sealed record CustomMineRockDefinition(
    string PrefabName,
    string DropItemPrefabName,
    float DropChance,
    int StackMin,
    int StackMax,
    int RequiredToolTier);

internal static class CustomMineRockRegistry
{
    private static readonly List<CustomMineRockDefinition> Definitions = new();

    public static void Register(string prefabName, string dropItemPrefabName, float dropChance, int stackMin, int stackMax, int requiredToolTier)
    {
        if (string.IsNullOrWhiteSpace(prefabName) || string.IsNullOrWhiteSpace(dropItemPrefabName))
        {
            return;
        }

        int sanitizedMin = Mathf.Max(1, stackMin);
        int sanitizedMax = Mathf.Max(sanitizedMin, stackMax);
        float sanitizedChance = Mathf.Clamp01(dropChance);
        int sanitizedTier = Mathf.Max(0, requiredToolTier);

        Definitions.RemoveAll(definition => definition.PrefabName == prefabName);
        Definitions.Add(new CustomMineRockDefinition(prefabName, dropItemPrefabName, sanitizedChance, sanitizedMin, sanitizedMax, sanitizedTier));
    }

    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static class ZNetSceneAwakePatch
    {
        private static void Postfix(ZNetScene __instance)
        {
            foreach (CustomMineRockDefinition definition in Definitions)
            {
                ConfigureMineRock(__instance, definition);
            }
        }
    }

    private static void ConfigureMineRock(ZNetScene scene, CustomMineRockDefinition definition)
    {
        GameObject? rockPrefab = scene.GetPrefab(definition.PrefabName);
        if (rockPrefab == null)
        {
            TestOresIngotsPlugin.TestOresIngotsLogger.LogWarning($"Unable to find mine rock prefab '{definition.PrefabName}' when applying drop configuration.");
            return;
        }

        GameObject? dropPrefab = scene.GetPrefab(definition.DropItemPrefabName);
        if (dropPrefab == null)
        {
            TestOresIngotsPlugin.TestOresIngotsLogger.LogWarning($"Unable to find drop prefab '{definition.DropItemPrefabName}' for mine rock '{definition.PrefabName}'.");
            return;
        }

        ItemDrop? dropItem = dropPrefab.GetComponent<ItemDrop>();
        if (dropItem == null)
        {
            TestOresIngotsPlugin.TestOresIngotsLogger.LogWarning($"Prefab '{definition.DropItemPrefabName}' is missing ItemDrop component and cannot be added to mine rock '{definition.PrefabName}'.");
            return;
        }

        DropTable dropTable = rockPrefab.GetComponent<DropTable>() ?? rockPrefab.AddComponent<DropTable>();
        dropTable.m_dropChance = definition.DropChance;
        dropTable.m_dropMin = definition.StackMin;
        dropTable.m_dropMax = definition.StackMax;
        dropTable.m_oneOfEach = false;
        dropTable.m_drops = new List<DropTable.DropData>
        {
            new()
            {
                m_item = dropPrefab,
                m_stackMin = definition.StackMin,
                m_stackMax = definition.StackMax,
                m_weight = 1f,
                m_dropChance = 1f
            }
        };

        MineRock? mineRock = rockPrefab.GetComponent<MineRock>();
        if (mineRock != null)
        {
            FieldInfo? dropItemsField = AccessTools.Field(mineRock.GetType(), "m_dropItems");
            dropItemsField?.SetValue(mineRock, dropTable);

            FieldInfo? dropItemField = AccessTools.Field(mineRock.GetType(), "m_dropItem");
            if (dropItemField != null)
            {
                object? value = dropItemField.FieldType == typeof(GameObject) ? dropPrefab : dropItemField.FieldType.IsInstanceOfType(dropItem) ? dropItem : null;
                if (value != null)
                {
                    dropItemField.SetValue(mineRock, value);
                }
            }

            FieldInfo? minToolTierField = AccessTools.Field(mineRock.GetType(), "m_minToolTier");
            minToolTierField?.SetValue(mineRock, definition.RequiredToolTier);
        }

        Destructible? destructible = rockPrefab.GetComponent<Destructible>();
        if (destructible != null)
        {
            destructible.m_minToolTier = definition.RequiredToolTier;
        }
    }
}
