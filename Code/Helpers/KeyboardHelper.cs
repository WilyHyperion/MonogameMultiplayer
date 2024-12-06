

namespace Game.Helpers;
using Microsoft.Xna.Framework.Input;
public static class KeyboardHelper {
    public static bool keyPressed(this KeyboardState state, Keys key) {
        return state.IsKeyDown(key) && !UnamedGame.Instance.oldState.IsKeyDown(key);
    }
}