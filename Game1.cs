using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UnamedGame.Abstract;
using UnamedGame.Helpers;
using UnamedGame.Player;
using UnamedGame.GameSystem;
using UnamedGame.GameSystem.UI;
using UnamedGame.System.Collision;
using UnamedGame.Entites;

namespace UnamedGame;

public class UnamedGame : Game
{
    public CollisionManager collisionManager = new CollisionManager();
    public Random random = new Random();
    public KeyboardState oldState;
    public List<Entity> entities = new List<Entity>();
    public PlayerEntity player;
    public Camera camera;
    public static UnamedGame Instance;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public UnamedGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = true;
        _graphics.HardwareModeSwitch = false;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Instance = this;
    }

    protected override void Initialize()
    {
        this.IsFixedTimeStep = true;
        Console.WriteLine("Game Initialized");
        camera = new Camera(new Vector2(0, 0), GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        player = new PlayerEntity(new Vector2(0, 0));
        entities.Add(player);
        UIManager.loadElementTextures();
    }
    public void SpawnEntity(Entity entity)
    {
        entities.Add(entity);
        if (entity is Collidable c)
        {
            collisionManager.addCollidable(c);
        }
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (Keyboard.GetState().IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
        {
            SpawnEntity(new StageGeometry(player.Position + new Vector2(random.Next(0, 100), random.Next(0, 100)), new Vector2(random.Next(0, 100), random.Next(0, 100)), new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255))));
        }
        base.Update(gameTime);
        for (int i = 0; i < entities.Count; i++)
        {
            Entity entity = entities[i];
            entity.whoAmi = i;
            entity.Update();
            if (entity is Collidable c)
            {

                c.MoveBy(c.Velocity);
                if (c.ShouldCheckCollisions)
                {
                    var collisions = collisionManager.gridSystem.GetCollisions(c.node.bounds);
                    foreach (Node node in collisions)
                    {
                        c.OnCollision(node.collidable);
                    }
                }

                c.PostUpdate();
                c.OldBounds = c.Bounds;
            }
            if (!entity.Active)
            {
                entities.RemoveAt(i);
            }
        }
        UIManager.UpdateElements();
        camera.Update();
        oldState = Keyboard.GetState();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: camera.TransformMatrix);
        foreach (Entity entity in entities)
        {
            entity.Draw(_spriteBatch);
        }
        DrawHelpers.DrawRectangle(_spriteBatch, new Rectangle(0, 0, 100, 100), Color.Red);
        DrawHelpers.DrawRectangleOutline(_spriteBatch, new Rectangle(0, 0, 100, 100), Color.Bisque);
        _spriteBatch.End();
        _spriteBatch.Begin();
        UIManager.Draw(_spriteBatch);
        base.Draw(gameTime);
        _spriteBatch.End();
    }
}
