using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class UI
{
	private float heartScale_ = 0.8f;

	private float heartMinScale_ = 0.8f;

	private float heartMaxScale_ = 1f;

	private float heartScaleRate_ = 1f;

	private bool timing_;

	private float timer_;

	private float time_;

	public void Update(float dt)
	{
		if (this.timing_)
		{
			this.timer_ += dt;
			if (this.timer_ > this.time_)
			{
				this.timing_ = false;
			}
		}
		this.heartScale_ += this.heartScaleRate_ * dt;
		if (this.heartScale_ < this.heartMinScale_)
		{
			this.heartScale_ = this.heartMinScale_;
			this.heartMaxScale_ = 1f;
			this.heartScaleRate_ = 1f;
		}
		if (this.heartScale_ > this.heartMaxScale_)
		{
			this.heartScale_ = this.heartMaxScale_;
			this.heartScaleRate_ = 0f - this.heartScaleRate_;
		}
	}

	public void Draw(SpriteBatch spriteBatch, int health)
	{
		int num = health / 2;
		int num2 = ((health % 2 == 1) ? 1 : 0);
		int i = 0;
		for (int j = 0; j < num; j++)
		{
			spriteBatch.Draw(Global.healthTex, new Vector2(120 + i * 50, 70f), (Rectangle?)new Rectangle(0, 0, 39, 36), Color.White, 0f, new Vector2(19.5f, 18f), this.heartScale_, SpriteEffects.None, 0f);
			i++;
		}
		for (int j = 0; j < num2; j++)
		{
			spriteBatch.Draw(Global.healthTex, new Vector2(120 + i * 50, 70f), (Rectangle?)new Rectangle(39, 0, 39, 36), Color.White, 0f, new Vector2(19.5f, 18f), this.heartScale_, SpriteEffects.None, 0f);
			i++;
		}
		for (; i < 3; i++)
		{
			spriteBatch.Draw(Global.healthTex, new Vector2(120 + i * 50, 70f), (Rectangle?)new Rectangle(78, 0, 39, 36), Color.White, 0f, new Vector2(19.5f, 18f), this.heartScale_, SpriteEffects.None, 0f);
		}
		if (this.timing_)
		{
			spriteBatch.Draw(Global.timerOutlineTex, new Vector2(640 - Global.timerOutlineTex.Width / 2, 70 - Global.timerOutlineTex.Height / 2), Color.White);
			spriteBatch.Draw(Global.timerInsideTex, new Vector2(640 - Global.timerInsideTex.Width / 2, 70 - Global.timerInsideTex.Height / 2), (Rectangle?)new Rectangle(0, 0, (int)((float)Global.timerInsideTex.Width * this.timer_ / this.time_), Global.timerInsideTex.Height), Color.White);
		}
	}

	public void AddHeart(int health)
	{
		this.heartMaxScale_ = 1.4f;
		this.heartScaleRate_ = 2f;
	}

	public void SetTimer(float time)
	{
		this.timing_ = true;
		this.timer_ = 0f;
		this.time_ = time;
	}

	public void Reset()
	{
		this.timing_ = false;
	}
}
