using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

abstract class Behaviour
{
    public Bot bot;


    public Behaviour(Bot control)
    {
        bot = control;

        Create();
    }

    public virtual void Create() { }
    public virtual void Update() { }
    public virtual void Hit(HitByBulletEvent e) { }
    public virtual void ScanBot(ScannedBotEvent e) { }
    public virtual void Killed(DeathEvent e) { }
}