#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Game.Helpers;
namespace Game.System.Collision;

//maybe the real quadtree was the better algorithm we made along the way
public class Node {
    public Collidable? collidable;
    public RectangleF bounds;
    public int ID;
    public Node(RectangleF bounds, int ID, Collidable collidable){
        this.bounds = bounds;
        this.ID = ID;
        this.collidable = collidable;
    }
    public Node(RectangleF bounds, int ID){
        this.bounds = bounds;
        this.ID = ID;
    }
    public override string ToString()
    {
        return $"Bounds :{ this.bounds}  ID : {ID}";
    }
}

public class GridSystem {
    int nextID = 0;
    public int GetNextID(){
        return nextID++;
    }
    //TODO determine this better
    public const int GridSize = 250;
    public static Vector2 GetGridPosition(Vector2 position){
        return new Vector2((int)(position.X / GridSize), (int)(position.Y / GridSize));
    }
    //x, y, elements in that gird location
    public Dictionary<int, Dictionary<int, List<Node>>> Grid = new Dictionary<int, Dictionary<int, List<Node>>>();

    public void AddNode(Node node){
        Vector2 gridPosition = GetGridPosition(new Vector2(node.bounds.X + node.bounds.Width/2, node.bounds.Y + node.bounds.Height/2));
        if(!Grid.ContainsKey((int)gridPosition.X)){
            Grid[(int)gridPosition.X] = new Dictionary<int, List<Node>>();
        }
        if(!Grid[(int)gridPosition.X].ContainsKey((int)gridPosition.Y)){
            Grid[(int)gridPosition.X][(int)gridPosition.Y] = new List<Node>();
        }
        Grid[(int)gridPosition.X][(int)gridPosition.Y].Add(node);
    }
    public void RemoveNode(Node? nullnode){
        if(nullnode == null){
            return;
        }
        Node node = (Node)nullnode;
        Vector2 gridPosition = GetGridPosition(node.bounds.Middle);
        if(!Grid.ContainsKey((int)gridPosition.X)){
            return;
        }
        if(!Grid[(int)gridPosition.X].ContainsKey((int)gridPosition.Y)){
            return;
        }
        Grid[(int)gridPosition.X][(int)gridPosition.Y].Remove(node);
    }
    public List<Node> GetNodesWithin(Vector2 worldPos, float dist){
        Vector2 gridPosition = GetGridPosition(worldPos);
        List<Node> nodes = new List<Node>();
        int searchAmount = (int)Math.Ceiling(dist/GridSize)/2;
        for(int x = -searchAmount; x <  searchAmount; x++){
            for(int y = -searchAmount; y < searchAmount; y++){
                if(Grid.ContainsKey((int)gridPosition.X + x) && Grid[(int)gridPosition.X + x].ContainsKey((int)gridPosition.Y + y)){
                    nodes.AddRange(Grid[(int)gridPosition.X + x][(int)gridPosition.Y + y]);
                }
            }
        }   
        return nodes;
    }
    public List<Node> GetNodesInAndAround(Vector2 worldPos){
        Vector2 gridPosition = GetGridPosition(worldPos);
        List<Node> nodes = new List<Node>();
        for(int x = -1; x < 2; x++){
            for(int y = -1; y < 2; y++){
                if(Grid.ContainsKey((int)gridPosition.X + x) && Grid[(int)gridPosition.X + x].ContainsKey((int)gridPosition.Y + y)){
                    nodes.AddRange(Grid[(int)gridPosition.X + x][(int)gridPosition.Y + y]);
                }
            }
        }
        return nodes;
    }
    public List<Node> GetNodesIn(Vector2 worldPos){
        Vector2 gridPosition = GetGridPosition(worldPos);
        List<Node> nodes = new List<Node>();
        if(Grid.ContainsKey((int)gridPosition.X) && Grid[(int)gridPosition.X].ContainsKey((int)gridPosition.Y)){
            nodes.AddRange(Grid[(int)gridPosition.X][(int)gridPosition.Y]);
        }
        return nodes;
    }
    public List<Node> GetCollisions(RectangleF bounds){
        List<Node> nodes = new List<Node>();
        Vector2 gridPosition = GetGridPosition(new Vector2(bounds.X, bounds.Y));
        int num = 0;
        for(int x = -1; x < 2; x++){
            for(int y = -1; y < 2; y++){
                if(Grid.ContainsKey((int)gridPosition.X + x) && Grid[(int)gridPosition.X + x].ContainsKey((int)gridPosition.Y + y)){
                    foreach(Node node in Grid[(int)gridPosition.X + x][(int)gridPosition.Y + y]){
                        num++;
                        if(node.bounds.Intersects(bounds) && node.collidable != null && node.collidable.Bounds != bounds){
                            nodes.Add(node);
                        }
                    }
                }
            }
        }
        return nodes;
    }
}
