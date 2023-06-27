using System;
using System.Collections.Generic;
using System.Numerics;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using Util;

enum ScanTrackState
{
    IDLE,
    SCANNING,
    TRACKING_TARGET
}

class ScanTrack : Behaviour
{
    private ScanTrackState state;
    private double lastTick;
    private double scanDelay = 1000;
    private Queue<Target> visionBuffer = new Queue<Target>();
    private int visionBufferSize = 50;
    private int targetId = -1;

    public ScanTrack(Bot bot) : base(bot)
    {
        UpdateTick();
    }

    public override void Update()
    {
        switch (state)
        {
            case ScanTrackState.IDLE:
                state = ScanTrackState.SCANNING;
                break;
            case ScanTrackState.SCANNING:
                Scanning();
                break;
            case ScanTrackState.TRACKING_TARGET:
                // Might need a fallback escape case?
                break;
            default:
                state = ScanTrackState.IDLE;
                break;
        }
    }

    public override void ScanBot(ScannedBotEvent e)
    {
        // Add this sighting to bugger ..

        var tx = e.X;
        var ty = e.Y;
        var tid = e.ScannedBotId;

        var target = new Target(tx, ty, tid);

        visionBuffer.Enqueue(target);

        if (visionBuffer.Count > visionBufferSize)
        {
            visionBuffer.Dequeue();
        }

        // Target evaluation ...

        EvaluateTarget(target);

        // Tracking ...

        if (targetId == target.Tid)
        {
            TrackTarget(target);
        }

    }

    private void EvaluateTarget(Target target)
    {
        // TODO:
        // Some sort of clever and cool analysis of
        // this bot and whether or not to switch target
        // or ignore it.
        //
        // For now, just stick to the one target ...

        if (targetId == -1)
        {
            targetId = target.Tid;
        }
    }

    private void TrackTarget(Target target)
    {
        // TODO: Stole from track bot, do something interesting ...

        var bearingFromGun = bot.GunBearingTo(target.Tx, target.Ty);

        bot.TurnGunLeft(bearingFromGun);
        bot.Rescan();

        if (Math.Abs(bearingFromGun) <= 3 && bot.GunHeat == 0)
            bot.Fire(Math.Min(3 - Math.Abs(bearingFromGun), bot.Energy - .1));
    }

    private void Scanning()
    {
        if (ShouldScan())
        {
            Scan();
            UpdateTick();
        }

    }

    private void Scan()
    {
        bot.TurnGunLeft(360);
    }

    private double GetTime()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    private bool ShouldScan()
    {
        return GetTime() > lastTick + scanDelay;
    }

    private void UpdateTick()
    {
        lastTick = GetTime();
    }
}