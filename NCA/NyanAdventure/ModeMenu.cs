using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class ModeMenu
{
	private MenuItem titleItem_;

	private int index_;

	private int maxIndex_ = 4;

	private float scale_ = 1f;

	private float scaleRate_ = 1f;

	private string[] modeNames_ = new string[5] { "Party", "Jump", "Gravitate", "Fly", "Toast" };

	private Color[] itemColors_ = new Color[5]
	{
		new Color(255, 0, 192),
		new Color(255, 128, 0),
		new Color(120, 255, 0),
		new Color(255, 0, 0),
		new Color(255, 240, 0)
	};

	public ModeMenu()
	{
		this.titleItem_ = new MenuItem("Select Mode", new Vector2(640f, 70f), Color.White);
		this.titleItem_.DefaultColor = new Color(0, 246, 255);
	}

	public void Update(float dt, ref MenuState menuState, ref int modeIndex, Character character, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		this.scale_ += this.scaleRate_ * dt;
		if (this.scale_ > 1f)
		{
			this.scale_ = 1f;
			this.scaleRate_ = 0f - this.scaleRate_;
		}
		if (this.scale_ < 0.8f)
		{
			this.scale_ = 0.8f;
			this.scaleRate_ = 0f - this.scaleRate_;
		}
		if ((currKState.IsKeyDown(Keys.Space) && prevKState.IsKeyUp(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A)))
		{
			modeIndex = this.index_;
			menuState = MenuState.INSTRUCTIONS;
			Global.PlayMenuSelect();
			this.index_ = 0;
		}
		if ((currKState.IsKeyDown(Keys.B) && prevKState.IsKeyUp(Keys.B)) || (prevPState.IsButtonUp(Buttons.B) && currPState.IsButtonDown(Buttons.B)))
		{
			menuState = MenuState.SELECTCHAR;
			Global.PlayMenuBack();
			this.index_ = 0;
		}
		if (((currKState.IsKeyDown(Keys.Down) && prevKState.IsKeyUp(Keys.Down)) || (currPState.IsButtonDown(Buttons.DPadDown) && prevPState.IsButtonUp(Buttons.DPadDown)) || (currPState.ThumbSticks.Left.Y < -0.5f && prevPState.ThumbSticks.Left.Y >= -0.5f)) && this.index_ < this.maxIndex_)
		{
			this.index_++;
			character.PositionX = 270f;
			character.PositionY = 100 + 84 * (this.index_ + 1);
			character.SetDemonstration(this.index_);
			Global.PlayMenuScroll();
		}
		if (((currKState.IsKeyDown(Keys.Up) && prevKState.IsKeyUp(Keys.Up)) || (currPState.IsButtonDown(Buttons.DPadUp) && prevPState.IsButtonUp(Buttons.DPadUp)) || (currPState.ThumbSticks.Left.Y > 0.5f && prevPState.ThumbSticks.Left.Y <= 0.5f)) && this.index_ > 0)
		{
			this.index_--;
			character.PositionX = 270f;
			character.PositionY = 100 + 84 * (this.index_ + 1);
			character.SetDemonstration(this.index_);
			Global.PlayMenuScroll();
		}
		character.Demonstrate(this.index_, dt);
		character.UpdateWithTrail(dt);
	}

	public void Draw(SpriteBatch spriteBatch, Background background, Character character)
	{
		background.DrawStars(spriteBatch);
		this.titleItem_.Draw(spriteBatch, selected: false);
		spriteBatch.Draw(Global.aButtonTex, new Vector2(128f, 606f), Color.White);
        spriteBatch.DrawString(Global.menuFontTex, "Select", new Vector2(190f, 586f + 20), Color.White);
        spriteBatch.Draw(Global.bButtonTex, new Vector2(1110f, 606f), Color.White);
        spriteBatch.DrawString(Global.menuFontTex, "Back", new Vector2(980f, 586f + 20), Color.White);
        for (int i = 0; i < 5; i++)
		{
			if (i == this.index_)
			{
				spriteBatch.DrawString(Global.menuFontTex, this.modeNames_[i], new Vector2(640f, 100 + 84 * (i + 1)), this.itemColors_[i], 0f, Global.menuFontTex.MeasureString(this.modeNames_[i]) / 2f, this.scale_, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.DrawString(Global.menuFontTex, this.modeNames_[i], new Vector2(640f, 100 + 84 * (i + 1)), Color.White, 0f, Global.menuFontTex.MeasureString(this.modeNames_[i]) / 2f, 0.8f, SpriteEffects.None, 0f);
			}
		}
		character.Draw(spriteBatch);
		character.DrawMirrored(spriteBatch);
		if (this.index_ == 4)
		{
			spriteBatch.Draw(Global.obstacleTex, new Vector2(120f, 520f), (Rectangle?)new Rectangle(200, 0, 120, 89), Color.White, (float)Math.PI / 2f, new Vector2(60f, 45f), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.obstacleTex, new Vector2(420f, 520f), (Rectangle?)new Rectangle(200, 0, 120, 89), Color.White, 4.712389f, new Vector2(60f, 45f), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.obstacleTex, new Vector2(1160f, 520f), (Rectangle?)new Rectangle(200, 0, 120, 89), Color.White, 4.712389f, new Vector2(60f, 45f), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.obstacleTex, new Vector2(860f, 520f), (Rectangle?)new Rectangle(200, 0, 120, 89), Color.White, (float)Math.PI / 2f, new Vector2(60f, 45f), 1f, SpriteEffects.None, 0f);
		}
	}

	public void GainFocus(Character character)
	{
		character.PositionX = 270f;
		character.PositionY = 100 + 84 * (this.index_ + 1);
		character.SetDemonstration(this.index_);
	}
}
