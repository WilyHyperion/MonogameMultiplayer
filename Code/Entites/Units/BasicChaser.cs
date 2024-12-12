



using System.Numerics;
using Game.Abstract.Entites;
using Game.Helpers;

namespace Game.Entites.Units;

public class BasicChaser : Unit
{
    public override void SetDefaults()
    {
        this.strength = 50;
    }
    public int counter;
    public override void AI()
    {
        counter++;
        if (counter % 120 == 0)
        {
            this.Velocity = UnamedGame.random.NextVector2(-20, 20);
        }
    }
}