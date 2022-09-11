using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class Barrel : AnimatedSprite
{
	private bool automatic_;

	private Vector2 referencePosition_;

	private float deltaVelocity_;

	private float deltaPosition_;

	private float deltaPositionMax_;

	private MovementMode movementMode_;

	private float rotationVelocity_;

	private RotationMode rotationMode_;

	public bool Automatic => this.automatic_;

	public Vector2 ReferencePosition
	{
		get
		{
			return this.referencePosition_;
		}
		set
		{
			this.referencePosition_ = value;
			base.position_ = value;
		}
	}

	public MovementMode MovementMode => this.movementMode_;

	public Vector2 PositionRangeX
	{
		get
		{
			if (this.movementMode_ == MovementMode.HORIZONTAL)
			{
				return new Vector2(this.referencePosition_.X, this.referencePosition_.X + this.deltaPositionMax_);
			}
			return new Vector2(this.referencePosition_.X, this.referencePosition_.X);
		}
	}

	public Vector2 PositionRangeY
	{
		get
		{
			if (this.movementMode_ == MovementMode.VERTICAL)
			{
				return new Vector2(this.referencePosition_.Y, this.referencePosition_.Y + this.deltaPositionMax_);
			}
			return new Vector2(this.referencePosition_.Y, this.referencePosition_.Y);
		}
	}

	public RotationMode RotationMode => this.rotationMode_;

	public MyRectangle CollisionRect
	{
		get
		{
			float num = base.frameInfo_.Height / 2;
			float num2 = base.frameInfo_.Width / 2 - base.frameInfo_.Height / 2;
			return new MyRectangle(base.position_.X - (float)((double)num + (double)num2 * Math.Abs(Math.Cos(base.rotation_))), base.position_.Y - (float)((double)num + (double)num2 * Math.Abs(Math.Sin(base.rotation_))), 2f * (float)((double)num + (double)num2 * Math.Abs(Math.Cos(base.rotation_))), 2f * (float)((double)num + (double)num2 * Math.Abs(Math.Sin(base.rotation_))));
		}
	}

	public Barrel()
		: base(Global.obstacleTex, new Rectangle(200, 0, 120, 89), 1, 0.0333f, Vector2.Zero)
	{
		base.visible_ = false;
	}

	public void Reset(Vector2 referencePosition, MovementMode movementMode, float movementVelocity, float movementPositionMax, RotationMode rotationMode, float rotation, float rotationValue, bool automatic)
	{
		if (!automatic)
		{
			base.frameInfo_ = new Rectangle(200, 0, 120, 89);
			base.frameCount_ = 1;
		}
		else
		{
			base.frameInfo_ = new Rectangle(200, 100, 120, 89);
			base.frameCount_ = 11;
		}
		base.SetSourceRect(base.frameInfo_);
		base.position_ = referencePosition;
		base.rotation_ = rotation;
		this.referencePosition_ = referencePosition;
		this.movementMode_ = movementMode;
		this.deltaPosition_ = 0f;
		this.deltaVelocity_ = movementVelocity;
		this.deltaPositionMax_ = movementPositionMax;
		this.rotationMode_ = rotationMode;
		switch (this.rotationMode_)
		{
		case RotationMode.NONE:
			this.rotationVelocity_ = 0f;
			break;
		case RotationMode.CONSTANT:
			this.rotationVelocity_ = rotationValue;
			break;
		}
		this.automatic_ = automatic;
		base.visible_ = true;
	}

	public void SetAttached(bool attached)
	{
		if (attached)
		{
			base.frameCount_ = 11;
		}
		else if (this.automatic_)
		{
			base.frameCount_ = 11;
		}
		else
		{
			base.frameCount_ = 1;
		}
	}

	public new void Update(float dt)
	{
		this.referencePosition_.X += MiniGame.tunnelSpeed_ * dt;
		this.deltaPosition_ += this.deltaVelocity_ * dt;
		if (this.deltaPosition_ < 0f)
		{
			this.deltaPosition_ = 0f;
			this.deltaVelocity_ = 0f - this.deltaVelocity_;
		}
		if (this.deltaPosition_ > this.deltaPositionMax_)
		{
			this.deltaPosition_ = this.deltaPositionMax_;
			this.deltaVelocity_ = 0f - this.deltaVelocity_;
		}
		switch (this.movementMode_)
		{
		case MovementMode.HORIZONTAL:
			base.position_ = this.referencePosition_ + new Vector2(this.deltaPosition_, 0f);
			break;
		case MovementMode.VERTICAL:
			base.position_ = this.referencePosition_ + new Vector2(0f, this.deltaPosition_);
			break;
		}
		switch (this.rotationMode_)
		{
		case RotationMode.CONSTANT:
			base.rotation_ += this.rotationVelocity_ * dt;
			break;
		}
		if (this.referencePosition_.X + this.deltaPositionMax_ + base.origin_.X < 0f)
		{
			base.visible_ = false;
		}
		base.Update(dt);
	}

	public new void Draw(SpriteBatch spriteBatch)
	{
		if (base.visible_)
		{
			if (this.movementMode_ == MovementMode.VERTICAL)
			{
				Global.DrawRect(spriteBatch, new Rectangle((int)this.referencePosition_.X, (int)this.referencePosition_.Y, 3, (int)this.deltaPositionMax_));
			}
			else
			{
				Global.DrawRect(spriteBatch, new Rectangle((int)this.referencePosition_.X, (int)this.referencePosition_.Y, (int)this.deltaPositionMax_, 3));
			}
		}
		base.Draw(spriteBatch);
	}

	public void Hide()
	{
		base.visible_ = false;
	}
}
