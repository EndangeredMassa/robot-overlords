using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotOverlords
{
    public class MobileSprite
    {
        #region Declarations

        // The SpriteAnimation object that holds the graphical and animation data for this object
        private readonly SpriteAnimation _spriteAnimation;

        // A queue of pathing vectors to allow the sprite to move along a path
        private readonly Queue<Vector2> _queuePath = new Queue<Vector2>();

        // The location the sprite is currently moving towards
        private Vector2 _target;

        // The speed at which the sprite will close with it's target
        private float _speed = 1f;

        // These two integers represent a clipping range for determining bounding-box style
        // collisions.  They return the bounding box of the sprite trimmed by a horizonal and
        // verticle offset to get a collision cushion
        private int _collisionBufferX = 0;
        private int _collisionBufferY = 0;

        // Determine the status of the sprite.  An inactive sprite will not be updated but will be drawn.
        private bool _isActive = true;

        // Determines if the sprite should track towards a target.  If set to false, the sprite
        // will not move on it's own towards target, and will not process pathing information
        private bool _isMovingTowardsTarget = true;

        // Determines if the sprite will follow the path in it's Path queue.  If true, when the sprite
        // has reached target the next path node will be pulled from the queue and set as
        // the new target.
        private bool _isPathing = true;

        // If true, any pathing node popped from the Queue will be placed back onto the end of the queue
        private bool _isLoopPath = true;

        // If true, the sprite can collide with other objects. This is only provided as a flag
        // for testing with outside code.
        private bool _isCollidable = true;

        // If true, the sprite will be drawn to the screen
        private bool _isVisible = true;

        // If true, the sprite will be deactivated when the Pathing Queue is empty.
        private bool _shouldDeactivateAtEndOfPath = false;

        // If true, isVisible will be set to false when the Pathing Queue is empty.
        private bool _shouldHideAtEndOfPath = false;

        // If set, when the Pathing Queue is empty, the named animation will be set as the
        // current animation on the sprite.
        private string _endPathAnimation = null;
        #endregion

        #region Properties

        public SpriteAnimation SpriteAnimation
        {
            get { return _spriteAnimation; }
        }

        public Vector2 Position
        {
            get { return _spriteAnimation.Position; }
            set { _spriteAnimation.Position = value; }
        }

        public Vector2 Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public int HorizontalCollisionBuffer
        {
            get { return _collisionBufferX; }
            set { _collisionBufferX = value; }
        }

        public int VerticalCollisionBuffer
        {
            get { return _collisionBufferY; }
            set { _collisionBufferY = value; }
        }

        public bool IsPathing
        {
            get { return _isPathing; }
            set { _isPathing = value; }
        }

        public bool DeactivateAfterPathing
        {
            get { return _shouldDeactivateAtEndOfPath; }
            set { _shouldDeactivateAtEndOfPath = value; }
        }

        public bool LoopPath
        {
            get { return _isLoopPath; }
            set { _isLoopPath = value; }
        }

        public string EndPathAnimation
        {
            get { return _endPathAnimation; }
            set { _endPathAnimation = value; }
        }

        public bool HideAtEndOfPath
        {
            get { return _shouldHideAtEndOfPath; }
            set { _shouldHideAtEndOfPath = value; }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public bool IsMoving
        {
            get { return _isMovingTowardsTarget; }
            set { _isMovingTowardsTarget = value; }
        }

        public bool IsCollidable
        {
            get { return _isCollidable; }
            set { _isCollidable = value; }
        }

        public Rectangle BoundingBox
        {
            get { return _spriteAnimation.BoundingBox; }
        }

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    _spriteAnimation.BoundingBox.X + _collisionBufferX,
                    _spriteAnimation.BoundingBox.Y + _collisionBufferY,
                    _spriteAnimation.Width - (2 * _collisionBufferX),
                    _spriteAnimation.Height - (2 * _collisionBufferY));
            }
        }

        #endregion

        #region Constructors
        public MobileSprite(Texture2D texture)
        {
            _spriteAnimation = new SpriteAnimation(texture);
        }
        #endregion

        #region Methods

        public void AddPathNode(Vector2 node)
        {
            _queuePath.Enqueue(node);
        }

        public void AddPathNode(int x, int y)
        {
            _queuePath.Enqueue(new Vector2(x, y));
        }

        public void ClearPathNodes()
        {
            _queuePath.Clear();
        }

        public void Update(GameTime gameTime)
        {
            if (_isActive && _isMovingTowardsTarget)
            {
                // Get a vector pointing from the current location of the sprite
                // to the destination.
                Vector2 delta = new Vector2(_target.X - _spriteAnimation.X, _target.Y - _spriteAnimation.Y);

                if (delta.Length() > Speed)
                {
                    delta.Normalize();
                    delta *= Speed;
                    Position += delta;
                }
                else
                {
                    if (_target == _spriteAnimation.Position && _isPathing && _queuePath.Count > 0)
                    {
                        _target = _queuePath.Dequeue();
                        if (_isLoopPath)
                        {
                            _queuePath.Enqueue(_target);
                        }
                    }
                    else
                    {
                        _spriteAnimation.Position = _target;
                    }

                    if (_endPathAnimation != null)
                    {
                        if (SpriteAnimation.CurrentAnimation != _endPathAnimation)
                        {
                            SpriteAnimation.CurrentAnimation = _endPathAnimation;
                        }
                    }

                    if (_shouldDeactivateAtEndOfPath)
                    {
                        IsActive = false;
                    }

                    if (_shouldHideAtEndOfPath)
                    {
                        IsVisible = false;
                    }
                }
            }
            if (_isActive)
                _spriteAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isVisible)
                _spriteAnimation.Draw(spriteBatch, 0, 0);
        }

        #endregion
    }
}
