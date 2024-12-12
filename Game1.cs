using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.Abstract;
using Game.Helpers;
using Game.Player;
using Game.GameSystem;
using Game.GameSystem.UI;
using Game.System.Collision;
using Game.Entites;
using Game.Abstract.UI;
using System.Diagnostics;
using Server.Packets;
using Game.GameSystem.Networking;
using Game.Abstract.Entites;
using Game.Entites.Units;

namespace Game;

public class UnamedGame : Microsoft.Xna.Framework.Game
{
    public int GameTick;
    public CollisionManager collisionManager = new CollisionManager();
    public static Random random = new Random();
    public KeyboardState oldState;
    public List<Entity> entities = new List<Entity>();
    public PlayerEntity player;
    public Camera camera;
    public static UnamedGame Instance;
    public Team playerTeam = new Team("Player");
    public Team enemyTeam = new Team("Enemy");
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    internal float updateTime;
    internal float drawTime;
    Stopwatch stopwatch = new Stopwatch();

    public UnamedGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = true;
        _graphics.HardwareModeSwitch = false;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Instance = this;
        new Server.Server();
    }
    public static ClientServer server = new ClientServer();
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
        Unit.InitializeUnits(Content);
        UIManager.loadElementTextures();
        SpriteFont font = Content.Load<SpriteFont>("Roboto");
        UILogText uIText = new UILogText( font);
        Helpers.Logger.LogText = uIText;
        UIManager.AddElement(uIText);
        UIManager.AddElement(new Preformance(font));
    }
    public void SpawnEntity(Entity entity)
    {
        entities.Add(entity);
        entity.whoAmi = entities.Count;
        if (entity is Collidable c)
        {
            collisionManager.addCollidable(c);
        }
    }
    protected override void Update(GameTime gameTime)
    {
        if(GameTick == 0 ) {
            this.player =  new PlayerEntity( new Vector2(0,0));
            SpawnEntity(player);
            server.Send(new Connect());
        }
        camera.Update();
        stopwatch.Restart();
        GameTick++;
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (Keyboard.GetState().IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
        {
            Unit.SpawnUnit<BasicChaser>(player.Position, playerTeam);
        }
        base.Update(gameTime);
        for (int i = 0; i < entities.Count; i++)
        {
            Entity entity = entities[i];
            entity.whoAmi = i;
            entity.Update();
            if (entity is Collidable c)
            {
                c.OldBounds = c.Bounds;
                c.MoveBy(c.Velocity);
                if (c.ShouldCheckCollisions)
                {
                    var collisions = collisionManager.gridSystem.GetCollisions(c.Bounds);
                    foreach (Node node in collisions)
                    {
                        c.OnCollision(node.collidable);
                    }
                }
                c.PostUpdate();
            }
            if (!entity.Active)
            {
                entities.RemoveAt(i);
            }
        }
        UIManager.UpdateElements();
        oldState = Keyboard.GetState();
        updateTime = stopwatch.ElapsedMilliseconds;

    }

    protected override void Draw(GameTime gameTime)
    {
        stopwatch.Restart();
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: camera.TransformMatrix);
        foreach (Entity entity in entities)
        {
            entity.Draw(_spriteBatch);
        }
        _spriteBatch.End();
        _spriteBatch.Begin();
        UIManager.Draw(_spriteBatch);
        base.Draw(gameTime);
        _spriteBatch.End();
        drawTime = stopwatch.ElapsedMilliseconds;
    }

    public void AddNewRemote(string name, Vector2 pos)
    {
        Console.WriteLine(name);
        Console.WriteLine(pos);
        PlayerEntity p = new PlayerEntity(pos);
        p.name = name;
        SpawnEntity(p);
        p.remote = true;
        
    }
}
