using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RobotOverlords
{
    public class SpriteAnimation
    {
        #region Declarations

        // The texture that holds the images for this sprite
        private readonly Texture2D _texture;

        // True if animations are being played
        private bool _isAnimating = true;

        // If set to anything other than Color.White, will colorize
        // the sprite with that color.
        private Color _colorTint = Color.White;

        // Screen Position of the SpriteAnimation
        private Vector2 _position = new Vector2(0, 0);
        private Vector2 _lastPosition = new Vector2(0, 0);

        // Dictionary holding all of the FrameAnimation objects
        // associated with this sprite.
        private readonly Dictionary<string, FrameAnimation> _frameAnimations = new Dictionary<string, FrameAnimation>();

        // Which FrameAnimation from the dictionary above is playing
        private string _currentAnimation = null;

        // If true, the sprite will automatically rotate to align itself
        // with the angle difference between it's new position and
        // it's previous position.  In this case, the 0 rotation point
        // is to the right (so the sprite should start out facing to
        // the right.
        private bool _isRotateByPosition = false;

        // How much the sprite should be rotated by when drawn
        // Value is in Radians, and 0 indicates no rotation.
        private float _rotation = 0f;
        
        // Calcualted center of the sprite
        private Vector2 _center;

        private float _scale = 1.0f;

        #endregion

        #region Properties

        /// <summary>
        /// Vector2 representing the position of the sprite's upper left
        /// corner pixel.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _lastPosition = _position;
                _position = value;
                UpdateRotation();
            }
        }

        /// <summary>
        /// The X position of the sprite's upper left corner pixel.
        /// </summary>
        public int X
        {
            get { return (int)_position.X; }
            set
            {
                _lastPosition.X = _position.X;
                _position.X = value;
                UpdateRotation();
            }
        }

        /// <summary>
        /// The Y position of the sprite's upper left corner pixel.
        /// </summary>
        public int Y
        {
            get { return (int)_position.Y; }
            set
            {
                _lastPosition.Y = _position.Y;
                _position.Y = value;
                UpdateRotation();
            }
        }

        /// <summary>
        /// Width (in pixels) of the sprite animation frames
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Scaled Width of the sprite animation frames
        /// </summary>
        public int ScaledWidth
        {
            get { return (int)Math.Round(Width * _scale); }
        }

        /// <summary>
        /// Height (in pixels) of the sprite animation frames
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Scaled Height of the sprite animation frames
        /// </summary>
        public int ScaledHeight
        {
            get { return (int)Math.Round(Height * _scale); }
        }

        /// <summary>
        /// Scale (in percent) to render.
        /// </summary>
        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// If true, the sprite will automatically rotate in the direction
        /// of motion whenever the sprite's Position changes.
        /// </summary>
        public bool AutoRotate
        {
            get { return _isRotateByPosition; }
            set { _isRotateByPosition = value; }
        }

        /// <summary>
        /// The degree of rotation (in radians) to be applied to the
        /// sprite when drawn.
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Screen coordinates of the bounding box surrounding this sprite
        /// </summary>
        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, Width, Height); }
        }


        /// <summary>
        /// The texture associated with this sprite.  All FrameAnimations will be
        /// relative to this texture.
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
        }

        /// <summary>
        /// Color value to tint the sprite with when drawing.  Color.White
        /// (the default) indicates no tinting.
        /// </summary>
        public Color Tint
        {
            get { return _colorTint; }
            set { _colorTint = value; }
        }

        /// <summary>
        /// True if the sprite is (or should be) playing animation frames.  If this value is set
        /// to false, the sprite will not be drawn (a sprite needs at least 1 single frame animation
        /// in order to be displayed.
        /// </summary>
        public bool IsAnimating
        {
            get { return _isAnimating; }
            set { _isAnimating = value; }
        }

        /// <summary>
        /// The FrameAnimation object of the currently playing animation
        /// </summary>
        public FrameAnimation CurrentFrameAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentAnimation))
                    return _frameAnimations[_currentAnimation];
                
                return null;
            }
        }

        /// <summary>
        /// The string name of the currently playing animaton.  Setting the animation
        /// resets the CurrentFrame and PlayCount properties to zero.
        /// </summary>
        public string CurrentAnimation
        {
            get { return _currentAnimation; }
            set
            {
                if (_frameAnimations.ContainsKey(value))
                {
                    _currentAnimation = value;
                    _frameAnimations[_currentAnimation].CurrentFrame = 0;
                    _frameAnimations[_currentAnimation].PlayCount = 0;
                }
            }
        }

        #endregion

        #region Constructors

        public SpriteAnimation(Texture2D texture)
        {
            _texture = texture;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the rotation of the sprite based on it's "last known" position
        /// if the "bRotatebyPosition" parameter is true.
        /// </summary>
        void UpdateRotation()
        {
            if (_isRotateByPosition)
            {
                _rotation = (float)Math.Atan2(_position.Y - _lastPosition.Y, _position.X - _lastPosition.X);
            }
        }

        /// <summary>
        /// Add an animation to the animations dictionary.
        /// </summary>
        /// <param name="name">SpriteAnimation-specific name of the animation.</param>
        /// <param name="col">Column number of the spritesheet for the animation</param>
        /// <param name="row">Row number of the spritesheet for the animation</param>
        /// <param name="frames">Number of frames in the animation</param>
        /// <param name="frameLength">Length (in seconds) to display each frame of the animation</param>
        public void AddAnimation(string name, int col, int row, int frames, float frameLength)
        {
            if (_frameAnimations.ContainsKey(name))
                throw new ArgumentException("Animation key already exists: " + name);

            var x = col * Width;
            var y = row * Height;

            _frameAnimations.Add(name, new FrameAnimation(x, y, Width, Height, frames, frameLength));
            _center = new Vector2((float)Width / 2, (float)Height / 2);
        }

        /// <summary>
        /// Add an animation to the animations dictionary.
        /// </summary>
        /// <param name="name">SpriteAnimation-specific name of the animation.</param>
        /// <param name="col">Column number of the spritesheet for the animation</param>
        /// <param name="row">Row number of the spritesheet for the animation</param>
        /// <param name="frames">Number of frames in the animation</param>
        /// <param name="frameLength"></param>
        /// <param name="nextAnimation">Name of the animation to play after this animation ends</param>
        public void AddAnimation(string name, int col, int row, int frames, float frameLength, string nextAnimation)
        {
            if (_frameAnimations.ContainsKey(name))
                throw new ArgumentException("Animation key already exists: " + name);

            var x = col * Width;
            var y = row * Height;

            _frameAnimations.Add(name, new FrameAnimation(x, y, Width, Height, frames, frameLength, nextAnimation));
            _center = new Vector2((float)Width / 2, (float)Height / 2);
        }

        /// <summary>
        /// Returns a FrameAnimation object associated with this sprite via 
        /// the animation name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FrameAnimation GetAnimationByName(string name)
        {
            if (_frameAnimations.ContainsKey(name))
                return _frameAnimations[name];
            
            return null;
        }

        public void MoveBy(int x, int y)
        {
            _lastPosition = _position;
            _position.X += x;
            _position.Y += y;
            UpdateRotation();
        }

        public void Update(GameTime gameTime)
        {
            // Don't do anything if the sprite is not animating
            if (_isAnimating)
            {
                // If there is not a currently active animation
                if (CurrentFrameAnimation == null)
                {
                    // Make sure we have an animation associated with this sprite
                    if (_frameAnimations.Count > 0)
                    {
                        // Set the active animation to the first animation
                        // associated with this sprite
                        var sKeys = new string[_frameAnimations.Count];
                        _frameAnimations.Keys.CopyTo(sKeys, 0);
                        CurrentAnimation = sKeys[0];
                    }
                    else
                    {
                        return;
                    }
                }

                if (CurrentFrameAnimation == null)
                    return;

                // Run the Animation's update method
                CurrentFrameAnimation.Update(gameTime);

                // Check to see if there is a "followup" animation named for this animation
                if (!String.IsNullOrEmpty(CurrentFrameAnimation.NextAnimation))
                {
                    // If there is, see if the currently playing animation has
                    // completed a full animation loop
                    if (CurrentFrameAnimation.PlayCount>0)
                    {
                        // If it has, set up the next animation
                        CurrentAnimation=CurrentFrameAnimation.NextAnimation;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int xOffset, int yOffset)
        {
            if (_isAnimating)
            {
                spriteBatch.Draw(_texture, (_position + new Vector2(xOffset, yOffset) + _center),
                                 CurrentFrameAnimation.FrameRectangle, _colorTint,
                                 _rotation, _center, _scale, SpriteEffects.None, 0);
            }
            
        }

        #endregion

    }
}
