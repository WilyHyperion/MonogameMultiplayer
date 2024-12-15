

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Game;
using Game.Abstract;
using Game.Helpers;
using Game.Abstract.Entities;

public class Team {
    public static Team Any = new Team("Any",  Color.Gray);
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
    public override bool Equals(object obj)
    {
        if(this == Any){
            return true;
        }
        return this.name == ((Team)obj).name;
    }
    public override string ToString()
    {
        return this.name;
    }
}