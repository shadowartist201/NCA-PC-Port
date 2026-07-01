using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NCA_Android;

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

	public void Update(float dt, ref MenuState menuState, ref int characterIndex, Character character, InputState inputState)
	{
		Rectangle placeholder = new Rectangle(0, 0, 1280, 720);
		Rectangle placeholder2 = new Rectangle(0, 0, 1, 1);
        if (inputState.IsButtonPressed(Buttons.A) || (placeholder.Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched()))
		{
			characterIndex = this.index_;
			menuState = MenuState.SELECTMODE;
			Global.PlayMenuSelect();
			this.index_ = 0;
			return;
		}
		if (inputState.IsButtonPressed(Buttons.B) || (placeholder2.Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched()))
		{
			menuState = MenuState.MAIN;
			Global.PlayMenuBack();
			this.index_ = 0;
			return;
		}
		if (inputState.IsButtonPressed(Buttons.DPadDown) && this.index_ < this.maxIndex_)
		{
			this.index_++;
			Global.PlayMenuScroll();
		}
		if (inputState.IsButtonPressed(Buttons.DPadUp) && this.index_ > 0)
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
		spriteBatch.DrawString(Global.menuFontTex, "Select", new Vector2(190f, 586f+20), Color.White);
		spriteBatch.Draw(Global.bButtonTex, new Vector2(1110f, 606f), Color.White);
		spriteBatch.DrawString(Global.menuFontTex, "Back", new Vector2(980f, 586f+20), Color.White);
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
