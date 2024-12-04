

using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

public class PlayerInput {
    public KeyboardState state = Keyboard.GetState();
    public Dictionary<Keys, bool> keys = new Dictionary<Keys, bool>();
    public PlayerInput() {
    }
    public void Update() {
        state = Keyboard.GetState();
        keys[Keys.W] = state.IsKeyDown(Keys.W);
        keys[Keys.A] = state.IsKeyDown(Keys.A);
        keys[Keys.S] = state.IsKeyDown(Keys.S);
        keys[Keys.D] = state.IsKeyDown(Keys.D);
    }
    public bool IsKeyDown(Keys key) {
        return state.IsKeyDown(key);
    }
    public bool IsKeyUp(Keys key) {
        return !state.IsKeyDown(key);
    }

    public MouseState MouseState = Mouse.GetState();
}