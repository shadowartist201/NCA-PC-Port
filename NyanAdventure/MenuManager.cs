using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NCA_Android;

internal class MenuManager
{
	private MenuState menuState_ = MenuState.SPLASH;

	private MenuState prevMenuState_ = MenuState.SPLASH;

	private int characterIndex_;

	private int modeIndex_;

	private SplashMenu splashMenu_;

	private IntroMenu introMenu_;

	private MainMenu mainMenu_;

	private ModeMenu modeMenu_;

	private CharacterMenu charMenu_;

	private InstructionsMenu instructionsMenu_;

	private OptionsMenu optionsMenu_;

	private LeaderboardsMenu leaderboardsMenu_;

	private CreditsMenu creditsMenu_;

	private PauseMenu pauseMenu_;

	private TrialPauseMenu trialPauseMenu_;

	public MenuState MenuState => this.menuState_;

	public MenuState PrevMenuState => this.prevMenuState_;

	public int CharacterIndex => this.characterIndex_;

	public int ModeIndex => this.modeIndex_;

	public MenuManager()
	{
		this.splashMenu_ = new SplashMenu();
		this.introMenu_ = new IntroMenu();
		this.mainMenu_ = new MainMenu();
		this.modeMenu_ = new ModeMenu();
		this.charMenu_ = new CharacterMenu();
		this.instructionsMenu_ = new InstructionsMenu();
		this.optionsMenu_ = new OptionsMenu();
		this.leaderboardsMenu_ = new LeaderboardsMenu();
		this.creditsMenu_ = new CreditsMenu();
		this.pauseMenu_ = new PauseMenu();
		this.trialPauseMenu_ = new TrialPauseMenu();
	}

	public void Update(float dt, MiniGame miniGame, Character character, SongManager songManager, InputState inputState)
	{
		Rectangle touchBounds = new Rectangle(1130, 38, 64, 64);
        this.prevMenuState_ = this.menuState_;
		switch (this.menuState_)
		{
		case MenuState.SPLASH:
			this.splashMenu_.Update(dt, ref this.menuState_);
			break;
		case MenuState.INTRO:
			this.introMenu_.Update(dt, ref this.menuState_, inputState);
			break;
		case MenuState.MAIN:
			this.mainMenu_.Update(dt, ref this.menuState_, inputState);
			break;
		case MenuState.SELECTMODE:
			this.modeMenu_.Update(dt, ref this.menuState_, ref this.modeIndex_, character, inputState);
			break;
		case MenuState.SELECTCHAR:
			this.charMenu_.Update(dt, ref this.menuState_, ref this.characterIndex_, character, inputState);
			break;
		case MenuState.INSTRUCTIONS:
			this.instructionsMenu_.Update(dt, ref this.menuState_, inputState);
			break;
		case MenuState.OPTIONS:
			this.optionsMenu_.Update(dt, ref this.menuState_, inputState);
			break;
		case MenuState.LEADERBOARDS:
			this.leaderboardsMenu_.Update(dt, ref this.menuState_, inputState);
			break;
		case MenuState.CREDITS:
			this.creditsMenu_.Update(dt, ref this.menuState_, inputState);
			break;
		case MenuState.PLAY:
			if (inputState.IsButtonPressed(Buttons.Start) || (touchBounds.Contains((Game1.touchLocations[0].Position - Game1.touchOffset) * Game1.resolutionDifference) && inputState.IsThingTouched()))
			{
				this.menuState_ = MenuState.PAUSE;
				Global.TurnOffBoost();
				Game1.songManager_.Pause();
				Global.PlayMenuBack();
				Global.SetVibrationPaused(paused: true);
			}
			if (Global.IsTrialMode && miniGame.UpdateTrial())
			{
				this.menuState_ = MenuState.TRIALPAUSE;
				Global.TurnOffBoost();
				Game1.songManager_.Pause();
				Global.PlayExplosion();
				this.trialPauseMenu_.ResetStartTimer();
				Global.SetVibrationPaused(paused: true);
			}
			break;
		case MenuState.PAUSE:
			this.pauseMenu_.Update(dt, ref this.menuState_, inputState);
			break;
		case MenuState.TRIALPAUSE:
			//this.trialPauseMenu_.Update(dt, ref this.menuState_, currKState, prevKState, currPState, prevPState, inputState);
			break;
		}
		if (this.menuState_ == MenuState.INTRO && this.prevMenuState_ == MenuState.MAIN)
		{
			this.introMenu_.Reset();
		}
		if (this.menuState_ == MenuState.INTRO && this.prevMenuState_ == MenuState.SPLASH)
		{
			songManager.BeginMenu();
		}
		if (this.menuState_ == MenuState.SELECTMODE && (this.prevMenuState_ == MenuState.SELECTCHAR || this.prevMenuState_ == MenuState.INSTRUCTIONS))
		{
			this.modeMenu_.GainFocus(character);
		}
	}

	public void Draw(SpriteBatch spriteBatch, Background background, Character character, ScoreSystem scoreSystem)
	{
		switch (this.menuState_)
		{
		case MenuState.SPLASH:
			this.splashMenu_.Draw(spriteBatch, background);
			break;
		case MenuState.INTRO:
			this.introMenu_.Draw(spriteBatch, background);
			break;
		case MenuState.MAIN:
			this.mainMenu_.Draw(spriteBatch, background);
			break;
		case MenuState.SELECTMODE:
			this.modeMenu_.Draw(spriteBatch, background, character);
			break;
		case MenuState.SELECTCHAR:
			this.charMenu_.Draw(spriteBatch, background, character);
			break;
		case MenuState.INSTRUCTIONS:
			this.instructionsMenu_.Draw(spriteBatch, background);
			break;
		case MenuState.OPTIONS:
			this.optionsMenu_.Draw(spriteBatch, background);
			break;
		case MenuState.LEADERBOARDS:
			this.leaderboardsMenu_.Draw(spriteBatch, background, scoreSystem);
			break;
		case MenuState.CREDITS:
			this.creditsMenu_.Draw(spriteBatch, background);
			break;
		case MenuState.PLAY:
			break;
		case MenuState.PAUSE:
			this.pauseMenu_.Draw(spriteBatch);
			break;
		case MenuState.TRIALPAUSE:
			this.trialPauseMenu_.Draw(spriteBatch);
			break;
		}
	}
}
