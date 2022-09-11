using System.Collections.Generic;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class PauseMenu
{
	private int index_;

	private List<MenuItem> menuItems_ = new List<MenuItem>();

	public PauseMenu()
	{
		this.menuItems_.Add(new MenuItem("Resume", new Vector2(640f, 330f), Color.Yellow, 461, 819));
		this.menuItems_.Add(new MenuItem("Quit", new Vector2(640f, 390f), Color.Yellow, 461, 819));
	}

	public void Update(float dt, ref MenuState menuState, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		if (this.menuItems_.Count != 3 && Global.IsTrialMode)
		{
			this.menuItems_.Clear();
			this.menuItems_.Add(new MenuItem("Resume", new Vector2(640f, 300f), Color.Yellow, 461, 819));
			this.menuItems_.Add(new MenuItem("Buy", new Vector2(640f, 360f), Color.Yellow, 461, 819));
			this.menuItems_.Add(new MenuItem("Quit", new Vector2(640f, 420f), Color.Yellow, 461, 819));
			this.index_ = 0;
		}
		if (this.menuItems_.Count != 2 && !Global.IsTrialMode)
		{
			this.menuItems_.Clear();
			this.menuItems_.Add(new MenuItem("Resume", new Vector2(640f, 330f), Color.Yellow, 461, 819));
			this.menuItems_.Add(new MenuItem("Quit", new Vector2(640f, 390f), Color.Yellow, 461, 819));
			this.index_ = 0;
		}
		if ((currKState.IsKeyDown(Keys.Space) && prevKState.IsKeyUp(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A)))
		{
			switch (this.index_)
			{
			case 0:
				Game1.songManager_.Resume();
				Global.SetVibrationPaused(paused: false);
				menuState = MenuState.PLAY;
				this.index_ = 0;
				Global.PlayMenuSelect();
				break;
			case 1:
				if (!Global.IsTrialMode)
				{
					menuState = MenuState.MAIN;
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
			case 2:
				menuState = MenuState.MAIN;
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
		for (int i = 0; i < this.menuItems_.Count; i++)
		{
			this.menuItems_[i].Update(dt, i == this.index_);
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

	public void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Global.popupTex, new Vector2(454.5f, 263f), Color.White);
		for (int i = 0; i < this.menuItems_.Count; i++)
		{
			this.menuItems_[i].Draw(spriteBatch, i == this.index_);
		}
	}
}
