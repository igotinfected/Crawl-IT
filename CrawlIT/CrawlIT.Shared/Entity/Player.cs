﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;


namespace CrawlIT.Shared.Entity
{
    public class Player : Entity
    {
        private Animation.Animation _walkUp;
        private Animation.Animation _walkDown;
        private Animation.Animation _walkLeft;
        private Animation.Animation _walkRight;

        private Animation.Animation _standUp;
        private Animation.Animation _standDown;
        private Animation.Animation _standLeft;
        private Animation.Animation _standRight;

        private Animation.Animation _currentAnimation;

        private Matrix _scale;

        private int _frameWidth;
        private int _frameHeight;

        private Rectangle currentCollision;

        public List<Rectangle> CollisionObjects { get; set; }

        public Vector2 CurrentVelocity { get; set; }

        // For collision
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)PosX, (int)PosY, _frameWidth, _frameHeight);
            }
        }

        public Player(Texture2D texture, Matrix scale, float posx = 50, float posy = 50)
        {
            TextureSheet = texture;
            PosX = posx;
            PosY = posy;
            _scale = scale;
            _frameWidth = 23;
            _frameHeight = 45;

            // TODO: rethink this animation frame setup, probably better ways to set this up
            _walkDown = new Animation.Animation();
            _walkDown.AddFrame(new Rectangle(0, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth * 2, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(0, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth * 3, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth * 4, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth * 3, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _walkUp = new Animation.Animation();
            _walkUp.AddFrame(new Rectangle(0, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth * 2, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(0, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth * 4, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _walkLeft = new Animation.Animation();
            _walkLeft.AddFrame(new Rectangle(0, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth * 2, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(0, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth * 4, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _walkRight = new Animation.Animation();
            _walkRight.AddFrame(new Rectangle(0, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth * 2, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(0, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth * 4, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _standUp = new Animation.Animation();
            _standUp.AddFrame(new Rectangle(0, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _standDown = new Animation.Animation();
            _standDown.AddFrame(new Rectangle(0, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _standLeft = new Animation.Animation();
            _standLeft.AddFrame(new Rectangle(0, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _standRight = new Animation.Animation();
            _standRight.AddFrame(new Rectangle(0, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
        }

        public override void Update(GameTime gameTime)
        {
            var velocity = GetVelocity();

            var oldPosX = PosX;
            var oldPosY = PosY;

            PosX += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            PosY += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Collides())
            {
                if ((velocity.Y > 0 && CollidesTop()) || (velocity.Y < 0 && CollidesBottom()))
                    PosY = oldPosY;
                if ((velocity.X > 0 && CollidesLeft()) || (velocity.X < 0 && CollidesRight()))
                    PosX = oldPosX;
            }

            SetAnimaton(velocity);

            _currentAnimation.Update(gameTime);
        }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 position = new Vector2(PosX, PosY);
        var sourceRectangle = _currentAnimation.CurrentRectangle;
        spriteBatch.Draw(TextureSheet, position, sourceRectangle, Color.White);
    }

    private Vector2 GetVelocity()
    {
        Vector2 velocity = new Vector2();

        TouchCollection touchCollection = TouchPanel.GetState();

        // if there is touch input
        if (touchCollection.Count > 0)
        {
            var touchPoint = ScaleInput(touchCollection[0].Position);
            velocity.X = touchPoint.X - (720 / 2);
            velocity.Y = touchPoint.Y - (1280 / 2);

            if (velocity.X != 0 || velocity.Y != 0)
            {
                velocity.Normalize();
                const float desiredSpeed = 170;
                velocity *= desiredSpeed;
            }
        }

        return velocity;
    }

    private void SetAnimaton(Vector2 velocity)
    {
        if (velocity != Vector2.Zero)
        {
            var movingHorizontally = Math.Abs(velocity.X) > Math.Abs(velocity.Y);
            _currentAnimation =
                movingHorizontally
                    ? (velocity.X > 0 ? _walkRight : _walkLeft)
                    : (velocity.Y > 0 ? _walkDown : _walkUp);
        }
        else
        {
            if (_currentAnimation == _walkUp)
                _currentAnimation = _standUp;
            else if (_currentAnimation == _walkDown)
                _currentAnimation = _standDown;
            else if (_currentAnimation == _walkLeft)
                _currentAnimation = _standLeft;
            else if (_currentAnimation == _walkRight)
                _currentAnimation = _standRight;
            else if (_currentAnimation == null)
                _currentAnimation = _standDown;
        }
    }

    private Vector2 ScaleInput(Vector2 vector)
    {
        return Vector2.Transform(vector, Matrix.Invert(_scale));
    }

    public bool Collides()
    {
        foreach (Rectangle rect in CollisionObjects)
            if (rect.Intersects(this.Rectangle))
            {
                currentCollision = rect;
                return true;
            }
        return false;
    }

    public bool CollidesTop()
    {
        return Rectangle.Bottom + CurrentVelocity.Y > currentCollision.Top &&
               Rectangle.Top < currentCollision.Top &&
               Rectangle.Right > currentCollision.Left &&
               Rectangle.Left < currentCollision.Right;
    }

    public bool CollidesBottom()
    {
        return Rectangle.Top + CurrentVelocity.Y < currentCollision.Bottom &&
               Rectangle.Bottom > currentCollision.Bottom &&
               Rectangle.Right > currentCollision.Left &&
               Rectangle.Left < currentCollision.Right;
    }

    public bool CollidesRight()
    {
        return Rectangle.Left + CurrentVelocity.X < currentCollision.Right &&
               Rectangle.Right > currentCollision.Right &&
               Rectangle.Bottom > currentCollision.Top &&
               Rectangle.Top < currentCollision.Bottom;
    }

    public bool CollidesLeft()
    {
        return Rectangle.Right + CurrentVelocity.X > currentCollision.Left &&
               Rectangle.Left < currentCollision.Left &&
               Rectangle.Bottom > currentCollision.Top &&
               Rectangle.Top < currentCollision.Bottom;
    }

}
}
