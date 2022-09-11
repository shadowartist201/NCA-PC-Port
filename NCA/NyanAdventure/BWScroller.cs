using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class BWScroller
{
	private float scrollPosition_ = 0f;

	private float scrollSpeed_ = 600f;

	private Vector2 basePosition_ = new Vector2(-2560f, 0f);

	private float width_;

	public BWScroller()
	{
		this.width_ = Global.bwBackgroundTex.Width;
	}

	public void Update(float dt)
	{
		if (this.basePosition_.X > 0f)
		{
			this.basePosition_.X -= this.scrollSpeed_ * dt;
			this.scrollPosition_ = 0f;
			if (this.basePosition_.X < 0f)
			{
				this.basePosition_.X = 0f;
			}
		}
		else if (this.basePosition_.X < 0f)
		{
			this.basePosition_.X -= this.scrollSpeed_ * dt;
			if (this.basePosition_.X < -2560f)
			{
				this.basePosition_.X = -2560f;
			}
		}
		else
		{
			this.scrollPosition_ += this.scrollSpeed_ * dt;
			if (this.scrollPosition_ > this.width_)
			{
				this.scrollPosition_ = 0f;
			}
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		if (this.basePosition_.X > 0f)
		{
			this.scrollSpeed_ = 3600f;
			spriteBatch.Draw(Global.bwTransitionTex, this.basePosition_ - new Vector2(1280f, 0f), (Rectangle?)null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
			spriteBatch.Draw(Global.bwBackgroundTex, this.basePosition_, (Rectangle?)new Rectangle(0, 0, 1280, 720), Color.White);
		}
		else if (this.basePosition_.X < 0f)
		{
			this.scrollSpeed_ = 3600f;
			spriteBatch.Draw(Global.bwTransitionTex, this.basePosition_ + new Vector2(1280f, 0f), (Rectangle?)null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			if (this.scrollPosition_ > this.width_ - 1280f)
			{
				int num = (int)(this.width_ - this.scrollPosition_);
				spriteBatch.Draw(Global.bwBackgroundTex, this.basePosition_ + new Vector2(num, 0f), (Rectangle?)new Rectangle(0, 0, 1280 - num, 720), Color.White);
				spriteBatch.Draw(Global.bwBackgroundTex, this.basePosition_, (Rectangle?)new Rectangle((int)this.scrollPosition_, 0, num, 720), Color.White);
			}
			else
			{
				spriteBatch.Draw(Global.bwBackgroundTex, this.basePosition_, (Rectangle?)new Rectangle((int)this.scrollPosition_, 0, 1280, 720), Color.White);
			}
		}
		else
		{
			this.scrollSpeed_ = 600f;
			if (this.scrollPosition_ > this.width_ - 1280f)
			{
				int num = (int)(this.width_ - this.scrollPosition_);
				spriteBatch.Draw(Global.bwBackgroundTex, this.basePosition_ + new Vector2(num, 0f), (Rectangle?)new Rectangle(0, 0, 1280 - num, 720), Color.White);
				spriteBatch.Draw(Global.bwBackgroundTex, this.basePosition_, (Rectangle?)new Rectangle((int)this.scrollPosition_, 0, num, 720), Color.White);
			}
			else
			{
				spriteBatch.Draw(Global.bwBackgroundTex, this.basePosition_, (Rectangle?)new Rectangle((int)this.scrollPosition_, 0, 1280, 720), Color.White);
			}
		}
	}

	public void Begin()
	{
		this.basePosition_ = new Vector2(2560f, 0f);
		this.basePosition_.X -= this.scrollSpeed_ * 0.01667f;
	}

	public void End()
	{
		this.basePosition_ = Vector2.Zero;
		this.basePosition_.X -= this.scrollSpeed_ * 0.016667f;
	}
}
