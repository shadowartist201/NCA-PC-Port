using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class MainMenu : Menu
{
	private float afkTimer_ = 0f;

	private float afkTime_ = 60f;

	public MainMenu()
		: base("")
	{
		base.AddMenuItem(new MenuItem("Start", new Vector2(640f, 314f), new Color(255, 0, 192)));
		base.AddMenuItem(new MenuItem("Options", new Vector2(640f, 387f), new Color(0, 246, 255)));
		base.AddMenuItem(new MenuItem("Leaderboards", new Vector2(640f, 460f), new Color(120, 255, 0)));
		base.AddMenuItem(new MenuItem("Credits", new Vector2(640f, 533f), new Color(255, 0, 0)));
		base.AddMenuItem(new MenuItem("Quit", new Vector2(640f, 606f), new Color(255, 240, 0)));
	}

	public void Update(float dt, ref MenuState menuState, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		this.afkTimer_ += dt;
		if (this.afkTimer_ > this.afkTime_)
		{
			menuState = MenuState.INTRO;
			base.index_ = 0;
			this.afkTimer_ = 0f;
			Global.PlayExplosion();
			return;
		}
		if (base.menuItems_.Count != 6 && Global.IsTrialMode)
		{
			base.menuItems_.Clear();
			base.AddMenuItem(new MenuItem("Start", new Vector2(640f, 314f), new Color(255, 0, 192)));
			base.AddMenuItem(new MenuItem("Options", new Vector2(640f, 374f), new Color(0, 246, 255)));
			base.AddMenuItem(new MenuItem("Leaderboards", new Vector2(640f, 434f), new Color(120, 255, 0)));
			base.AddMenuItem(new MenuItem("Credits", new Vector2(640f, 494f), new Color(255, 0, 0)));
			base.AddMenuItem(new MenuItem("Buy", new Vector2(640f, 554f), Color.Orange));
			base.AddMenuItem(new MenuItem("Quit", new Vector2(640f, 614f), new Color(255, 240, 0)));
			base.index_ = 0;
		}
		if (base.menuItems_.Count != 5 && !Global.IsTrialMode)
		{
			base.menuItems_.Clear();
			base.AddMenuItem(new MenuItem("Start", new Vector2(640f, 314f), new Color(255, 0, 192)));
			base.AddMenuItem(new MenuItem("Options", new Vector2(640f, 387f), new Color(0, 246, 255)));
			base.AddMenuItem(new MenuItem("Leaderboards", new Vector2(640f, 460f), new Color(120, 255, 0)));
			base.AddMenuItem(new MenuItem("Credits", new Vector2(640f, 533f), new Color(255, 0, 0)));
			base.AddMenuItem(new MenuItem("Quit", new Vector2(640f, 606f), new Color(255, 240, 0)));
			base.index_ = 0;
		}
		if ((currKState.IsKeyDown(Keys.Space) && prevKState.IsKeyUp(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A)))
		{
			switch (base.index_)
			{
			case 0:
				menuState = MenuState.SELECTCHAR;
				base.index_ = 0;
				Global.PlayMenuSelect();
				break;
			case 1:
				menuState = MenuState.OPTIONS;
				base.index_ = 0;
				Global.PlayMenuSelect();
				break;
			case 2:
				menuState = MenuState.LEADERBOARDS;
				base.index_ = 0;
				Global.PlayMenuSelect();
				break;
			case 3:
				menuState = MenuState.CREDITS;
				base.index_ = 0;
				Global.PlayMenuSelect();
				break;
			case 4:
				if (!Global.IsTrialMode)
				{
					menuState = MenuState.QUIT;
					base.index_ = 0;
					Global.PlayMenuSelect();
				}
				else if (Global.CanBuyGame())
				{
					//Guide.ShowMarketplace(Global.PlayerIndex.Value);
					Global.PlayMenuSelect();
				}
				else
				{
					Global.PlayMenuBack();
				}
				break;
			case 5:
				menuState = MenuState.QUIT;
				base.index_ = 0;
				Global.PlayMenuSelect();
				break;
			}
			this.afkTimer_ = 0f;
		}
		int num = base.index_;
		base.Update(dt, currKState, prevKState, currPState, prevPState);
		if (base.index_ != num)
		{
			this.afkTimer_ = 0f;
		}
	}

	public new void Draw(SpriteBatch spriteBatch, Background background)
	{
		base.Draw(spriteBatch, background);
	}
}
