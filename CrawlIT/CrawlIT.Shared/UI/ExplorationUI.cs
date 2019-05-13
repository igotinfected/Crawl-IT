﻿using CrawlIT.Shared.Entity;
using CrawlIT.Shared.GameState;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Camera = CrawlIT.Shared.Camera.Camera;

namespace CrawlIT.Shared.UI
{
    public class ExplorationUI
    {
        private UIIcon _lifeBar;
        private UIIcon _surgeCrystal;
        private UIIcon _save;
        private UIIcon _inventory;
        private UIIcon _help;
        private UIIcon _badges;

        private Texture2D _lifeBarTexture;
        private Texture2D _surgeCrystalTexture;
        private Texture2D _saveTexture;
        private Texture2D _inventoryTexture;
        private Texture2D _helpTexture;
        private Texture2D _badgesTexture;

        private readonly float _zoom;
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private Camera.Camera _staticCamera;
        private SpriteFont _font;
        private SpriteBatch _spriteBatch;

        public ExplorationUI(float zoom, GraphicsDeviceManager graphics, ContentManager content, Camera.Camera staticCamera)
        {
            _zoom = zoom;
            _graphics = graphics;
            _content = content;
            _staticCamera = staticCamera;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            _lifeBarTexture = _content.Load<Texture2D>("Sprites/lifebar");
            _saveTexture = _content.Load<Texture2D>("Sprites/save");
            _inventoryTexture = _content.Load<Texture2D>("Sprites/iconplaceholder");
            _helpTexture = _content.Load<Texture2D>("Sprites/iconplaceholder");
            _badgesTexture = _content.Load<Texture2D>("Sprites/iconplaceholder");
            _surgeCrystalTexture = _content.Load<Texture2D>("Sprites/surgecrystal");

            _font = _content.Load<SpriteFont>("Fonts/File");

            _lifeBar = new UIIcon(_lifeBarTexture, _zoom, 50, 50);
            _surgeCrystal = new UIIcon(_surgeCrystalTexture, _zoom, _graphics.PreferredBackBufferWidth - (32 * _zoom + 50), 50);
            _save = new UIIcon(_saveTexture, _zoom, (_graphics.PreferredBackBufferWidth / 4) + 50, _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
            _inventory = new UIIcon(_inventoryTexture, _zoom, _graphics.PreferredBackBufferWidth - (32 * _zoom + 50), _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
            _help = new UIIcon(_helpTexture, _zoom, 50, _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
            _badges = new UIIcon(_badgesTexture, _zoom, (_graphics.PreferredBackBufferWidth / 2) + 50, _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;

            var levelString = "Semester 1";
            var (levelStringDimensionX, levelStringDimensionY) = _font.MeasureString(levelString);
            var levelStringPosX = (_graphics.PreferredBackBufferWidth - levelStringDimensionX) / 2;
            var levelStringPosY = 50;

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, levelString, new Vector2(levelStringPosX, levelStringPosY), Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.BackToFront,
                              BlendState.AlphaBlend,
                              SamplerState.PointClamp, null, null, null,
                              _staticCamera.Transform);
            _lifeBar.Draw(_spriteBatch);
            _surgeCrystal.Draw(_spriteBatch);
            _save.Draw(_spriteBatch);
            _inventory.Draw(_spriteBatch);
            _help.Draw(_spriteBatch);
            _badges.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
