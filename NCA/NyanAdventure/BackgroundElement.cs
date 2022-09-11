using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class BackgroundElement : AnimatedSprite
{
	private Vector2 velocity_;

	private float rotationRate_;

	private float minPositionX_;

	private float maxPositionX_;

	private float deploymentTimer_;

	private float deploymentTime_;

	private float minDeploymentTime_;

	private float maxDeploymentTime_;

	private float minPositionY_;

	private float maxPositionY_;

	private float minVelocityX_;

	private float maxVelocityX_;

	public BackgroundElement(bool initialize, Texture2D texture, Rectangle? sourceRect, int frameCount, float frameTime, float minDeploymentTime, float maxDeploymentTime, float minPosY, float maxPosY, float minVelX, float maxVelX, float rotationRate)
		: base(texture, sourceRect, frameCount, frameTime, Vector2.Zero)
	{
		this.minPositionX_ = 0f - base.origin_.X;
		this.maxPositionX_ = Global.ScreenSize.X + base.origin_.X;
		this.minDeploymentTime_ = minDeploymentTime;
		this.maxDeploymentTime_ = maxDeploymentTime;
		this.minPositionY_ = minPosY + base.origin_.Y;
		this.maxPositionY_ = maxPosY - base.origin_.Y;
		this.minVelocityX_ = minVelX;
		this.maxVelocityX_ = maxVelX;
		this.rotationRate_ = rotationRate;
		if (initialize)
		{
			this.Initialize();
		}
		else
		{
			this.Deploy();
		}
	}

	public new void Update(float dt)
	{
		if (base.frameCount_ != 1)
		{
			base.Update(dt);
		}
		if (base.position_.X < this.minPositionX_ || base.position_.X > this.maxPositionX_)
		{
			base.visible_ = false;
			this.deploymentTimer_ += dt;
			if (this.deploymentTimer_ > this.deploymentTime_)
			{
				this.Deploy();
			}
		}
		else
		{
			base.rotation_ += this.rotationRate_ * dt;
			base.position_ += new Vector2(this.velocity_.X, this.velocity_.Y) * dt;
		}
	}

	private void Initialize()
	{
		base.position_.X = Global.RandomBetween(this.minPositionX_, this.maxPositionX_);
		base.position_.Y = Global.RandomBetween(this.minPositionY_, this.maxPositionY_);
		this.velocity_ = new Vector2(Global.RandomBetween(this.minVelocityX_, this.maxVelocityX_), 0f);
		this.deploymentTime_ = Global.RandomBetween(this.minDeploymentTime_, this.maxDeploymentTime_);
		this.deploymentTimer_ = 0f;
		base.visible_ = true;
	}

	private void Deploy()
	{
		base.position_.Y = Global.RandomBetween(this.minPositionY_, this.maxPositionY_);
		this.velocity_ = new Vector2(Global.RandomBetween(this.minVelocityX_, this.maxVelocityX_), 0f);
		if (Math.Abs(this.velocity_.X) < 0f)
		{
			base.position_.X = this.maxPositionX_;
		}
		else if (Math.Sign(this.velocity_.X) == 1)
		{
			base.position_.X = this.minPositionX_;
		}
		else
		{
			base.position_.X = this.maxPositionX_;
		}
		this.deploymentTime_ = Global.RandomBetween(this.minDeploymentTime_, this.maxDeploymentTime_);
		this.deploymentTimer_ = 0f;
		base.visible_ = true;
	}
}
