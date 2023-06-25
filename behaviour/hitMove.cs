using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using Utils;

enum HitMoveState
{
    IDLE,
    MOVING,
    HALTED
}

class HitMove : Behaviour
{
    public StateMachine<HitMoveState> sm = new StateMachine<HitMoveState>();

    private double targetX;
    private double targetY;

    public HitMove(Bot bot) : base(bot) { }

    public override void Create(GameStartedEvent e)
    {
        sm.AddState(HitMoveState.IDLE, () => { sm.SetState(HitMoveState.MOVING); });
        sm.AddState(HitMoveState.MOVING, SeekPosition);
        sm.AddState(HitMoveState.HALTED, Spin);

        sm.SetState(HitMoveState.IDLE);
    }

    public override void Update()
    {
        sm.Update();
    }

    public override void Hit(HitByBulletEvent e)
    {
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

        sm.SetState(HitMoveState.HALTED);
    }

    private void Spin()
    {
        bot.TurnRight(10);
    }

    private void ChangePosition()
    {
        if (sm.IsActiveState(HitMoveState.MOVING)) return;

        Random random = new Random();

        targetX = bot.ArenaWidth * random.NextDouble();
        targetY = bot.ArenaHeight * random.NextDouble();

        sm.SetState(HitMoveState.MOVING);
    }
}