using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

enum HitMoveState
{
    IDLE,
    MOVING,
    HALTED
}

class HitMove : Behaviour
{
    private HitMoveState state;
    private double targetX;
    private double targetY;
    private int painTolerance = 4;
    private int pain = 0;

    public HitMove(Bot bot) : base(bot) { }

    public override void Update()
    {
        switch (state)
        {
            case HitMoveState.IDLE:
                state = HitMoveState.MOVING;
                break;
            case HitMoveState.MOVING:
                SeekPosition();
                break;
            default:
                state = HitMoveState.IDLE;
                break;
        }
    }

    public override void Hit(HitByBulletEvent e)
    {
        pain++;

        if (state != HitMoveState.HALTED) return;
        if (pain < painTolerance) return;

        pain = 0;

        ChangePosition();
    }

    private void SeekPosition()
    {
        var tx = targetX - bot.X;
        var ty = targetY - bot.Y;

        bot.TurnLeft(-bot.Direction);

        double angleToPosition = Math.Atan2(ty, tx) * 180 / Math.PI;

        bot.TurnLeft(angleToPosition);

        double distanceToPosition = Math.Sqrt(Math.Pow(tx, 2) + Math.Pow(ty, 2));

        bot.Forward(distanceToPosition);

        state = HitMoveState.HALTED;
    }

    private void ChangePosition()
    {
        if (state == HitMoveState.MOVING) return;

        Random random = new Random();

        targetX = bot.ArenaWidth * random.NextDouble();
        targetY = bot.ArenaHeight * random.NextDouble();

        state = HitMoveState.MOVING;
    }
}