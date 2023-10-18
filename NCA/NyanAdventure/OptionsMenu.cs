using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class OptionsMenu : Menu
{
	private bool sfxOn = true;

	private bool musicOn = true;

	public OptionsMenu()
		: base("Options")
	{
		base.AddMenuItem(new MenuItem("Sound FX", new Vector2(580f, 380f), new Color(255, 0, 192), 838f));
		base.AddMenuItem(new MenuItem("Music", new Vector2(630f, 470f), new Color(120, 255, 0), 838f));
		base.AddMenuItem(new MenuItem("Vibration", new Vector2(588f, 560f), new Color(120, 255, 0), 838f));
		base.AddMenuItem(new MenuItem("Back", new Vector2(640f, 650f), new Color(255, 240, 0)));
	}

	public void Update(float dt, ref MenuState menuState, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		if ((currKState.IsKeyDown(Keys.Left) && prevKState.IsKeyUp(Keys.Left)) || (prevPState.IsButtonUp(Buttons.DPadLeft) && currPState.IsButtonDown(Buttons.DPadLeft)) || (currPState.ThumbSticks.Left.X < -0.5f && prevPState.ThumbSticks.Left.X >= -0.5f))
		{
			switch (base.index_)
			{
			case 0:
				this.sfxOn = true;
				Global.SFXVolume = (this.sfxOn ? 1 : 0);
				break;
			case 1:
				this.musicOn = true;
				Game1.songManager_.Volume = (this.musicOn ? 1 : 0);
				break;
			case 2:
				Global.vibrationOn = true;
				break;
			}
			Global.PlayMenuSelect();
		}
		if ((currKState.IsKeyDown(Keys.Right) && prevKState.IsKeyUp(Keys.Right)) || (prevPState.IsButtonUp(Buttons.DPadRight) && currPState.IsButtonDown(Buttons.DPadRight)) || (currPState.ThumbSticks.Left.X > 0.5f && prevPState.ThumbSticks.Left.X <= 0.5f))
		{
			switch (base.index_)
			{
			case 0:
				this.sfxOn = false;
				Global.SFXVolume = (this.sfxOn ? 1 : 0);
				break;
			case 1:
				this.musicOn = false;
				Game1.songManager_.Volume = (this.musicOn ? 1 : 0);
				break;
			case 2:
				Global.vibrationOn = false;
				break;
			}
			Global.PlayMenuSelect();
		}
		if ((currKState.IsKeyDown(Keys.Space) && prevKState.IsKeyUp(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A)))
		{
			switch (base.index_)
			{
			case 0:
				this.sfxOn = !this.sfxOn;
				Global.SFXVolume = (this.sfxOn ? 1 : 0);
				break;
			case 1:
				this.musicOn = !this.musicOn;
				Game1.songManager_.Volume = (this.musicOn ? 1 : 0);
				break;
			case 2:
				Global.vibrationOn = !Global.vibrationOn;
				break;
			case 3:
				menuState = MenuState.MAIN;
				break;
			}
			if (base.index_ == 3)
			{
				base.index_ = 0;
				Global.PlayMenuBack();
			}
			else
			{
				Global.PlayMenuSelect();
			}
		}
		if ((currKState.IsKeyDown(Keys.B) && prevKState.IsKeyUp(Keys.B)) || (prevPState.IsButtonUp(Buttons.B) && currPState.IsButtonDown(Buttons.B)))
		{
			menuState = MenuState.MAIN;
			Global.PlayMenuBack();
			base.index_ = 0;
		}
		base.Update(dt, currKState, prevKState, currPState, prevPState);
	}

	public new void Draw(SpriteBatch spriteBatch, Background background)
	{
		spriteBatch.Draw(Global.sliderBarTex, new Vector2(770f, base.menuItems_[0].Position.Y+7), (Rectangle?)null, Color.White, 0f, new Vector2(Global.sliderBarTex.Width / 2, Global.sliderBarTex.Height / 2), 1f, SpriteEffects.None, 0f);
		spriteBatch.Draw(Global.sliderBarTex, new Vector2(770f, base.menuItems_[1].Position.Y+7), (Rectangle?)null, Color.White, 0f, new Vector2(Global.sliderBarTex.Width / 2, Global.sliderBarTex.Height / 2), 1f, SpriteEffects.None, 0f);
		spriteBatch.Draw(Global.sliderBarTex, new Vector2(770f, base.menuItems_[2].Position.Y+7), (Rectangle?)null, Color.White, 0f, new Vector2(Global.sliderBarTex.Width / 2, Global.sliderBarTex.Height / 2), 1f, SpriteEffects.None, 0f);
		if (this.sfxOn)
		{
			spriteBatch.Draw(Global.onBarTex, new Vector2(721f, base.menuItems_[0].Position.Y - 22f+7), Color.White);
		}
		else
		{
			spriteBatch.Draw(Global.offBarTex, new Vector2(770f, base.menuItems_[0].Position.Y - 22f + 7), Color.White);
		}
		if (this.musicOn)
		{
			spriteBatch.Draw(Global.onBarTex, new Vector2(721f, base.menuItems_[1].Position.Y - 22f + 7), Color.White);
		}
		else
		{
			spriteBatch.Draw(Global.offBarTex, new Vector2(770f, base.menuItems_[1].Position.Y - 22f + 7), Color.White);
		}
		if (Global.vibrationOn)
		{
			spriteBatch.Draw(Global.onBarTex, new Vector2(721f, base.menuItems_[2].Position.Y - 22f + 7), Color.White);
		}
		else
		{
			spriteBatch.Draw(Global.offBarTex, new Vector2(770f, base.menuItems_[2].Position.Y - 22f + 7), Color.White);
		}
		base.Draw(spriteBatch, background);
	}
}
