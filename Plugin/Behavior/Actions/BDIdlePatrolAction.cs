using DrakiaXYZ.BigBrain.Brains;
using EFT;

namespace BlackDiv.Behavior.Actions;

// Runs vanilla's own patrol node, exactly the way MoreBotsAPI's own
// SearchForTargetAction does it (same AIActionsList.CreateNode call, same
// BotLogicDecision.simplePatrol / followerPatrol split by boss/follower) - copied
// pattern, not guessed, since that class already proves this works for these bots.
// The only difference: no huntManager to time out against, since this action isn't
// part of the hunt system at all - it just runs until BDIdlePatrolLayer stops being
// the active layer (SAIN or HuntTargetLayer reclaiming the bot).
public class BDIdlePatrolAction : CustomLogic
{
    private readonly AICoreNode baseAction;

    public BDIdlePatrolAction(BotOwner botOwner) : base(botOwner)
    {
        baseAction = botOwner.Boss.IamBoss
            ? AIActionsList.CreateNode(BotLogicDecision.simplePatrol, botOwner)
            : AIActionsList.CreateNode(BotLogicDecision.followerPatrol, botOwner);
    }

    public override void Update(CustomLayer.ActionData data)
    {
        baseAction.UpdateNodeByMain(data);
    }
}
