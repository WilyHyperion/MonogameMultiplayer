
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace UnamedGame.System.Collision;

//maybe the real quadtree was the better algorithm we made along the way
struct Node {
    public Rectangle bounds;
    public int ID;
    public Node(Rectangle bounds, int ID){
        this.bounds = bounds;
        this.ID = ID;
    }
    
}

class GridSystem {
    //TODO determine this better
    const int GridSize = 32;
    public static Vector2 GetGridPosition(Vector2 position){
        return new Vector2((int)(position.X / GridSize), (int)(position.Y / GridSize));
    }
    //x, y, elements in that gird location
    public Dictionary<int, Dictionary<int, List<Node>>> Grid = new Dictionary<int, Dictionary<int, List<Node>>>();

    public void AddNode(Node node){
        Vector2 gridPosition = GetGridPosition(new Vector2(node.bounds.X, node.bounds.Y));
        if(!Grid.ContainsKey((int)gridPosition.X)){
            Grid[(int)gridPosition.X] = new Dictionary<int, List<Node>>();
        }
        if(!Grid[(int)gridPosition.X].ContainsKey((int)gridPosition.Y)){
            Grid[(int)gridPosition.X][(int)gridPosition.Y] = new List<Node>();
        }
        Grid[(int)gridPosition.X][(int)gridPosition.Y].Add(node);
    }
    public void RemoveNode(Node node){
        Vector2 gridPosition = GetGridPosition(new Vector2(node.bounds.X, node.bounds.Y));
        if(!Grid.ContainsKey((int)gridPosition.X)){
            return;
        }
        if(!Grid[(int)gridPosition.X].ContainsKey((int)gridPosition.Y)){
            return;
        }
        Grid[(int)gridPosition.X][(int)gridPosition.Y].Remove(node);
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
    public List<Node> GetCollisions(Rectangle bounds){
        List<Node> nodes = new List<Node>();
        Vector2 gridPosition = GetGridPosition(new Vector2(bounds.X, bounds.Y));
        for(int x = -1; x < 2; x++){
            for(int y = -1; y < 2; y++){
                if(Grid.ContainsKey((int)gridPosition.X + x) && Grid[(int)gridPosition.X + x].ContainsKey((int)gridPosition.Y + y)){
                    foreach(Node node in Grid[(int)gridPosition.X + x][(int)gridPosition.Y + y]){
                        if(node.bounds.Intersects(bounds)){
                            nodes.Add(node);
                        }
                    }
                }
            }
        }
        return nodes;
    }
}