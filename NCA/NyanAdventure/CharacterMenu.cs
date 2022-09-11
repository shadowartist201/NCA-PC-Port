using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class CharacterMenu
{
	private MenuItem titleItem_;

	private int index_;

	private int maxIndex_ = 7;

	public CharacterMenu()
	{
		this.titleItem_ = new MenuItem("Select Character", new Vector2(640f, 90f), Color.White);
		this.titleItem_.DefaultColor = new Color(0, 246, 255);
	}

	public void Update(float dt, ref MenuState menuState, ref int characterIndex, Character character, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		if ((currKState.IsKeyDown(Keys.Space) && prevKState.IsKeyUp(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A)))
		{
			characterIndex = this.index_;
			menuState = MenuState.SELECTMODE;
			Global.PlayMenuSelect();
			this.index_ = 0;
			return;
		}
		if ((currKState.IsKeyDown(Keys.B) && prevKState.IsKeyUp(Keys.B)) || (prevPState.IsButtonUp(Buttons.B) && currPState.IsButtonDown(Buttons.B)))
		{
			menuState = MenuState.MAIN;
			Global.PlayMenuBack();
			this.index_ = 0;
			return;
		}
		if (((currKState.IsKeyDown(Keys.Down) && prevKState.IsKeyUp(Keys.Down)) || (currPState.IsButtonDown(Buttons.DPadDown) && prevPState.IsButtonUp(Buttons.DPadDown)) || (currPState.ThumbSticks.Left.Y < -0.5f && prevPState.ThumbSticks.Left.Y >= -0.5f)) && this.index_ < this.maxIndex_)
		{
			this.index_++;
			Global.PlayMenuScroll();
		}
		if (((currKState.IsKeyDown(Keys.Up) && prevKState.IsKeyUp(Keys.Up)) || (currPState.IsButtonDown(Buttons.DPadUp) && prevPState.IsButtonUp(Buttons.DPadUp)) || (currPState.ThumbSticks.Left.Y > 0.5f && prevPState.ThumbSticks.Left.Y <= 0.5f)) && this.index_ > 0)
		{
			this.index_--;
			Global.PlayMenuScroll();
		}
		if (character.Index != this.index_ || character.Position.X != 640f)
		{
			character.Position = new Vector2(640f, 180f + 64f * (float)this.index_);
			character.SetCharacter(this.index_);
		}
		character.UpdateWithTrail(dt);
	}

	public void Draw(SpriteBatch spriteBatch, Background background, Character character)
	{
		background.DrawStars(spriteBatch);
		this.titleItem_.Draw(spriteBatch, selected: false);
		spriteBatch.Draw(Global.aButtonTex, new Vector2(128f, 606f), Color.White);
		spriteBatch.DrawString(Global.menuFontTex, "Select", new Vector2(177f, 586f), Color.White);
		spriteBatch.Draw(Global.bButtonTex, new Vector2(1110f, 606f), Color.White);
		spriteBatch.DrawString(Global.menuFontTex, "Back", new Vector2(980f, 586f), Color.White);
		character.Draw(spriteBatch);
		for (int i = 0; i < this.index_; i++)
		{
			spriteBatch.Draw(Global.characterListTex, new Vector2(640f, 180f + 64f * (float)i), (Rectangle?)new Rectangle(152 * i, 0, 152, 80), Color.White, 0f, new Vector2(76f, 40f), 0.5f, SpriteEffects.None, 0f);
		}
		for (int i = this.index_ + 1; i <= this.maxIndex_; i++)
		{
			spriteBatch.Draw(Global.characterListTex, new Vector2(640f, 180f + 64f * (float)i), (Rectangle?)new Rectangle(152 * i, 0, 152, 80), Color.White, 0f, new Vector2(76f, 40f), 0.5f, SpriteEffects.None, 0f);
		}
	}
}
