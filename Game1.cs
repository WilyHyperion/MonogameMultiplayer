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
using Game.Entities;
using Game.Abstract.UI;
using System.Diagnostics;
using Server.Packets;
using Game.GameSystem.Networking;
using Game.Abstract.Entities;
using Game.Entities.Units;
using Game.System;
using Debug = Game.System.Debug;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Linq;
using System.Dynamic;

namespace Game;

public class UnamedGame : Microsoft.Xna.Framework.Game
{
    public Vector2 MouseWorldPos()
    {
        return this.camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
    }
    public void ReSyncPlayer(PlayerEntity p)
    {
        if (ConnectedPlayers.ContainsKey(p.ID))
        {
            ConnectedPlayers[p.ID].player = p;
        }
        else
        {
            Console.WriteLine("new plaer join");
            ConnectedPlayers.Add(p.ID, new GamePlayer(p.ID, p, false));
        }

    }
    public byte MyID;
    public int GameTick;
    public CollisionManager collisionManager = new CollisionManager(1000, 1000);
    public static Random random = new Random();
    public KeyboardState oldState;
    public List<Entity> entities = new List<Entity>();
    public List<Entity> GetEntities(){
        List<Entity> other = new List<Entity>();
        for(int i = 0; i < ConnectedPlayers.Count; i ++){
            other.Add(ConnectedPlayers.ElementAt(i).Value.player);
        }
        return [.. this.entities, .. other];
    }
    public PlayerEntity player;
    public Dictionary<int, Player.GamePlayer> ConnectedPlayers = new Dictionary<int, Player.GamePlayer>();
    public Camera camera;
    public static UnamedGame Instance;
    public Team playerTeam = new Team("Player");
    public Team enemyTeam = new Team("Enemy");
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    internal float updateTime;
    internal float drawTime;
    Stopwatch stopwatch = new Stopwatch();
    public ClientServer server;

    public UnamedGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = true;
        _graphics.HardwareModeSwitch = false;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Instance = this;
        server = new ClientServer();
        server.init();
    }
    protected override void Initialize()
    {
        DrawHelpers.init();
        this.IsFixedTimeStep = true;
        camera = new Camera(new Vector2(0, 0), GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        base.Initialize();
        player = new PlayerEntity( Vector2.Zero);
        server.Send(new Connect());
    }
    public static SpriteFont font;
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Unit.InitializeUnits(Content);
        Projectile.InitializeUnits(Content);
        UIManager.loadElementTextures();
        font = Content.Load<SpriteFont>("Roboto");
        UILogText uIText = new UILogText(font);
        Helpers.Logger.LogText = uIText;
        UIManager.AddElement(uIText);
        UIManager.AddElement(new Preformance(font));
        PlayerEntity.guntexture = Content.Load<Texture2D>("gun");
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
    public GamePlayer myPlayer;
    protected override void Update(GameTime gameTime)
    {
        ClientServer.RunPackets();
        camera.Update();
        stopwatch.Restart();
        GameTick++;
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (Keyboard.GetState().IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
        {
            Unit.SpawnUnit<BasicChaser>(player.Position + random.NextVector2(0, 500), playerTeam);
            Unit.SpawnUnit<BasicChaser>(player.Position + random.NextVector2(-500, 0), enemyTeam);
            Unit.SpawnUnit<BasicShooter>(player.Position + random.NextVector2(0, 500), playerTeam);
            Unit.SpawnUnit<BasicShooter>(player.Position + random.NextVector2(-500, 0), enemyTeam);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.OemTilde) && oldState.IsKeyUp(Keys.OemTilde))
        {
            Debug.ToggleDebug();
        }
        if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
        {
            this.collisionManager.SetTile(new Tile(1), MouseWorldPos());
        }
        base.Update(gameTime);
        
       
        List<Entity> all = GetEntities();
        for (int i = 0; i < all.Count; i++)
        {
            Entity entity = all[i];
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
            
        }
        UIManager.UpdateElements();
        oldState = Keyboard.GetState();
        updateTime = stopwatch.ElapsedMilliseconds;

    }
    //TODO - Only Draw entities within camera bounds
    protected override void Draw(GameTime gameTime)
    {
        stopwatch.Restart();
        GraphicsDevice.Clear(Color.BlanchedAlmond);
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: camera.TransformMatrix);
        RectangleF bounds = camera.camBounds;
        bounds.X -= bounds.X % CollisionManager.TileSize + CollisionManager.TileSize;
        bounds.Y -= bounds.Y % CollisionManager.TileSize + CollisionManager.TileSize;
        for (float i = bounds.X; i < bounds.Right + CollisionManager.TileSize; i += CollisionManager.TileSize)
        {
            for (float j = bounds.Y; j < bounds.Bottom + CollisionManager.TileSize; j += CollisionManager.TileSize)
            {
                Tile t = collisionManager.GetTile((int)i, (int)j);
                if (t.Type != 0)
                {
                    DrawHelpers.DrawRectangle(_spriteBatch, new Rectangle((int)i, (int)j, CollisionManager.TileSize, CollisionManager.TileSize), random.GetRandomColor());
                }
            }
        }
        _spriteBatch.End();
        _spriteBatch.Begin(transformMatrix: camera.TransformMatrix);
        
        List<Entity> all = GetEntities();
        foreach (Entity entity in all)
        {
            entity.Draw(_spriteBatch);
            if (Debug.DebugMode)
                Debug.DrawDebugInformation(entity, _spriteBatch);
        }
        if (Debug.DebugMode)
        {
            Debug.DrawDebugGrid(_spriteBatch, collisionManager.gridSystem);
        }
        _spriteBatch.End();
        _spriteBatch.Begin();
        UIManager.Draw(_spriteBatch);

        base.Draw(gameTime);
        _spriteBatch.End();
        drawTime = stopwatch.ElapsedMilliseconds;
    }


}
