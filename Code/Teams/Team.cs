

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Game;
using Game.Abstract;
using Game.Helpers;
using Game.Abstract.Entites;

public class Team {
    public List<Unit> Units = new List<Unit>();
    public int ID;
    public Color TeamColor;
    public List<Team> Allied = new List<Team>();

    public string name;
    public Team(String name){
        this.name = name;
        this.TeamColor = UnamedGame.random.GetRandomColor();
    }
    public Team(String name, Color c){
        this.name = name;
        this.TeamColor = c;
    }
}