using System.Collections.Generic;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class TrialPauseMenu
{
	private int index_;

	private List<MenuItem> menuItems_ = new List<MenuItem>();

	private float startTimer_ = 0f;

	private float startTime_ = 1.2f;

	public TrialPauseMenu()
	{
		this.menuItems_.Add(new MenuItem("Buy", new Vector2(640f, 360f), Color.Orange, 461, 819));
		this.menuItems_.Add(new MenuItem("Resume", new Vector2(640f, 420f), Color.Yellow, 461, 819));
	}

	public void Update(float dt, ref MenuState menuState, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		this.startTimer_ += dt;
		if (this.menuItems_.Count != 2 && Global.IsTrialMode)
		{
			this.menuItems_.Clear();
			this.menuItems_.Add(new MenuItem("Buy", new Vector2(640f, 360f), Color.Orange, 461, 819));
			this.menuItems_.Add(new MenuItem("Resume", new Vector2(640f, 420f), Color.Yellow, 461, 819));
			this.index_ = 0;
		}
		if (this.menuItems_.Count != 1 && !Global.IsTrialMode)
		{
			this.menuItems_.Clear();
			this.menuItems_.Add(new MenuItem("Resume", new Vector2(640f, 360f), Color.Yellow, 461, 819));
			this.index_ = 0;
		}
		if (this.startTimer_ > this.startTime_)
		{
			if ((currKState.IsKeyDown(Keys.Space) && prevKState.IsKeyUp(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A)))
			{
				switch (this.index_)
				{
				case 0:
					if (!Global.IsTrialMode)
					{
						Game1.songManager_.Resume();
						Global.SetVibrationPaused(paused: false);
						menuState = MenuState.PLAY;
						this.index_ = 0;
						Global.PlayMenuSelect();
					}
					else if (Global.CanBuyGame())
					{
						//Guide.ShowMarketplace(Global.PlayerIndex.Value);
						Global.PlayMenuSelect();
					}
					else
					{
						Global.PlayMenuBack();
					}
					break;
				case 1:
					Game1.songManager_.Resume();
					Global.SetVibrationPaused(paused: false);
					menuState = MenuState.PLAY;
					this.index_ = 0;
					Global.PlayMenuSelect();
					break;
				}
			}
			if ((currKState.IsKeyDown(Keys.B) && prevKState.IsKeyUp(Keys.B)) || (prevPState.IsButtonUp(Buttons.B) && currPState.IsButtonDown(Buttons.B)))
			{
				menuState = MenuState.PLAY;
				Global.PlayMenuBack();
				this.index_ = 0;
			}
			if (((currKState.IsKeyDown(Keys.Down) && prevKState.IsKeyUp(Keys.Down)) || (currPState.IsButtonDown(Buttons.DPadDown) && prevPState.IsButtonUp(Buttons.DPadDown)) || (currPState.ThumbSticks.Left.Y < -0.5f && prevPState.ThumbSticks.Left.Y >= -0.5f)) && this.index_ + 1 < this.menuItems_.Count)
			{
				this.index_++;
				Global.PlayMenuScroll();
			}
			if (((currKState.IsKeyDown(Keys.Up) && prevKState.IsKeyUp(Keys.Up)) || (currPState.IsButtonDown(Buttons.DPadUp) && prevPState.IsButtonUp(Buttons.DPadUp)) || (currPState.ThumbSticks.Left.Y > 0.5f && prevPState.ThumbSticks.Left.Y <= 0.5f)) && this.index_ > 0)
			{
				this.index_--;
				Global.PlayMenuScroll();
			}
		}
		for (int i = 0; i < this.menuItems_.Count; i++)
		{
			this.menuItems_[i].Update(dt, i == this.index_);
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Global.popupTex, new Vector2(454.5f, 263f), Color.White);
		spriteBatch.DrawString(Global.menuFontTex, "Trial Limit Reached", new Vector2(638f, 302f), new Color(255, 0, 192), 0f, Global.menuFontTex.MeasureString("Trial Limit Reached") / 2f, 0.7f, SpriteEffects.None, 0f);
		spriteBatch.DrawString(Global.menuFontTex, "Trial Limit Reached", new Vector2(640f, 300f), Color.White, 0f, Global.menuFontTex.MeasureString("Trial Limit Reached") / 2f, 0.7f, SpriteEffects.None, 0f);
		for (int i = 0; i < this.menuItems_.Count; i++)
		{
			this.menuItems_[i].Draw(spriteBatch, i == this.index_);
		}
	}

	public void ResetStartTimer()
	{
		this.startTimer_ = 0f;
	}
}
