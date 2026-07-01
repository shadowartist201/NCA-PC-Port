using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NCA_Android;

internal class CreditsMenu : Menu
{
	private string[] creditNames_ = new string[27]
	{
		"Creator / Lead Artist - PRguitarman", "Programming / Game Design - Nick Kinkade", "Creative Director - Brian Ferrara", "Art Director - Narek Gevorgian", "Artist / Designer - Otto Chin", "Music / Sound Design - Kevin Salchert", "Marketing - Melissa Fassetta", "", "W21 Team", "Rolo Ledesma",
		"Yikuno \"Kino\" Barnaby", "Edwin Matos", "Special Thanks", "Maurice Freedman", "Ben Lashes", "Sandy Fliderman", "KD", "Momone Momo", "Daniwell", "Hatsune Miku",
		"", "Nyan Cat & Associated characters Copyright 2009-2011", "LOL_comics / PRguitarman", "", "www.nyancatadventure.com", "www.21stgames.com", "www.prguitarman.com"
	};

	private Vector2[] origins_;

	public CreditsMenu()
		: base("Credits")
	{
		this.origins_ = new Vector2[this.creditNames_.Length];
		for (int i = 0; i < this.creditNames_.Length; i++)
		{
			ref Vector2 reference = ref this.origins_[i];
			reference = Global.fontTex.MeasureString(this.creditNames_[i]) / 2f;
		}
		base.AddMenuItem(new MenuItem("Back", new Vector2(640f, 666f), new Color(255, 240, 0)));
	}

	public void Update(float dt, ref MenuState menuState, InputState inputState)
	{
		Rectangle placeholder = new Rectangle(0, 0, 1280, 720);
		Rectangle placeholder2 = new Rectangle(0, 0, 1, 1);
        if (inputState.IsButtonPressed(Buttons.A) || (placeholder.Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched()))
		{
			if (base.index_ == 0)
			{
				menuState = MenuState.MAIN;
			}
			if (base.index_ == 0)
			{
				Global.PlayMenuBack();
			}
			else
			{
				Global.PlayMenuSelect();
			}
			base.index_ = 0;
		}
		if (inputState.IsButtonPressed(Buttons.B) || (placeholder2.Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched()))
		{
			menuState = MenuState.MAIN;
			Global.PlayMenuBack();
			base.index_ = 0;
		}
		base.Update(dt, inputState);
	}

	public new void Draw(SpriteBatch spriteBatch, Background background)
	{
		for (int i = 0; i < this.creditNames_.Length / 2 - 1; i++)
		{
			spriteBatch.DrawString(Global.fontTex, this.creditNames_[i], new Vector2(320f, 320 + i * 20), Color.White, 0f, this.origins_[i], 0.5f, SpriteEffects.None, 0f);
		}
		for (int i = this.creditNames_.Length / 2 - 1; i < this.creditNames_.Length - 3; i++)
		{
			spriteBatch.DrawString(Global.fontTex, this.creditNames_[i], new Vector2(960f, 320 + (i - (this.creditNames_.Length / 2 - 2)) * 20), Color.White, 0f, this.origins_[i], 0.5f, SpriteEffects.None, 0f);
		}
		for (int i = this.creditNames_.Length - 3; i < this.creditNames_.Length; i++)
		{
			spriteBatch.DrawString(Global.fontTex, this.creditNames_[i], new Vector2(640f, 320 + (i - (this.creditNames_.Length / 2 - 2)) * 20), Color.White, 0f, this.origins_[i], 0.5f, SpriteEffects.None, 0f);
		}
		base.Draw(spriteBatch, background);
	}
}
