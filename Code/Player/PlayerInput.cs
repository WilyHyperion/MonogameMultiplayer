

using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

public class PlayerInput {
    public KeyboardState state = Keyboard.GetState();
    public Dictionary<Keys, bool> keys = new Dictionary<Keys, bool>();
    public PlayerInput() {
        keys.Add(Keys.W, state.IsKeyDown(Keys.W) );
        keys.Add(Keys.A, state.IsKeyDown(Keys.A) );
        keys.Add(Keys.S, state.IsKeyDown(Keys.S) );
        keys.Add(Keys.D, state.IsKeyDown(Keys.D) );
    }
    public void Update() {
        state = Keyboard.GetState();
        keys[Keys.W] = state.IsKeyDown(Keys.W);
        keys[Keys.A] = state.IsKeyDown(Keys.A);
        keys[Keys.S] = state.IsKeyDown(Keys.S);
        keys[Keys.D] = state.IsKeyDown(Keys.D);
    }
    public bool IsKeyDown(Keys key) {
        return keys[key];
    }
    public bool IsKeyUp(Keys key) {
        return !keys[key];
    }

    public MouseState MouseState = Mouse.GetState();
}