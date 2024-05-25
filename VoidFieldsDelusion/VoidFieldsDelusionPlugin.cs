using BepInEx;
using R2API.Utils;
using RoR2;
using System.Diagnostics;
using UnityEngine;

namespace VoidFieldsDelusion
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    public class VoidFieldsDelusionPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "VoidFieldsDelusion";
        public const string PluginVersion = "1.0.0";

        internal static VoidFieldsDelusionPlugin Instance { get; private set; }

        void Awake()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Log.Init(Logger);

            Instance = SingletonHelper.Assign(Instance, this);

            On.RoR2.ArenaMissionController.OnStartServer += ArenaMissionController_OnStartServer;

            stopwatch.Stop();
            Log.Info_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalSeconds:F2} seconds");
        }

        void OnDestroy()
        {
            On.RoR2.ArenaMissionController.OnStartServer -= ArenaMissionController_OnStartServer;

            Instance = SingletonHelper.Unassign(Instance, this);
        }

        static void ArenaMissionController_OnStartServer(On.RoR2.ArenaMissionController.orig_OnStartServer orig, ArenaMissionController self)
        {
            orig(self);

            if (self.nullWards == null || self.nullWards.Length <= 0)
                return;

            GameObject lastWard = self.nullWards[self.nullWards.Length - 1];
            if (lastWard && lastWard.TryGetComponent(out HoldoutZoneController holdoutZoneController))
            {
                holdoutZoneController.applyDelusionResetChests = true;
            }
        }
    }
}
