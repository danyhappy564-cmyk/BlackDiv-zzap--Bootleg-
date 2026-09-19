using BlackDiv.Behavior.Actions;
using DrakiaXYZ.BigBrain.Brains;
using EFT;

namespace BlackDiv.Behavior.Layers;

// Safety net for the gap this repo's own README documents (26/09/18 entry):
// SainBrainLayerPatch removes BD/Wedge's vanilla fallback layers so SAIN can run at
// all for them, and HuntTargetLayer (priority 10) is the only thing left once SAIN
// has no GoalEnemy. But HuntTargetLayer itself goes inactive whenever
// BotHuntManager.HasHuntTarget() is false (its hunt target died with no live
// replacement, or none was ever assigned) - and with no vanilla layer left to catch
// that, BigBrain finds nothing active at all for the bot that tick and just holds
// its last action forever (BotBaseBrainUpdatePatch's "no layers are active, return
// null" branch). Not a freeze exactly - nothing tells the bot to do anything new -
// but indistinguishable from one in practice.
//
// This layer is registered at priority 5, below HuntTargetLayer's 10 and every SAIN
// layer, so it only ever wins when nothing else claimed the bot this tick. Its own
// IsActive() is unconditional (alive check only) precisely because it's the last
// entry in the chain - reaching it at all already means everything above it passed.
internal class BDIdlePatrolLayer(BotOwner botOwner, int priority) : CustomLayer(botOwner, priority)
{
    public override string GetName()
    {
        return "BD Idle Patrol (no SAIN target, no hunt target)";
    }

    public override bool IsActive()
    {
        return BotOwner?.HealthController?.IsAlive == true;
    }

    public override Action GetNextAction()
    {
        return new Action(typeof(BDIdlePatrolAction), "NoSAINGoalEnemy_NoHuntTarget");
    }

    public override bool IsCurrentActionEnding()
    {
        // Only one action type here, so there's nothing to switch between - this
        // layer's action only ever ends when BigBrain hands the bot to a
        // higher-priority layer, which the framework handles on its own.
        return false;
    }
}
