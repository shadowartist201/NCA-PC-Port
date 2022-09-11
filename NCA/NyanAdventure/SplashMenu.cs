using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class SplashMenu
{
	private float timer_ = 0f;

	private float time_ = 3f;

	private bool playedYeah = false;

	public void Update(float dt, ref MenuState menuState)
	{
		if (!this.playedYeah)
		{
			Global.PlayMenuYeah();
			this.playedYeah = true;
		}
		this.timer_ += dt;
		if (this.timer_ > this.time_)
		{
			menuState = MenuState.INTRO;
		}
	}

	public void Draw(SpriteBatch spriteBatch, Background background)
	{
		background.DrawStars(spriteBatch);
		spriteBatch.Draw(Global.splashBGTex, Vector2.Zero, (Rectangle?)null, Color.White * ((this.timer_ > this.time_ - 0.5f) ? (1f - (this.timer_ - this.time_ + 0.5f) * 2f) : 1f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		spriteBatch.Draw(Global.splashFGTex, new Vector2(640f, 250f), (Rectangle?)null, Color.White * ((this.timer_ > this.time_ - 0.5f) ? (1f - (this.timer_ - this.time_ + 0.5f) * 2f) : (this.timer_ * 2f)), 0f, new Vector2(225f, 233f), 1f, SpriteEffects.None, 0f);
	}
}
