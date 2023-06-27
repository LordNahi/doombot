using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using System.Collections.Generic;

enum TankState
{
    IDLE,
    SEEKING_POSITION,
    EXTERMINATING
}

public class Doom : Bot
{
    List<Behaviour> behaviours = new List<Behaviour>();
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
            foreach (Behaviour behaviour in behaviours)
            {
                behaviour.Update();
            }
        }
    }

    private void SetUp()
    {
        state = TankState.IDLE;

        AdjustGunForBodyTurn = false;
        AdjustRadarForGunTurn = false;
        AdjustRadarForBodyTurn = false;

        BulletColor = Color.Yellow;
        BodyColor = Color.Green;
        TurretColor = Color.Gray;
        RadarColor = Color.Red;
        ScanColor = Color.Red;

        enemyCount = EnemyCount;

        targetX = ArenaWidth / 2;
        targetY = ArenaHeight / 2;

        // Define behaviours ...

        behaviours.Add(new HitMove(this));
        behaviours.Add(new ScanTrack(this));
    }

    public override void OnGameStarted(GameStartedEvent e)
    {
        foreach (Behaviour behaviour in behaviours)
        {
            behaviour.Create(e);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        foreach (Behaviour behaviour in behaviours)
        {
            behaviour.ScanBot(e);
        }
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        foreach (Behaviour behaviour in behaviours)
        {
            behaviour.Hit(e);
        }
    }

    public override void OnDeath(DeathEvent e)
    {
        foreach (Behaviour behaviour in behaviours)
        {
            behaviour.Killed(e);
        }
    }
}