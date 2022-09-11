using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class IntroMenu
{
	private int introState_ = 0;

	private Vector2 characterPosition_ = new Vector2(640f, 360f);

	private float characterRotation_ = 4.712389f;

	private Vector2 handlePosition_ = new Vector2(740f, 404f);

	private float handleVelocity_ = -1000f;

	private float characterVelocity_ = -1000f;

	private Rectangle trailRect_ = new Rectangle(561, 260, 0, 154);

	private float trailProgress_;

	private float trailRotation_ = 4.712389f;

	private float textProgress_;

	private float textProgressRate_ = 10f;

	private int textIndex_;

	private string[] texts_ = new string[2] { "In the year 20XX...", "NYAN CAT GOES ADVENTURE!" };

	private Vector2[] textPositions_ = new Vector2[2];

	public IntroMenu()
	{
		for (int i = 0; i < 2; i++)
		{
			ref Vector2 reference = ref this.textPositions_[i];
			reference = new Vector2(640f, 600f) - Global.menuFontTex.MeasureString(this.texts_[i]) / 2f;
		}
	}

	public void Update(float dt, ref MenuState menuState, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		if ((currKState.IsKeyDown(Keys.Space) && prevKState.IsKeyUp(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A)) || (prevPState.IsButtonUp(Buttons.Start) && currPState.IsButtonDown(Buttons.Start)))
		{
			menuState = MenuState.MAIN;
			Global.PlayExplosion();
		}
		switch (this.introState_)
		{
		case 0:
			this.textProgress_ += this.textProgressRate_ * dt;
			if (this.textProgress_ > (float)(this.texts_[this.textIndex_].Length + 8))
			{
				if (this.textIndex_ == 0)
				{
					this.textProgress_ = 0f;
					this.textProgressRate_ = 12f;
					this.textIndex_++;
				}
				else
				{
					Global.PlayExplosion();
					this.introState_++;
				}
			}
			break;
		case 1:
			this.handlePosition_.Y += this.handleVelocity_ * dt;
			this.characterPosition_.Y += this.characterVelocity_ * dt;
			if (this.handlePosition_.Y < 303f)
			{
				this.handlePosition_.Y = 303f;
			}
			if (this.characterPosition_.Y + 73f < (float)this.trailRect_.Y)
			{
				this.trailProgress_ += 1000f * dt;
				this.trailRect_.Width = (int)this.trailProgress_;
			}
			if (this.trailRect_.Width > 520)
			{
				Global.PlayTransition();
				this.characterPosition_ = new Vector2(640f, 1034f);
				this.trailRect_.Y = 961;
				this.trailRect_.Width = 0;
				this.trailProgress_ = 0f;
				this.introState_++;
			}
			break;
		case 2:
			this.characterPosition_.Y += this.characterVelocity_ * dt;
			this.trailProgress_ += 1000f * dt;
			this.trailRect_.Width = (int)this.trailProgress_;
			if (this.trailRect_.Width > 1234)
			{
				Global.PlayTransition();
				this.characterPosition_ = new Vector2(-314f, 170f);
				this.characterRotation_ = 0f;
				this.introState_++;
			}
			break;
		case 3:
			this.characterPosition_.X -= this.characterVelocity_ * dt;
			if (this.characterPosition_.X - 314f > 1280f)
			{
				Global.PlayExplosion();
				menuState = MenuState.MAIN;
			}
			break;
		}
	}

	public void Draw(SpriteBatch spriteBatch, Background background)
	{
		background.DrawStars(spriteBatch);
		switch (this.introState_)
		{
		case 0:
			spriteBatch.Draw(Global.introToasterTex, new Vector2(640f, 360f), (Rectangle?)null, Color.White, 0f, new Vector2(Global.introToasterTex.Width / 2, Global.introToasterTex.Height / 2), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.introCharTex, this.characterPosition_, (Rectangle?)null, Color.White, this.characterRotation_, new Vector2(Global.introCharTex.Width / 2, Global.introCharTex.Height / 2), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.introToasterFront, new Vector2(640f, 360f), (Rectangle?)null, Color.White, 0f, new Vector2(Global.introToasterFront.Width / 2, Global.introToasterFront.Height / 2), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.introHandleTex, this.handlePosition_, Color.White);
			spriteBatch.DrawString(Global.menuFontTex, this.texts_[this.textIndex_].Substring(0, Math.Min(this.texts_[this.textIndex_].Length, (int)this.textProgress_)), this.textPositions_[this.textIndex_], Color.White);
			break;
		case 1:
			spriteBatch.Draw(Global.introToasterTex, new Vector2(640f, 360f), (Rectangle?)null, Color.White, 0f, new Vector2(Global.introToasterTex.Width / 2, Global.introToasterTex.Height / 2), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.introTrailTex, this.trailRect_, (Rectangle?)null, Color.White, this.trailRotation_, Vector2.Zero, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.introCharTex, this.characterPosition_, (Rectangle?)null, Color.White, this.characterRotation_, new Vector2(Global.introCharTex.Width / 2, Global.introCharTex.Height / 2), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.introToasterFront, new Vector2(640f, 360f), (Rectangle?)null, Color.White, 0f, new Vector2(Global.introToasterFront.Width / 2, Global.introToasterFront.Height / 2), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.introHandleTex, this.handlePosition_, Color.White);
			spriteBatch.DrawString(Global.menuFontTex, this.texts_[this.textIndex_].Substring(0, Math.Min(this.texts_[this.textIndex_].Length, (int)this.textProgress_)), this.textPositions_[this.textIndex_], Color.White);
			break;
		case 2:
			spriteBatch.Draw(Global.introTrailTex, this.trailRect_, (Rectangle?)null, Color.White, this.trailRotation_, Vector2.Zero, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.introCharTex, this.characterPosition_, (Rectangle?)null, Color.White, this.characterRotation_, new Vector2(Global.introCharTex.Width / 2, Global.introCharTex.Height / 2), 1f, SpriteEffects.None, 0f);
			break;
		case 3:
			spriteBatch.Draw(Global.titleBGTex, new Vector2(0f, 85f), (Rectangle?)new Rectangle(0, 0, (int)(this.characterPosition_.X - 73f), 154), Color.White);
			spriteBatch.Draw(Global.titleTex, new Vector2(465f, -10f), (Rectangle?)new Rectangle(0, 0, (int)(this.characterPosition_.X - 465f), 255), Color.White);
			spriteBatch.Draw(Global.introCharTex, this.characterPosition_, (Rectangle?)null, Color.White, this.characterRotation_, new Vector2(Global.introCharTex.Width / 2, Global.introCharTex.Height / 2), 1f, SpriteEffects.None, 0f);
			break;
		}
	}

	public void Reset()
	{
		this.introState_ = 0;
		this.characterPosition_ = new Vector2(640f, 360f);
		this.characterRotation_ = 4.712389f;
		this.handlePosition_ = new Vector2(740f, 404f);
		this.handleVelocity_ = -1000f;
		this.characterVelocity_ = -1000f;
		this.trailRect_ = new Rectangle(561, 260, 0, 154);
		this.trailProgress_ = 0f;
		this.trailRotation_ = 4.712389f;
		this.textProgress_ = 0f;
		this.textProgressRate_ = 10f;
		this.textIndex_ = 0;
	}
}
