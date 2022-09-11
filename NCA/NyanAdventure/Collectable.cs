using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class Collectable : AnimatedSprite
{
	private bool consumed_;

	private float scaleRate_ = 1f;

	private float scaleMin_ = 1f;

	private float scaleMax_ = 1.2f;

	private CollectableType type_;

	public bool Consumable => !this.consumed_;

	public CollectableType Type => this.type_;

	public MyRectangle CollisionRect => new MyRectangle(base.position_.X - base.origin_.X, base.position_.Y - base.origin_.Y, base.frameInfo_.Width, base.frameInfo_.Height);

	public Collectable()
		: base(Global.collectableTex, new Rectangle(0, 0, 0, 0), 1, 0.0333f, Vector2.Zero)
	{
	}

	public new void Update(float dt)
	{
		if (!this.consumed_)
		{
			base.scale_.X += this.scaleRate_ * dt;
			base.scale_.Y = base.scale_.X;
			if (base.scale_.X < this.scaleMin_)
			{
				base.scale_ = new Vector2(this.scaleMin_, this.scaleMin_);
				this.scaleRate_ = 0f - this.scaleRate_;
			}
			if (base.scale_.Y > this.scaleMax_)
			{
				base.scale_ = new Vector2(this.scaleMax_, this.scaleMax_);
				this.scaleRate_ = 0f - this.scaleRate_;
			}
		}
		else
		{
			base.scale_.X += 4f * dt;
			base.scale_.Y = base.scale_.X;
			base.Alpha = (1.5f - base.scale_.X) * 2f;
			base.rotation_ += 12f * dt;
			if (base.scale_.X > 2f)
			{
				base.scale_ = Vector2.One;
				this.consumed_ = false;
				base.visible_ = false;
				return;
			}
		}
		base.Update(dt);
	}

	public void UpdateX(float dt)
	{
		base.position_.X += MiniGame.tunnelSpeed_ * dt;
		if (base.position_.X + base.origin_.X < 0f)
		{
			base.visible_ = false;
		}
	}

	public new void Draw(SpriteBatch spriteBatch)
	{
		if (base.visible_)
		{
		}
		base.Draw(spriteBatch);
	}

	public void Reset(CollectableType type, Vector2 position)
	{
		this.type_ = type;
		base.position_ = position;
		base.scale_ = Vector2.One;
		this.consumed_ = false;
		base.visible_ = true;
		base.Alpha = 1f;
		base.rotation_ = 0f;
		switch (this.type_)
		{
		case CollectableType.NONE:
			base.ResetAnimation(new Rectangle(0, 0, 0, 0), 1, 1f);
			break;
		case CollectableType.POINTS:
			base.ResetAnimation(new Rectangle(396, 0, 45, 45), 1, 1f);
			break;
		case CollectableType.MULTIPLIER_TWO:
			base.ResetAnimation(new Rectangle(52, 0, 80, 39), 1, 1f);
			break;
		case CollectableType.MULTIPLIER_THREE:
			base.ResetAnimation(new Rectangle(132, 0, 80, 39), 1, 1f);
			break;
		case CollectableType.HEALTH:
			base.ResetAnimation(new Rectangle(0, 0, 52, 45), 1, 1f);
			break;
		case CollectableType.INVINCIBILITY:
			base.ResetAnimation(new Rectangle(212, 0, 80, 45), 1, 1f);
			break;
		case CollectableType.SLOWMO:
			base.ResetAnimation(new Rectangle(292, 0, 77, 67), 1, 1f);
			break;
		case CollectableType.POWERDOWN:
			base.ResetAnimation(new Rectangle(Global.Random.Next(0, 10) * 30, 100, 28, 50), 1, 1f);
			break;
		}
	}

	public void Consume()
	{
		this.consumed_ = true;
	}

	public void Hide()
	{
		base.visible_ = false;
	}
}
