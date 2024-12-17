

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Game.Abstract;
using Game.Helpers;
using Microsoft.Xna.Framework.Input;
using System.Security.Claims;


namespace Game.System.Collision;
public enum Direction
{
    Right,
    Left,
    Up,
    Down
}
public struct Tile
{
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
    public const int TileSize = 50;
    /// <summary>
    /// Height and Width are 2x due to negative space
    /// </summary>
    /// <param name="Width"></param>
    /// <param name="Height"></param> <summary>
    /// 
    /// </summary>
    /// <param name="Width"></param>
    /// <param name="Height"></param>
    public CollisionManager(int Width, int Height)
    {
        this.WorldCollision = new Tile[Width * 2, Height * 2];
    }
    public Tile GetTile(int worldX, int worldY)
    {
        Point p = new Point(worldX / TileSize, worldY / TileSize);
        if (p.X + (WorldCollision.GetLength(0) / 2) >= (WorldCollision.GetLength(0)))
        {
            return new Tile(1);
        }
        if (p.X + (WorldCollision.GetLength(0) / 2) < 0)
        {
            return new Tile(1);
        }
        if (p.Y + (WorldCollision.GetLength(1) / 2) >= (WorldCollision.GetLength(1)))
        {
            return new Tile(1);
        }
        if (p.Y + (WorldCollision.GetLength(1) / 2) < 0)
        {
            return new Tile(1);
        }
        return this.WorldCollision[p.X + (WorldCollision.GetLength(0) / 2), p.Y + WorldCollision.GetLength(1) / 2];
    }
    public bool TilePosinWorld(int x, int y)
    {
        Point p = new Point(x, y);
        if (p.X + (WorldCollision.GetLength(0) / 2) >= (WorldCollision.GetLength(0)))
        {
            return false;
        }
        if (p.X + (WorldCollision.GetLength(0) / 2) < 0)
        {
            return false;
        }
        if (p.Y + (WorldCollision.GetLength(1) / 2) >= (WorldCollision.GetLength(1)))
        {
            return false;
        }
        if (p.Y + (WorldCollision.GetLength(1) / 2) < 0)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// trunicates, not rounded
    /// </summary>
    /// <param name="t"></param>
    /// <param name="pos"></param> <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <param name="pos"></param>
    public void SetTile(Tile t, Vector2 pos)
    {
        SetTile(t, (int)pos.X, (int)pos.Y);
    }
    public void SetTile(Tile t, int worldX, int worldY)
    {
        Point p = new Point(worldX / TileSize, worldY / TileSize);
        if (!TilePosinWorld(p.X, p.Y))
        {
            return;
        }
        this.WorldCollision[p.X + (WorldCollision.GetLength(0) / 2), p.Y + WorldCollision.GetLength(1) / 2] = t;
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
        int leftTile = (int)Math.Floor(newBounds.Left / TileSize);
        int rightTile = (int)Math.Floor(newBounds.Right / TileSize);
        int topTile = (int)Math.Floor(newBounds.Top / TileSize);
        int bottomTile = (int)Math.Floor(newBounds.Bottom / TileSize);
        for (int x = leftTile; x <= rightTile; x++)
        {
            for (int y = topTile; y <= bottomTile; y++)
            {
                Tile tile = GetTile(x * TileSize, y * TileSize);
                if (tile.Type != 0)
                {
                    Rectangle tileRect = new RectangleF(x * TileSize, y * TileSize, TileSize, TileSize);
                    if (newBounds.Intersects(tileRect))
                    {
                        Direction d = Direction.Right;
                        float xDist = 0;
                        float ydist = 0;
                        if (bounds.Right < tileRect.Left)
                        {
                            Console.WriteLine("1");
                            xDist =  tileRect.Left - bounds.Right;
                        }
                        if (bounds.Left > tileRect.Right)
                        {
                            Console.WriteLine("2");
                            xDist = bounds.Left - tileRect.Right;
                        }
                        Console.WriteLine($" ${bounds.Top}  {tileRect}");
                        if (bounds.Bottom < tileRect.Top)
                        {
                            Console.WriteLine("3");
                            ydist = bounds.Bottom - tileRect.Top;
                            Console.WriteLine(ydist);
                        }
                        if (bounds.Top > tileRect.Bottom)
                        {
                            Console.WriteLine("4");
                            ydist = tileRect.Bottom - bounds.Top;
                            Console.WriteLine(ydist);
                        }
                        Console.WriteLine($"x : {Math.Abs(xDist)}  y : {Math.Abs(ydist)}");
                        if (Math.Abs(ydist) > Math.Abs(xDist))
                        {
                            d =  ydist > 0 ? Direction.Down : Direction.Up ;
                        }
                        else
                        {

                        }
                        switch (d)
                        {
                            case Direction.Right:
                                newBounds.X = tileRect.Left - newBounds.Width;
                                break;
                            case Direction.Left:
                                newBounds.X = tileRect.Right;
                                break;
                            case Direction.Up:
                                newBounds.Y = tileRect.Bottom;
                                break;
                            default:
                                newBounds.Y = tileRect.Top - newBounds.Height;
                                break;
                        }
                    }
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
            return new Vector2(Bounds.X, Bounds.Y);
        }
    }
}