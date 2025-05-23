﻿using HarmonyLib;
using ProjectM;
using Unity.Collections;
using Unity.Entities;

namespace Bloodcraft.Patches;

[HarmonyPatch]
internal static class UnitSpawnerPatch
{
    [HarmonyPatch(typeof(UnitSpawnerReactSystem), nameof(UnitSpawnerReactSystem.OnUpdate))]
    [HarmonyPrefix]
    static void OnUpdatePrefix(UnitSpawnerReactSystem __instance)
    {
        if (!Core._initialized) return;

        NativeArray<Entity> entities = __instance.EntityQueries[0].ToEntityArray(Allocator.Temp);
        try
        {
            foreach (Entity entity in entities)
            {
                if (entity.Has<IsMinion>())
                {
                    entity.With((ref IsMinion isMinion) =>
                    {
                        isMinion.Value = true;
                    });
                }
            }
        }
        finally
        {
            entities.Dispose();
        }
    }
}
