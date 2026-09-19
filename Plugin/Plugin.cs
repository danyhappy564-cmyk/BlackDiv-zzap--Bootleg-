using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using BlackDiv.Behavior.Layers;
using BlackDiv.Patches;
using System;
using System.Collections.Generic;
using DrakiaXYZ.BigBrain.Brains;
using EFT;
using MoreBotsAPI.Behavior.Layers;
using MoreBotsAPI.Components;

namespace BlackDiv
{
    [BepInDependency("xyz.drakia.bigbrain")]
    [BepInDependency("me.sol.sain", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.morebotsapi.tacticaltoaster")]
    [BepInPlugin(ClientInfo.GUID, ClientInfo.PluginName, ClientInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;

        // BaseUnityPlugin inherits MonoBehaviour, so you can use base unity functions like Awake() and Update()
        private void Awake()
        {
            // save the Logger to variable so we can use it elsewhere in the project
            LogSource = Logger;

            new TarkovInitPatch().Enable();
            //new BotOwnerActivatePatch().Enable();
            //new BotsControllerInitPatch().Enable();
            new BDNvgPatch().Enable();
            new SainBrainLayerPatch().Enable();

            // Only touches SAIN types when SAIN is actually loaded - GetTargetMethod() would
            // throw resolving them otherwise, and SAIN is a soft dependency for us.
            if (Chainloader.PluginInfos.ContainsKey("me.sol.sain"))
            {
                // Config is BaseUnityPlugin's own ConfigFile - no SAIN type token appears in
                // this line. BDPersonalityConfig.Bind's own body is where EPersonality gets
                // referenced, and that method only runs from here, already behind this guard.
                BDPersonalityConfig.Bind(Config);

                new BDSteeringHandoffDiagnostic().Enable();
                new BDUnderFireSteeringFallback().Enable();
                new BDPersonalityOverride().Enable();
            }

            var bdEnums = new List<int> { 848420, 848421, 848422, 848423, 848424, 848426 }
                .ConvertAll(x => (WildSpawnType)x);
            
            MonoBehaviourSingleton<HuntManager>.Instance.AddHuntRoles(bdEnums, [WildSpawnType.pmcUSEC, WildSpawnType.pmcBEAR]);
            
            MonoBehaviourSingleton<HuntManager>.Instance.AddHuntSides(bdEnums, new List<EPlayerSide>()
            { 
                EPlayerSide.Usec,
                EPlayerSide.Bear,
            });
            
            var brainList = new List<string>() { "PMC", "ExUsec", "Assault", "PmcUsec", "PmcBear", "PmcUSEC", "PmcBEAR" };
            var typesList = new List<int>() { 848420, 848421, 848422, 848423, 848424, 848426 }.ConvertAll(x => (WildSpawnType)x);

            BrainManager.AddCustomLayer(typeof(HuntTargetLayer), brainList, 10, typesList);
            BrainManager.RemoveLayers(["AdvAssaultTarget"], brainList, typesList);

            // Safety net: priority 5, below HuntTargetLayer (10) and every SAIN layer, so
            // it only ever activates when neither SAIN nor HuntTargetLayer claimed the bot
            // this tick (no GoalEnemy and no live hunt target). See BDIdlePatrolLayer.cs.
            BrainManager.AddCustomLayer(typeof(BDIdlePatrolLayer), brainList, 5, typesList);
        }
    }
}
