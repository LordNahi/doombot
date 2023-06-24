using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

enum TankState
{
    IDLE,
    SEEKING_POSITION,
    EXTERMINATING
}

public class Doom : Bot
{
    int enemyCount;
    double targetX;
    double targetY;
    int speed = 30;
    TankState state = TankState.IDLE;

    static void Main(string[] args)
    {
        new Doom().Start();
    }

    Doom() : base(BotInfo.FromFile("Doom.json")) { }

    public override void Run()
    {
        SetUp();

        while (IsRunning)
        {
            switch (state)
            {
                case TankState.IDLE:
                    state = TankState.SEEKING_POSITION;
                    break;
                case TankState.SEEKING_POSITION:
                    SeekPosition();
                    break;
                case TankState.EXTERMINATING:
                    Exterminate();
                    break;
                default:
                    state = TankState.IDLE;
                    break;
            }
        }
    }

    private void SetUp()
    {
        state = TankState.IDLE;

        AdjustGunForBodyTurn = false;
        AdjustRadarForGunTurn = false;

        BulletColor = Color.Yellow;
        BodyColor = Color.Green;
        TurretColor = Color.Gray;
        RadarColor = Color.Red;
        ScanColor = Color.Red;

        enemyCount = EnemyCount;

        targetX = ArenaWidth / 2;
        targetY = ArenaHeight / 2;
    }

    private void SeekPosition()
    {
        var tx = targetX - X;
        var ty = targetY - Y;

        TurnLeft(-Direction);

        double angleToPosition = Math.Atan2(ty, tx) * 180 / Math.PI;

        TurnLeft(angleToPosition);

        double distanceToPosition = Math.Sqrt(Math.Pow(tx, 2) + Math.Pow(ty, 2));

        Forward(distanceToPosition);

        state = TankState.EXTERMINATING;
    }

    private void Exterminate()
    {
        TurnRight(10);
    }

    private void ChangePosition()
    {
        if (state == TankState.SEEKING_POSITION) return;

        Random random = new Random();

        targetX = ArenaWidth * random.NextDouble();
        targetY = ArenaHeight * random.NextDouble();

        state = TankState.SEEKING_POSITION;
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        if (state == TankState.EXTERMINATING)
        {
            Fire(3);
        }
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        ChangePosition();
    }

    public override void OnDeath(DeathEvent e)
    {
        Console.WriteLine("No ragrets.");
    }
}