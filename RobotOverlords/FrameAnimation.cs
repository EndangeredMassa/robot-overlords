using Microsoft.Xna.Framework;

namespace RobotOverlords
{
    /// <summary>
    /// The FrameAnimation class is used to describe the animated frames
    /// on a sprite sheet by rectangles associated with the frame number.
    /// 
    /// The class tracks only the initial frame of the animation and 
    /// calculates the current frame by offsetting the X component of the
    /// rectangle by the sprite width multipled by the frame number.
    /// </summary>
    public class FrameAnimation
    {
        #region Declarations

        // The first frame of the Animation.  We will calculate other
        // frames on the fly based on this frame.
        private Rectangle _initialFrame;

        // Number of frames in the Animation
        private int _frameCount = 1;

        // The frame currently being displayed.  
        // This value ranges from 0 to iFrameCount-1
        private int _currentFrame = 0;

        // Amount of time (in seconds) to display each frame
        private float _frameLength = 0.2f;

        // Amount of time that has passed since we last animated
        private float _frameTimer = 0.0f;

        // The number of times this animation has been played
        private int _playCount = 0;

        // The animation that should be played after this animation
        private string _nextAnimation = null;

        #endregion

        #region Properties
        /// <summary>
        /// The number of frames the animation contains
        /// </summary>
        public int FrameCount 
        {
            get { return _frameCount;}
            set { _frameCount = value; }
        }

        /// <summary>
        /// The time (in seconds) to display each frame
        /// </summary>
        public float FrameLength
        {
            get { return _frameLength; }
            set { _frameLength = value; }
        }

        /// <summary>
        /// The frame number currently being displayed
        /// </summary>
        public int CurrentFrame
        {
            get { return _currentFrame; }
            set { _currentFrame = (int)MathHelper.Clamp(value, 0, _frameCount - 1); }
        }

        public int FrameWidth
        {
            get { return _initialFrame.Width; }
        }

        public int FrameHeight
        {
            get { return _initialFrame.Height; }
        }

        /// <summary>
        /// The rectangle associated with the current
        /// animation frame.
        /// </summary>
        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle(
                    _initialFrame.X + (_initialFrame.Width * _currentFrame),
                    _initialFrame.Y, _initialFrame.Width, _initialFrame.Height);
            }
        }

        public int PlayCount
        {
            get { return _playCount; }
            set { _playCount = value; }
        }

        public string NextAnimation
        {
            get { return _nextAnimation; }
            set { _nextAnimation = value; }
        }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Creates a new frame animation.
        /// </summary>
        /// <param name="firstFrame">Rectangle representing the location of the first frame.</param>
        /// <param name="frames">Integer holding the number of frames in the animation.</param>
        public FrameAnimation(Rectangle firstFrame, int frames)
        {
            _initialFrame = firstFrame;
            _frameCount = frames;
        }

        /// <summary>
        /// Creates a new frame animation.
        /// </summary>
        /// <param name="x">Leftmost pixel of the first frame.</param>
        /// <param name="y">Topmost pixel of the first frame.</param>
        /// <param name="width">Width of the animation frames.</param>
        /// <param name="height">Height of the animation frames.</param>
        /// <param name="frames">Integer holding the number of frames in the animation.</param>
        public FrameAnimation(int x, int y, int width, int height, int frames)
        {
            _initialFrame = new Rectangle(x, y, width, height);
            _frameCount = frames;
        }

        /// <summary>
        /// Creates a new frame animation.
        /// </summary>
        /// <param name="x">Leftmost pixel of the first frame.</param>
        /// <param name="y">Topmost pixel of the first frame.</param>
        /// <param name="width">Width of the animation frames.</param>
        /// <param name="height">Height of the animation frames.</param>
        /// <param name="frames">Integer holding the number of frames in the animation.</param>
        /// <param name="frameLength">Time (in seconds) to display each frame.</param>
        public FrameAnimation(int x, int y, int width, int height, int frames, float frameLength)
        {
            _initialFrame = new Rectangle(x, y, width, height);
            _frameCount = frames;
            _frameLength = frameLength;
        }

        /// <summary>
        /// Creates a new frame animation
        /// </summary>
        /// <param name="x">Leftmost pixel of the first frame.</param>
        /// <param name="y">Topmost pixel of the first frame.</param>
        /// <param name="width">Width of the animation frames.</param>
        /// <param name="height">Height of the animation frames.</param>
        /// <param name="frames">Integer holding the number of frames in the animation.</param>
        /// <param name="frameLength">Time (in seconds) to display each frame.</param>
        /// <param name="nextAnimation">The name of the animation after this animation.</param>
        public FrameAnimation(int x, int y, int width, int height, int frames, float frameLength, string nextAnimation)
        {
            _initialFrame = new Rectangle(x, y, width, height);
            _frameCount = frames;
            _frameLength = frameLength;
            _nextAnimation = nextAnimation;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the current frame based on elapsed game time and frame length
        /// </summary>
        /// <param name="gameTime">GameTime value representing the current game time.</param>
        public void Update(GameTime gameTime)
        {
            _frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_frameTimer > _frameLength)
            {
                _frameTimer = 0.0f;
                _currentFrame = (_currentFrame + 1) % _frameCount;
                if (_currentFrame == 0)
                    _playCount = (int)MathHelper.Min(_playCount + 1, int.MaxValue);
            }
        }

        public FrameAnimation Clone()
        {
            return new FrameAnimation(_initialFrame.X, _initialFrame.Y,
                                      _initialFrame.Width, _initialFrame.Height,
                                      _frameCount, _frameLength, _nextAnimation);
        }

        #endregion
    }
}
