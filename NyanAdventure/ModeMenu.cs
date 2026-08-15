using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NCA_Android;

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

	private Rectangle[] touchBounds = new Rectangle[7]
	{
		new Rectangle(566,148,139,66), //party
		new Rectangle(568,232,138,67), //jump
		new Rectangle(539,319,198,61), //gravity
		new Rectangle(588,402,97,63), //fly
		new Rectangle(570,487,136,61), //toast
		new Rectangle(114,588,231,73), //select
		new Rectangle(965,592,195,67) //back
	};

    public void Update(float dt, ref MenuState menuState, ref int modeIndex, Character character, InputState inputState)
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
		if (inputState.IsButtonPressed(Buttons.A))
		{
			modeIndex = this.index_;
			menuState = MenuState.INSTRUCTIONS;
			Global.PlayMenuSelect();
			this.index_ = 0;
		}
		if (inputState.IsButtonPressed(Buttons.B))
		{
			menuState = MenuState.SELECTCHAR;
			Global.PlayMenuBack();
			this.index_ = 0;
		}
		if (inputState.IsButtonPressed(Buttons.DPadDown) && this.index_ < this.maxIndex_)
		{
			this.index_++;
			character.PositionX = 270f;
			character.PositionY = 100 + 84 * (this.index_ + 1);
			character.SetDemonstration(this.index_);
			Global.PlayMenuScroll();
		}
		if (inputState.IsButtonPressed(Buttons.DPadUp) && this.index_ > 0)
		{
			this.index_--;
			character.PositionX = 270f;
			character.PositionY = 100 + 84 * (this.index_ + 1);
			character.SetDemonstration(this.index_);
			Global.PlayMenuScroll();
		}
        if (touchBounds[0].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched()) 
		{
            modeIndex = 0;
            menuState = MenuState.INSTRUCTIONS;
            Global.PlayMenuSelect();
            this.index_ = 0;
        }
        if (touchBounds[1].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            modeIndex = 1;
            menuState = MenuState.INSTRUCTIONS;
            Global.PlayMenuSelect();
            this.index_ = 0;
        }
        if (touchBounds[2].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            modeIndex = 2;
            menuState = MenuState.INSTRUCTIONS;
            Global.PlayMenuSelect();
            this.index_ = 0;
        }
        if (touchBounds[3].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            modeIndex = 3;
            menuState = MenuState.INSTRUCTIONS;
            Global.PlayMenuSelect();
            this.index_ = 0;
        }
        if (touchBounds[4].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            modeIndex = 4;
            menuState = MenuState.INSTRUCTIONS;
            Global.PlayMenuSelect();
            this.index_ = 0;
        }
        if (touchBounds[5].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            //unimplemented
        }
        if (touchBounds[6].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            menuState = MenuState.SELECTCHAR;
            Global.PlayMenuBack();
            this.index_ = 0;
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
