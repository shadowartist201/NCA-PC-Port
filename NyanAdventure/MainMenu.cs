using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NCA_Android;

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

	private Rectangle[] touchBounds = new Rectangle[5]
	{
		new Rectangle(570, 294, 136, 55), //start
		new Rectangle(544, 367, 187, 64), //options
		new Rectangle(470, 441, 337, 58), //leaderboards
		new Rectangle(548, 515, 178, 53), //credits
		new Rectangle(581, 583, 111, 65) //quit
	};

    public void Update(float dt, ref MenuState menuState, InputState inputState)
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
		if (inputState.IsButtonPressed(Buttons.A))
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
		else if (touchBounds[0].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
		{
            menuState = MenuState.SELECTCHAR;
            base.index_ = 0;
            Global.PlayMenuSelect();
        }
        else if (touchBounds[1].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
		{
            menuState = MenuState.OPTIONS;
            base.index_ = 0;
            Global.PlayMenuSelect();
        }
		else if (touchBounds[2].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
		{
            menuState = MenuState.LEADERBOARDS;
            base.index_ = 0;
            Global.PlayMenuSelect();
        }
		else if (touchBounds[3].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
		{
            menuState = MenuState.CREDITS;
            base.index_ = 0;
            Global.PlayMenuSelect();
        }
        else if (touchBounds[4].Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched())
		{
            menuState = MenuState.QUIT;
            base.index_ = 0;
            Global.PlayMenuSelect();
        }
        int num = base.index_;
		base.Update(dt, inputState);
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
