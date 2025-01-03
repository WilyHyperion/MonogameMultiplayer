

using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Player;

public class GamePlayer {
    public int ID;
    public PlayerEntity player;
    public bool Local;
    public GamePlayer(int id, PlayerEntity player, bool local){
        this.ID = id;
        this.player = player;
        this.player.remote = !local;
        this.Local = local;
        
    }
    public void Update(){
        player.Update();
    }
    public void Draw(SpriteBatch s){
        player.Draw(s);
    }
}