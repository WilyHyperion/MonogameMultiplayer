
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UnamedGame.Abstract.Entites;
using UnamedGame.Helpers;

namespace UnamedGame.Entites
{
    public class StageGeometry : SoildEntity
    {
        public Color Color;

        public StageGeometry(Vector2 position, Vector2 size, Color color)
        {
            this.Bounds = new RectangleF(position.X, position.Y, size.X, size.Y);
            this.OldBounds = Bounds;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            Helpers.DrawHelpers.DrawRectangle(spriteBatch, Bounds, Color);
        }
    }
}