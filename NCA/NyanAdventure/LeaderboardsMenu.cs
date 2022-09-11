using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class LeaderboardsMenu : Menu
{
	public LeaderboardsMenu()
		: base("Leaderboards")
	{
		base.AddMenuItem(new MenuItem("Back", new Vector2(640f, 666f), new Color(255, 240, 0)));
	}

	public void Update(float dt, ref MenuState menuState, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		if ((currKState.IsKeyDown(Keys.Space) && prevKState.IsKeyUp(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A)))
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
		if ((currKState.IsKeyDown(Keys.B) && prevKState.IsKeyUp(Keys.B)) || (prevPState.IsButtonUp(Buttons.B) && currPState.IsButtonDown(Buttons.B)))
		{
			menuState = MenuState.MAIN;
			Global.PlayMenuBack();
			base.index_ = 0;
		}
		base.Update(dt, currKState, prevKState, currPState, prevPState);
	}

	public void Draw(SpriteBatch spriteBatch, Background background, ScoreSystem scoreSystem)
	{
		base.Draw(spriteBatch, background);
		scoreSystem.DrawLeaderboard(spriteBatch);
	}
}
