using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NCA_Android;

internal class LeaderboardsMenu : Menu
{
	public LeaderboardsMenu()
		: base("Leaderboards")
	{
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

	public void Draw(SpriteBatch spriteBatch, Background background, ScoreSystem scoreSystem)
	{
		base.Draw(spriteBatch, background);
		scoreSystem.DrawLeaderboard(spriteBatch);
	}
}
