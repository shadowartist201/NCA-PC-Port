using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class Character : AnimatedSprite
{
	private int index_;

	private int[] characterFrameCounts_ = new int[8] { 6, 12, 12, 12, 6, 6, 6, 11 };

	private Rectangle[] characterFrames_ = new Rectangle[8]
	{
		new Rectangle(0, 0, 107, 66),
		new Rectangle(0, 0, 106, 66),
		new Rectangle(0, 0, 116, 80),
		new Rectangle(0, 0, 106, 66),
		new Rectangle(0, 0, 105, 66),
		new Rectangle(0, 0, 131, 66),
		new Rectangle(0, 0, 122, 66),
		new Rectangle(0, 0, 105, 66)
	};

	private float[] characterOriginXs_ = new float[8] { 53f, 53f, 53f, 53f, 53f, 65.5f, 67.5f, 52.5f };

	private float[] characterFrameTimes_ = new float[8] { 0.075f, 0.075f, 0.075f, 0.075f, 0.075f, 0.075f, 0.075f, 0.075f };

	private CharacterTrail characterTrail_;

	private int health_ = 6;

	private int maxHealth_ = 6;

	private float damageTime_ = 1.8f;

	private float damageTimer_ = 1.8f;

	private bool invincible_;

	private float blinkTime_ = 0.05f;

	private float blinkTimer_ = 0f;

	private float pressTime_ = 0.1f;

	private float pressTimer_ = 0.1f;

	private bool gravityReversed_;

	private float flipTimer_ = 100f;

	private float flipTime_ = 0.75f;

	private float scaleTimer_ = 100f;

	private float scaleTime_ = 0.15f;

	private float velocityY_;

	private float minVelocityY_;

	private float maxVelocityY_;

	private float accelerationY_;

	private float velocityX_;

	private bool onPlatform_;

	private bool attached_;

	private Barrel attachedBarrel_;

	private Vector2 demonstrationPosition_;

	private Vector2 demonstrationOffset_;

	private Vector2 demonstrationVelocity_;

	private Vector2 demonstrationAcceleration_;

	private Color[] rainbowColors_ = new Color[6]
	{
		Color.Red,
		Color.Orange,
		Color.Yellow,
		Color.Green,
		Color.Blue,
		Color.Purple
	};

	private int rainbowIndex_;

	private float movementSpeed_ = 4f;

	private float epsilon = 2f;

	private float onPlatformTimer = 0f;

	private float onPlatformTime = 0.03f;

	public int Index => this.index_;

	private MyRectangle CollisionRect => new MyRectangle(base.position_.X - 53f + 10f, base.position_.Y + base.origin_.Y - 66f, 94f, 66f);

	private bool CanTakeDamage => this.damageTimer_ > this.damageTime_;

	public int Health => this.health_;

	public float VelocityX
	{
		set
		{
			this.velocityX_ = value;
		}
	}

	public bool Attached => this.attached_;

	public Character()
		: base(Global.characterTex[0], new Rectangle(0, 0, 107, 66), 12, 0.075f, Vector2.Zero)
	{
		this.characterTrail_ = new CharacterTrail();
		base.position_ = new Vector2(400f, 360f);
		this.SetCharacter(0);
	}

	public void HandleInput(GameMode gameMode, float dt, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		this.characterTrail_.Update(dt, base.Position + new Vector2(0f, (base.frameInfo_.Height - 66) / 2), this.attached_);
		this.pressTimer_ += dt;
		this.flipTimer_ += dt;
		this.scaleTimer_ += dt;
		this.ResetOnPlatform(dt);
		if (this.flipTimer_ < this.flipTime_)
		{
			if (this.flipTimer_ < this.flipTime_ / 2f)
			{
				base.scale_.Y = 1f - this.flipTimer_ / (this.flipTime_ / 2f);
			}
			else
			{
				base.scale_.Y = this.flipTimer_ / (this.flipTime_ / 2f) - 1f;
				if (this.gravityReversed_)
				{
					base.spriteEffects_ = SpriteEffects.FlipVertically;
				}
				else
				{
					base.spriteEffects_ = SpriteEffects.None;
				}
			}
		}
		else
		{
			base.scale_ = Vector2.One;
			if (this.gravityReversed_)
			{
				base.spriteEffects_ = SpriteEffects.FlipVertically;
			}
			else
			{
				base.spriteEffects_ = SpriteEffects.None;
			}
		}
		if (gameMode == GameMode.BARREL)
		{
			if (this.attached_)
			{
				if (this.scaleTimer_ < this.scaleTime_)
				{
					if (this.scaleTimer_ < this.scaleTime_)
					{
						float num = 1f - this.scaleTimer_ / this.scaleTime_;
						base.scale_ = new Vector2(num, num);
					}
				}
				else
				{
					base.scale_ = Vector2.Zero;
				}
			}
			else if (this.scaleTimer_ < this.scaleTime_)
			{
				float num = this.scaleTimer_ / this.scaleTime_;
				base.scale_ = new Vector2(num, num);
			}
			else
			{
				base.scale_ = Vector2.One;
			}
		}
		if (gameMode != GameMode.HELICOPTER)
		{
			Global.StopBoost(dt);
		}
		switch (gameMode)
		{
		case GameMode.PLATFORMER:
			this.minVelocityY_ = -900f;
			this.maxVelocityY_ = 900f;
			this.accelerationY_ = 2400f;
			this.velocityX_ = 0f;
			if ((prevKState.IsKeyUp(Keys.Space) && currKState.IsKeyDown(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A)))
			{
				this.pressTimer_ = 0f;
			}
			if (this.onPlatform_ && this.pressTimer_ < this.pressTime_)
			{
				this.velocityY_ = -900f;
				this.pressTimer_ = this.pressTime_;
				Global.PlayJump();
			}
			break;
		case GameMode.GRAVITY:
			this.minVelocityY_ = -300f;
			this.maxVelocityY_ = 300f;
			this.velocityX_ = 0f;
			if (!this.gravityReversed_)
			{
				this.accelerationY_ = 2400f;
			}
			else
			{
				this.accelerationY_ = -2400f;
			}
			if ((prevKState.IsKeyUp(Keys.Space) && currKState.IsKeyDown(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A)))
			{
				this.pressTimer_ = 0f;
			}
			if (this.onPlatform_ && this.pressTimer_ < this.pressTime_ && this.flipTimer_ > 0.05f)
			{
				this.gravityReversed_ = !this.gravityReversed_;
				this.flipTimer_ = 0f;
				this.accelerationY_ = 0f - this.accelerationY_;
				Global.PlayJump();
			}
			break;
		case GameMode.HELICOPTER:
			this.minVelocityY_ = -260f;
			this.maxVelocityY_ = 260f;
			this.velocityX_ = 0f;
			if (currKState.IsKeyDown(Keys.Space) || currPState.IsButtonDown(Buttons.A))
			{
				this.accelerationY_ = -887.5f;
				Global.PlayBoost(dt);
			}
			else
			{
				this.accelerationY_ = 887.5f;
				Global.StopBoost(dt);
			}
			break;
		case GameMode.BARREL:
			this.minVelocityY_ = -10000f;
			this.maxVelocityY_ = 10000f;
			this.accelerationY_ = 0f;
			if (this.attached_ && this.attachedBarrel_ != null && (this.attachedBarrel_.Automatic || (prevKState.IsKeyUp(Keys.Space) && currKState.IsKeyDown(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A))))
			{
				base.position_ = this.attachedBarrel_.Position;
				this.velocityX_ = MiniGame.tunnelSpeed_ + (float)(1200.0 * Math.Cos(this.attachedBarrel_.Rotation - (float)Math.PI / 2f));
				this.velocityY_ = (float)(1200.0 * Math.Sin(this.attachedBarrel_.Rotation - (float)Math.PI / 2f));
				this.attachedBarrel_.SetAttached(attached: false);
				this.attached_ = false;
				this.flipTimer_ = 0f;
				Global.PlayExplosion();
			}
			break;
		}
	}

	public void SetDemonstration(int index)
	{
		this.invincible_ = false;
		this.characterTrail_.Reset(base.position_);
		this.attached_ = false;
		this.attachedBarrel_ = null;
		base.scale_ = Vector2.One;
		base.spriteEffects_ = SpriteEffects.None;
		base.color_ = Color.White;
		this.demonstrationOffset_ = Vector2.Zero;
		this.demonstrationPosition_ = base.position_;
		switch (index)
		{
		case 0:
			this.gravityReversed_ = true;
			this.flipTimer_ = 0f;
			this.demonstrationVelocity_ = new Vector2(0f, -450f);
			this.demonstrationAcceleration_ = new Vector2(0f, 1200f);
			break;
		case 1:
			this.demonstrationVelocity_ = new Vector2(0f, -900f);
			this.demonstrationAcceleration_ = new Vector2(0f, 4800f);
			break;
		case 2:
			this.flipTimer_ = 0f;
			this.gravityReversed_ = true;
			this.demonstrationVelocity_ = new Vector2(0f, -100f);
			break;
		case 3:
			this.demonstrationVelocity_ = new Vector2(0f, -260f);
			break;
		case 4:
			this.demonstrationVelocity_ = new Vector2(600f, 0f);
			break;
		}
	}

	public void Demonstrate(int index, float dt)
	{
		switch (index)
		{
		case 0:
			this.flipTimer_ += dt * 2f;
			if (this.flipTimer_ < this.flipTime_)
			{
				if (this.flipTimer_ < this.flipTime_ / 2f)
				{
					base.scale_.Y = 1f - this.flipTimer_ / (this.flipTime_ / 2f);
				}
				else
				{
					base.scale_.Y = this.flipTimer_ / (this.flipTime_ / 2f) - 1f;
					if (this.gravityReversed_)
					{
						base.spriteEffects_ = SpriteEffects.FlipVertically;
					}
					else
					{
						base.spriteEffects_ = SpriteEffects.None;
					}
				}
			}
			else
			{
				base.scale_ = Vector2.One;
			}
			this.demonstrationVelocity_ += this.demonstrationAcceleration_ * dt;
			this.demonstrationOffset_ += this.demonstrationVelocity_ * dt;
			if (this.demonstrationVelocity_.Y > 0f && this.gravityReversed_)
			{
				this.gravityReversed_ = false;
				this.flipTimer_ = 0f;
			}
			if (this.demonstrationOffset_.Y > 0f)
			{
				this.demonstrationVelocity_ = new Vector2(0f, -450f);
				this.demonstrationOffset_ = Vector2.Zero;
				this.gravityReversed_ = true;
				this.flipTimer_ = 0f;
			}
			base.position_ = this.demonstrationPosition_ + this.demonstrationOffset_;
			break;
		case 1:
			this.demonstrationVelocity_ += this.demonstrationAcceleration_ * dt;
			this.demonstrationOffset_ += this.demonstrationVelocity_ * dt;
			if (this.demonstrationOffset_.Y > 0f)
			{
				this.demonstrationVelocity_ = new Vector2(0f, -900f);
				this.demonstrationOffset_ = Vector2.Zero;
			}
			base.position_ = this.demonstrationPosition_ + this.demonstrationOffset_;
			break;
		case 2:
			this.flipTimer_ += dt * 2f;
			if (this.flipTimer_ < this.flipTime_)
			{
				if (this.flipTimer_ < this.flipTime_ / 2f)
				{
					base.scale_.Y = 1f - this.flipTimer_ / (this.flipTime_ / 2f);
				}
				else
				{
					base.scale_.Y = this.flipTimer_ / (this.flipTime_ / 2f) - 1f;
					if (this.gravityReversed_)
					{
						base.spriteEffects_ = SpriteEffects.FlipVertically;
					}
					else
					{
						base.spriteEffects_ = SpriteEffects.None;
					}
				}
			}
			else
			{
				base.scale_ = Vector2.One;
			}
			this.demonstrationOffset_ += this.demonstrationVelocity_ * dt;
			if (this.demonstrationOffset_.Y > 0f)
			{
				this.demonstrationVelocity_ = new Vector2(0f, -100f);
				this.demonstrationOffset_ = Vector2.Zero;
				this.gravityReversed_ = !this.gravityReversed_;
				this.flipTimer_ = 0f;
			}
			if (this.demonstrationOffset_.Y < -50f)
			{
				this.demonstrationVelocity_ = new Vector2(0f, 100f);
				this.demonstrationOffset_ = new Vector2(0f, -50f);
				this.gravityReversed_ = !this.gravityReversed_;
				this.flipTimer_ = 0f;
			}
			base.position_ = this.demonstrationPosition_ + this.demonstrationOffset_;
			break;
		case 3:
			this.demonstrationOffset_ += this.demonstrationVelocity_ * dt;
			if (this.demonstrationOffset_.Y > 0f)
			{
				this.demonstrationVelocity_ = new Vector2(0f, -260f);
				this.demonstrationOffset_ = Vector2.Zero;
			}
			if (this.demonstrationOffset_.Y < -50f)
			{
				this.demonstrationVelocity_ = new Vector2(0f, 260f);
				this.demonstrationOffset_ = new Vector2(0f, -50f);
			}
			base.position_ = this.demonstrationPosition_ + this.demonstrationOffset_;
			break;
		case 4:
			this.demonstrationOffset_ += this.demonstrationVelocity_ * dt;
			if (this.demonstrationOffset_.X > 100f)
			{
				this.demonstrationVelocity_ = new Vector2(-600f, 0f);
				this.demonstrationOffset_ = new Vector2(100f, 0f);
			}
			if (this.demonstrationOffset_.X < -100f)
			{
				this.demonstrationVelocity_ = new Vector2(600f, 0f);
				this.demonstrationOffset_ = new Vector2(-100f, 0f);
			}
			base.position_ = this.demonstrationPosition_ + this.demonstrationOffset_;
			break;
		}
	}

	public void Update(float dt, GameMode gameMode, bool paused)
	{
		if (paused)
		{
			this.characterTrail_.Update(dt, base.Position + new Vector2(0f, (base.frameInfo_.Height - 66) / 2), this.attached_);
			Global.TurnOffBoost();
		}
		this.damageTimer_ += dt;
		if (this.damageTimer_ > this.damageTime_)
		{
			base.color_ = Color.White;
			base.Alpha = 1f;
			this.invincible_ = false;
			Game1.songManager_.SetEndSpeedup();
		}
		else
		{
			this.blinkTimer_ += dt;
			if (this.blinkTimer_ > this.blinkTime_)
			{
				if (this.invincible_)
				{
					this.rainbowIndex_ = (this.rainbowIndex_ + 1) % 6;
					base.color_ = this.rainbowColors_[this.rainbowIndex_];
				}
				else if (base.color_.R == byte.MaxValue)
				{
					base.color_ = Color.Black;
				}
				else
				{
					base.color_ = Color.White;
				}
				this.blinkTimer_ -= this.blinkTime_;
			}
			base.Alpha = 0.5f;
		}
		base.Update(dt);
	}

	public void UpdateWithTrail(float dt)
	{
		this.characterTrail_.Update(dt, base.position_ + new Vector2(0f, (base.frameInfo_.Height - 66) / 2), attached: false);
		base.Update(dt);
	}

	public void UpdateXProgress(float dt, GameMode gameMode)
	{
		if (!this.attached_ && gameMode != GameMode.BARREL && base.position_.X + this.movementSpeed_ * dt < 400f)
		{
			base.position_.X += this.movementSpeed_ * dt;
		}
	}

	public void UpdateX(float dt)
	{
		if (this.attached_)
		{
			if (this.attachedBarrel_ != null)
			{
				base.position_.X = this.attachedBarrel_.Position.X;
			}
		}
		else
		{
			base.position_.X += this.velocityX_ * dt;
		}
	}

	public void OffsetTrail(float dx)
	{
		this.characterTrail_.Offset(dx);
	}

	public void UpdateY(float dt)
	{
		if (this.attached_)
		{
			if (this.attachedBarrel_ != null)
			{
				base.position_.Y = this.attachedBarrel_.Position.Y;
			}
			return;
		}
		this.velocityY_ += this.accelerationY_ * dt;
		if (this.velocityY_ < this.minVelocityY_)
		{
			this.velocityY_ = this.minVelocityY_;
		}
		if (this.velocityY_ > this.maxVelocityY_)
		{
			this.velocityY_ = this.maxVelocityY_;
		}
		base.position_.Y += this.velocityY_ * dt;
	}

	public new void Draw(SpriteBatch spriteBatch)
	{
		this.characterTrail_.Draw(spriteBatch, base.scale_, base.color_, this.attached_);
		base.Draw(spriteBatch);
	}

	public new void DrawMirrored(SpriteBatch spriteBatch)
	{
		this.characterTrail_.DrawMirrored(spriteBatch, base.scale_, base.color_, this.attached_);
		base.DrawMirrored(spriteBatch);
	}

	public void Reset(bool fullReset)
	{
		this.invincible_ = false;
		Game1.songManager_.SetNormal();
		base.position_ = new Vector2(400f, 360f);
		this.characterTrail_.Reset(base.position_);
		this.velocityY_ = 0f;
		this.velocityX_ = 0f;
		this.onPlatform_ = false;
		if (fullReset || this.health_ <= 0)
		{
			this.health_ = this.maxHealth_;
		}
		this.gravityReversed_ = false;
		if (this.attached_ && this.attachedBarrel_ != null)
		{
			this.attachedBarrel_.SetAttached(attached: false);
		}
		this.attached_ = false;
		this.attachedBarrel_ = null;
		base.scale_ = Vector2.One;
		base.spriteEffects_ = SpriteEffects.None;
		this.damageTimer_ = 1.8f;
		this.damageTime_ = 1.8f;
		this.flipTimer_ = this.flipTime_;
		this.scaleTimer_ = this.scaleTime_;
	}

	public bool AdjustCollisionCollectable(Collectable collectable)
	{
		return collectable.CollisionRect.Intersects(this.CollisionRect);
	}

	public void AdjustCollisionBarrels(Barrel barrel)
	{
		if (this.attachedBarrel_ != barrel && barrel.CollisionRect.Intersects(this.CollisionRect) && !this.attached_)
		{
			barrel.SetAttached(attached: true);
			this.attachedBarrel_ = barrel;
			this.attached_ = true;
			this.scaleTimer_ = 0f;
			Global.PlayLand();
		}
	}

	public void AdjustCollisionHorizontal(MyRectangle collisionRect, bool deadly)
	{
		if (this.CollisionRect.Intersects(collisionRect))
		{
			if (!deadly)
			{
				base.position_.X = collisionRect.X - 51f - this.epsilon;
				this.velocityX_ = 0f;
			}
			else
			{
				this.DoDamage();
			}
		}
	}

	public void AdjustCollisionVertical(MyRectangle collisionRect, bool deadly)
	{
		MyRectangle collisionRect2 = this.CollisionRect;
		if (!collisionRect2.Intersects(collisionRect))
		{
			return;
		}
		if (!deadly)
		{
			if (collisionRect2.Y + collisionRect2.Height > collisionRect.Y + collisionRect.Height)
			{
				base.position_.Y = collisionRect.Y + collisionRect.Height - (base.origin_.Y - 66f) + 0.001f;
				if (this.gravityReversed_)
				{
					if (!this.onPlatform_)
					{
						Global.PlayLand();
					}
					this.onPlatform_ = true;
					this.onPlatformTimer = 0f;
				}
			}
			else
			{
				base.position_.Y = collisionRect.Y - base.origin_.Y - 0.001f;
				if (!this.gravityReversed_)
				{
					if (!this.onPlatform_)
					{
						Global.PlayLand();
					}
					this.onPlatform_ = true;
					this.onPlatformTimer = 0f;
				}
			}
			this.velocityY_ = 0f;
		}
		else
		{
			this.DoDamage();
		}
	}

	public bool AdjustCollisionScreen(GameMode gameMode)
	{
		if (gameMode == GameMode.BARREL && this.invincible_ && (this.CollisionRect.Y < 0f || this.CollisionRect.Y + this.CollisionRect.Height > 720f) && !this.attached_)
		{
			if (this.CollisionRect.Y + this.CollisionRect.Height > 720f)
			{
				base.position_.Y = 720f - base.origin_.Y - 0.001f;
			}
			else
			{
				base.position_.Y = 66f - base.origin_.Y + 0.001f;
			}
			this.velocityY_ = 0f - this.velocityY_;
			if (this.attachedBarrel_ != null && !this.CollisionRect.Intersects(this.attachedBarrel_.CollisionRect))
			{
				this.attachedBarrel_ = null;
			}
		}
		if (gameMode == GameMode.BARREL && this.invincible_ && (this.CollisionRect.X < 0f || this.CollisionRect.X + this.CollisionRect.Width > 1280f) && !this.attached_)
		{
			if (this.CollisionRect.X + this.CollisionRect.Width > 1280f)
			{
				base.position_.X = 1229f - this.epsilon;
			}
			else
			{
				base.position_.X = 43f + this.epsilon;
			}
			this.velocityX_ = 0f - this.velocityX_;
			if (this.attachedBarrel_ != null && !this.CollisionRect.Intersects(this.attachedBarrel_.CollisionRect))
			{
				this.attachedBarrel_ = null;
			}
		}
		if (!this.CollisionRect.Intersects(new MyRectangle(0f, 0f, Global.ScreenSize.X + 100f, Global.ScreenSize.Y)))
		{
			this.health_ -= 2;
			if (this.health_ <= 0)
			{
				this.health_ = 0;
				Global.PlayDeath();
			}
			else
			{
				Global.PlayHurt();
			}
			return true;
		}
		return false;
	}

	public void AddToHealth(int health)
	{
		if (this.health_ < this.maxHealth_)
		{
			this.health_ += health;
		}
	}

	public void SetInvincible(float invincibleTime)
	{
		this.damageTimer_ = 0f;
		this.damageTime_ = invincibleTime;
		this.invincible_ = true;
	}

	private void DoDamage()
	{
		if (this.CanTakeDamage)
		{
			this.health_--;
			if (this.health_ == 0)
			{
				Global.PlayDeath();
				return;
			}
			this.damageTimer_ = 0f;
			this.damageTime_ = 1.8f;
			Global.PlayHurt();
		}
	}

	private void ResetOnPlatform(float dt)
	{
		if (this.onPlatform_)
		{
			this.onPlatformTimer += dt;
			if (this.onPlatformTimer >= this.onPlatformTime)
			{
				this.onPlatform_ = false;
			}
		}
	}

	public void ResetGravity()
	{
		if (this.gravityReversed_)
		{
			this.flipTimer_ = 0f;
		}
		this.gravityReversed_ = false;
	}

	public void SetCharacter(int index)
	{
		this.index_ = index;
		base.ResetAnimation(Global.characterTex[index], this.characterFrames_[index], this.characterFrameCounts_[index], this.characterFrameTimes_[index]);
		base.origin_.X = this.characterOriginXs_[index];
		switch (index)
		{
		case 0:
			this.characterTrail_.SetTexture(0);
			break;
		case 1:
			this.characterTrail_.SetTexture(1);
			break;
		case 2:
			this.characterTrail_.SetTexture(0);
			break;
		case 3:
			this.characterTrail_.SetTexture(2);
			break;
		case 4:
			this.characterTrail_.SetTexture(0);
			break;
		case 5:
			this.characterTrail_.SetTexture(3);
			break;
		case 6:
			this.characterTrail_.SetTexture(-1);
			break;
		case 7:
			this.characterTrail_.SetTexture(-1);
			break;
		}
		this.characterTrail_.Reset(base.position_);
		base.scale_ = Vector2.One;
		base.spriteEffects_ = SpriteEffects.None;
		base.color_ = Color.White;
	}
}
