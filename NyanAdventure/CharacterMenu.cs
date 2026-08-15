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

	private Rectangle[] touchBounds = new Rectangle[10]
	{
		new Rectangle(595,152,90,52), //nyan
		new Rectangle(594,218,91,54), //tac
		new Rectangle(594,283,91,50), //afro
        new Rectangle(594,343,91,54), //gameboy
        new Rectangle(594,406,91,55), //jazz
        new Rectangle(594,470,91,55), //taco
        new Rectangle(594,534,91,56), //jetpack
        new Rectangle(594,599,91,56), //sheep
        new Rectangle(116,590,236,74), //select
        new Rectangle(962,588,205,74) //back
    };

	public void Update(float dt, ref MenuState menuState, ref int characterIndex, Character character, InputState inputState)
	{
        if (inputState.IsButtonPressed(Buttons.A))
		{
			characterIndex = this.index_;
			menuState = MenuState.SELECTMODE;
			Global.PlayMenuSelect();
			this.index_ = 0;
			return;
		}
		if (inputState.IsButtonPressed(Buttons.B))
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
        if (touchBounds[0].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched()) 
		{
            characterIndex = 0;
            menuState = MenuState.SELECTMODE;
            Global.PlayMenuSelect();
            this.index_ = 0;
            character.Position = new Vector2(640f, 180f + 64f * (float)0);
            character.SetCharacter(0);
        }
        if (touchBounds[1].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched()) 
		{
            characterIndex = 1;
            menuState = MenuState.SELECTMODE;
            Global.PlayMenuSelect();
            this.index_ = 0;
            character.Position = new Vector2(640f, 180f + 64f * (float)1);
            character.SetCharacter(1);
        }
        if (touchBounds[2].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            characterIndex = 2;
            menuState = MenuState.SELECTMODE;
            Global.PlayMenuSelect();
            this.index_ = 0;
            character.Position = new Vector2(640f, 180f + 64f * (float)2);
            character.SetCharacter(2);
        }
        if (touchBounds[3].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            characterIndex = 3;
            menuState = MenuState.SELECTMODE;
            Global.PlayMenuSelect();
            this.index_ = 0;
            character.Position = new Vector2(640f, 180f + 64f * (float)3);
            character.SetCharacter(3);
        }
        if (touchBounds[4].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            characterIndex = 4;
            menuState = MenuState.SELECTMODE;
            Global.PlayMenuSelect();
            this.index_ = 0;
            character.Position = new Vector2(640f, 180f + 64f * (float)4);
            character.SetCharacter(4);
        }
        if (touchBounds[5].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            characterIndex = 5;
            menuState = MenuState.SELECTMODE;
            Global.PlayMenuSelect();
            this.index_ = 0;
            character.Position = new Vector2(640f, 180f + 64f * (float)5);
            character.SetCharacter(5);
        }
        if (touchBounds[6].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            characterIndex = 6;
            menuState = MenuState.SELECTMODE;
            Global.PlayMenuSelect();
            this.index_ = 0;
            character.Position = new Vector2(640f, 180f + 64f * (float)6);
            character.SetCharacter(6);
        }
        if (touchBounds[7].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            characterIndex = 7;
            menuState = MenuState.SELECTMODE;
            Global.PlayMenuSelect();
            this.index_ = 0;
            character.Position = new Vector2(640f, 180f + 64f * (float)7);
            character.SetCharacter(7);
        }
        if (touchBounds[8].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            //unimplemented
        }
        if (touchBounds[9].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
        {
            menuState = MenuState.MAIN;
            Global.PlayMenuBack();
            this.index_ = 0;
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
