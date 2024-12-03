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
using UnamedGame.System;

namespace UnamedGame;

public class UnamedGame : Game
{
    
    public Random random = new Random();
    public KeyboardState oldState;
    public List<Entity> entities = new List<Entity>();
    public PlayerEntity player;
    public Camera camera;
    public static UnamedGame Instance;
    Texture2D unitTexture;
    Texture2D guntexture;
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
        camera = new Camera(new Vector2(0, 0), GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width , GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        player = new PlayerEntity( new Vector2(0, 0));
        entities.Add(player);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        base.Update(gameTime);
        for (int i = 0; i < entities.Count; i++)
        {
            Entity entity = entities[i];
            entity.whoAmi = i;
            entity.Update();
            entity.Position += entity.Velocity;
            if (!entity.Active)
            {
                entities.RemoveAt(i);
            }
        }
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
        base.Draw(gameTime);
        _spriteBatch.End();
    }
}
