

using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

public class PlayerInput {
    public KeyboardState state = Keyboard.GetState();
    public PlayerInput() {
    }
    public void Update() {
        state = Keyboard.GetState();
    }
    public bool IsKeyDown(Keys key) {
        return state.IsKeyDown(key);
    }
    public bool IsKeyUp(Keys key) {
        return !state.IsKeyDown(key);
    }

    public MouseState MouseState = Mouse.GetState();
}