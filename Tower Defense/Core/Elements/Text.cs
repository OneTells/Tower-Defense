using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower_Defense.Core.Elements
{
    public class Text : Component
    {
        private readonly SpriteFont _font;
        private readonly Vector2 _position;
        private readonly bool _isShift = true;
        
        public bool IsSelect;
        public string Caption;

        public Text(string fontName, Vector2 position, string caption)
        {
            _position = position;
            _font = Content.Load<SpriteFont>(fontName);
            Caption = caption;
        }
    
        public Text(string fontName, Vector2 position, string caption, bool isShift)
        {
            _position = position;
            _font = Content.Load<SpriteFont>(fontName);
            Caption = caption;

            _isShift = isShift;
        }
    
        public override void Update() {}
    
        public override void Draw()
        {
            Sprite.DrawString(
                _font, Caption, 
                new Vector2(_position.X - (_isShift ? _font.MeasureString(Caption).X : 0), _position.Y), 
                IsSelect ? Color.Gray : Color.White
            );
        }
    }
}