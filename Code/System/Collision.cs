

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Game.Abstract;
using Game.Helpers;
using Microsoft.Xna.Framework.Input;

namespace Game.System.Collision;
public struct Tile {
    public ushort Type = 0;

    public Tile(int t = 1)
    {
        this.Type = 1;
    }
}
public class CollisionManager
{
    /// <summary>
    /// Width/height in pixels of a tile
    /// </summary> <summary>
    /// 
    /// </summary>
    public const int TileSize = 16;
    /// <summary>
    /// Height and Width are 2x due to negative space
    /// </summary>
    /// <param name="Width"></param>
    /// <param name="Height"></param> <summary>
    /// 
    /// </summary>
    /// <param name="Width"></param>
    /// <param name="Height"></param>
    public CollisionManager(int Width, int Height){
        this.WorldCollision = new Tile[Width * 2, Height * 2];
    }
    public Tile GetTile(int worldX, int worldY){
        Point p = new Point(worldX/ TileSize, worldY/TileSize);
        return this.WorldCollision[p.X + (WorldCollision.GetLength(0)/2), p.Y + WorldCollision.GetLength(1)/2];
    }
    public  void SetTile(Tile t,Vector2 pos){
        SetTile(t, (int)pos.X, (int)pos.Y);
    }
    public  void SetTile(Tile t, int worldX, int worldY){
        Point p = new Point(worldX/ TileSize, worldY/TileSize);
        this.WorldCollision[p.X + (WorldCollision.GetLength(0)/2), p.Y + WorldCollision.GetLength(1)/2] = t;
    }
    /// <summary>
    /// Do not index directly without knowing what your doing -- me
    /// </summary>
    Tile[,] WorldCollision;
    public List<Collidable> collidables = new List<Collidable>();
    public GridSystem gridSystem = new GridSystem();
    public void addCollidable(Collidable collidable)
    {
        collidable.gridSystem = gridSystem;
        collidable.node = new Node(collidable.Bounds, gridSystem.GetNextID(), collidable);
        gridSystem.AddNode(collidable.node);
        collidables.Add(collidable);
    }
    public void removeCollidable(Collidable collidable)
    {
        gridSystem.RemoveNode(collidable.node);
        collidables.Remove(collidable);
    }

    public RectangleF moveRect(RectangleF bounds, Vector2 velocity)
    {
        RectangleF newBounds = bounds;
        newBounds.X += velocity.X;
        newBounds.Y += velocity.Y;
        //check worldcollision tiles
        for(int i = 0; i < newBounds.Width; i+= TileSize) {
            for(int j = 0; j < newBounds.Height; j+= TileSize) {
                Tile t = GetTile((int)(newBounds.X + i), (int)(newBounds.Y + j));
                if(t.Type != 0 ){
                    return new RectangleF(bounds.X + i - TileSize, bounds.Y + j - TileSize, bounds.Width, bounds.Height);
                }
            }
        }
        return newBounds;
    }


}

public abstract class Collidable : Entity
{
    public bool ShouldCheckCollisions = true;
    public Node node;
    public GridSystem gridSystem;
     RectangleF _bounds = new RectangleF();
    public RectangleF OldBounds { get; set; }
    public RectangleF Bounds
    {
        get
        {
            return _bounds;
        }
        set
        {
            if (gridSystem == null)
            {
                gridSystem = UnamedGame.Instance.collisionManager.gridSystem;
            }
            _bounds = value;
            if (node == null)
            {
                createNode();
            }
            gridSystem.RemoveNode(node);
            node = new Node(value, node.ID, this);
            gridSystem.AddNode(node);
        }
    }
    public virtual void OnCollision(Collidable other)
    {

    }
    public CollisionManager collisionManager = UnamedGame.Instance.collisionManager;
    public void MoveBy(Vector2 velocity)
    {

        Bounds = collisionManager.moveRect(this.Bounds, velocity);
    }

    public void createNode()
    {
        this.node = new Node(Bounds, gridSystem.GetNextID(), this);
    }
    public Vector2 Position
    {
        get
        {
            return new Vector2(Bounds.X , Bounds.Y);
        }
    }
}