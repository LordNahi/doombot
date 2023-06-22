using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

enum TankState
{
    IDLE,
    SEEKING_CENTER,
    EXTERMINATING
}

public class Doom : Bot
{
    int enemyCount;
    int centerX;
    int centerY;
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
                    state = TankState.SEEKING_CENTER;
                    break;
                case TankState.SEEKING_CENTER:
                    SeekCenter();
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

        centerX = ArenaWidth / 2;
        centerY = ArenaHeight / 2;
    }

    private void SeekCenter()
    {
        var cx = centerX - X;
        var cy = centerY - Y;

        TurnLeft(-Direction);

        double angleToCenter = Math.Atan2(centerY - Y, centerX - X) * 180 / Math.PI;

        TurnLeft(angleToCenter);

        double distanceToCenter = Math.Sqrt(Math.Pow(centerX - X, 2) + Math.Pow(centerY - Y, 2));

        Forward(distanceToCenter);

        state = TankState.EXTERMINATING;
    }

    private void Exterminate()
    {
        TurnRight(10);
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        if (state == TankState.EXTERMINATING)
        {
            Fire(3);
        }
    }

    public override void OnDeath(DeathEvent e)
    {
        Console.WriteLine("No ragrets.");
    }
}