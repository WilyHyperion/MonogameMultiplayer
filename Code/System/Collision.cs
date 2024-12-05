

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UnamedGame.Abstract;
using UnamedGame.Helpers;

namespace UnamedGame.System.Collision;
public class CollisionManager(){
    public List<Collidable> collidables = new List<Collidable>();
    public GridSystem gridSystem = new GridSystem();
    public void addCollidable(Collidable collidable){
        collidable.gridSystem = gridSystem;
        collidable.node = new Node(collidable.Bounds, gridSystem.GetNextID(), collidable);
        gridSystem.AddNode(collidable.node);
        collidables.Add(collidable);
    }
    public void removeCollidable(Collidable collidable){
        gridSystem.RemoveNode(collidable.node);
        collidables.Remove(collidable);
    }
    public void Update(){
        foreach (Collidable collidable in collidables){
            gridSystem.GetCollisions(collidable.node.bounds).ForEach((node) => collidable.OnCollision(node.collidable));
        }
    }

}

public abstract class Collidable : Entity{
    public Node node;
    public GridSystem gridSystem;
    public  RectangleF _bounds {get; set;}
    public RectangleF OldBounds {get; set;}
    public RectangleF Bounds {get {
        return _bounds;
    }
     set  {
        if(gridSystem == null){
            gridSystem = UnamedGame.Instance.collisionManager.gridSystem;
        }
        _bounds = value;
        if(node == null){
            createNode();
        }
        gridSystem.RemoveNode(node);
        node = new Node(value, node.ID, this);
        gridSystem.AddNode(node);
    } }
    public virtual void OnCollision(Collidable other){

    }

    public void MoveBy(Vector2 velocity)
    {
        Bounds = new RectangleF((Bounds.X + velocity.X), (Bounds.Y + velocity.Y), Bounds.Width, Bounds.Height);
    }

    public void createNode() {
        Console.WriteLine(Bounds);
        this.node = new Node( Bounds, gridSystem.GetNextID(), this);
    }
    public Vector2 Position {get {
        return new Vector2(Bounds.X, Bounds.Y);
    } }
}