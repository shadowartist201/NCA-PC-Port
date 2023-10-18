using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class Game1 : Game
{
	private GraphicsDeviceManager graphics;

	private SpriteBatch spriteBatch;

	private MenuManager menuManager_;

	private Stage stage_;

	public static SongManager songManager_;

	private GamePadState prevPState_;

	private KeyboardState prevKState_;

	private KeyboardState currKState_;

	private GamePadState currPState_;

	private FPSDisplay fpsDisplay_;

	public Game1()
	{
		Global.DeviceManager = new StorageDeviceManager(this);
		((Collection<IGameComponent>)(object)base.Components).Add((IGameComponent)Global.DeviceManager);
		//GamerServicesComponent item = new GamerServicesComponent(this);
		//((Collection<IGameComponent>)(object)base.Components).Add((IGameComponent)item);
		Global.DeviceManager.DeviceSelectorCanceled += DeviceSelectorCanceled;
		Global.DeviceManager.DeviceDisconnected += DeviceDisconnected;
		Global.DeviceManager.DeviceSelected += DeviceSelected;
		Global.DeviceManager.PromptForDevice();
		//base.add_Exiting((EventHandler<EventArgs>)OnExit);
		this.graphics = new GraphicsDeviceManager(this);
		base.Content.RootDirectory = "Content";
		this.graphics.PreferredBackBufferWidth = 1280;
		this.graphics.PreferredBackBufferHeight = 720;
		this.graphics.SynchronizeWithVerticalRetrace = false;
		base.IsFixedTimeStep = false;
		this.graphics.ApplyChanges();
	}

	private void DeviceDisconnected(object sender, StorageDeviceEventArgs e)
	{
		e.EventResponse = StorageDeviceSelectorEventResponse.Prompt;
	}

	private void DeviceSelectorCanceled(object sender, StorageDeviceEventArgs e)
	{
		e.EventResponse = StorageDeviceSelectorEventResponse.Prompt;
	}

	private void DeviceSelected(object sender, EventArgs e)
	{
		this.stage_.ScoreSystem.LoadInfo();
	}

	private void OnExit(object o, EventArgs e)
	{
		this.stage_.ScoreSystem.SaveInfo();
	}

	protected override void Initialize()
	{
		base.Initialize();
	}

	protected override void LoadContent()
	{
		this.spriteBatch = new SpriteBatch(base.GraphicsDevice);
		Global.introCharTex = base.Content.Load<Texture2D>("Graphics//Intro//character");
		Global.introToasterTex = base.Content.Load<Texture2D>("Graphics//Intro//toaster");
		Global.introHandleTex = base.Content.Load<Texture2D>("Graphics//Intro//toasterhandle");
		Global.introTrailTex = base.Content.Load<Texture2D>("Graphics//Intro//trail");
		Global.introToasterFront = base.Content.Load<Texture2D>("Graphics//Intro//toasterfront");
		for (int i = 0; i < 8; i++)
		{
			Global.characterTex[i] = base.Content.Load<Texture2D>("Graphics//Characters//character" + i);
		}
		Global.characterListTex = base.Content.Load<Texture2D>("Graphics//Characters//characterList");
		Global.characterTrailTex = base.Content.Load<Texture2D>("Graphics//Characters//trails");
		Global.obstacleTex = base.Content.Load<Texture2D>("Graphics//Obstacles//obstacles");
		for (int i = 0; i < 6; i++)
		{
			Global.bSpriteTex[i] = base.Content.Load<Texture2D>("Graphics//BSprites//bsprite" + i);
		}
		Global.bgLightTex = base.Content.Load<Texture2D>("Graphics//Backgrounds//All//light_bg");
		Global.bgDarkTex = base.Content.Load<Texture2D>("Graphics//Backgrounds/All//dark_bg");
		Global.collectableTex = base.Content.Load<Texture2D>("Graphics//Collectables//collectables");
		Global.starTex = base.Content.Load<Texture2D>("Graphics//Backgrounds//All//stars");
		Global.bwBackgroundTex = base.Content.Load<Texture2D>("Graphics//Backgrounds/BW//BWbackground");
		Global.bwTransitionTex = base.Content.Load<Texture2D>("Graphics//Backgrounds/BW//BWtransition");
		for (int i = 0; i < 4; i++)
		{
			Global.asteroidTex[i] = base.Content.Load<Texture2D>("Graphics//Backgrounds//Space//Normal//asteroid" + i);
		}
		for (int i = 0; i < 2; i++)
		{
			Global.planetTex[i] = base.Content.Load<Texture2D>("Graphics//Backgrounds//Space//Normal//planet" + i);
		}
		for (int i = 0; i < 6; i++)
		{
			Global.buildingTex[i] = base.Content.Load<Texture2D>("Graphics//Backgrounds//City//Normal//building" + i);
		}
		Global.grassTex = base.Content.Load<Texture2D>("Graphics//Backgrounds//City//Normal//grass");
		Global.healthTex = base.Content.Load<Texture2D>("Graphics//UI//health");
		Global.timerInsideTex = base.Content.Load<Texture2D>("Graphics//UI//timerInside2");
		Global.timerOutlineTex = base.Content.Load<Texture2D>("Graphics//UI//timerOutline2");
		Global.splashBGTex = base.Content.Load<Texture2D>("Graphics//Menu//splashBG");
		Global.splashFGTex = base.Content.Load<Texture2D>("Graphics//Menu//splashFG");
		Global.offBarTex = base.Content.Load<Texture2D>("Graphics//Menu//offbar");
		Global.onBarTex = base.Content.Load<Texture2D>("Graphics//Menu//onbar");
		Global.sliderBarTex = base.Content.Load<Texture2D>("Graphics//Menu//sliderbg");
		Global.selectBarTex = base.Content.Load<Texture2D>("Graphics//Menu//selectbar");
		Global.titleTex = base.Content.Load<Texture2D>("Graphics//Menu//title");
		Global.titleBGTex = base.Content.Load<Texture2D>("Graphics//Menu//titlebg");
		Global.modeSymbolTex = base.Content.Load<Texture2D>("Graphics//Menu//symbols");
		Global.popupTex = base.Content.Load<Texture2D>("Graphics//Menu//popup");
		Global.aButtonTex = base.Content.Load<Texture2D>("Graphics//Menu//aButton");
		Global.bButtonTex = base.Content.Load<Texture2D>("Graphics//Menu//bButton");
		Global.transitionLineTex = base.Content.Load<Texture2D>("Graphics//Other//transitionLine");
		Global.countdownTex = base.Content.Load<Texture2D>("Graphics//Other//countdown");
		Global.fontTex = base.Content.Load<SpriteFont>("Graphics//Fonts//font");
		Global.menuFontTex = base.Content.Load<SpriteFont>("Graphics//Fonts//fontMenu");
		Global.creditsFontTex = base.Content.Load<SpriteFont>("Graphics//Fonts//fontCredits");
		Global.menuFontTex.LineSpacing = -10;

        for (int i = 0; i < 2; i++)
		{
			Global.explosionSfx[i] = base.Content.Load<SoundEffect>("Sound//SFX//Explosion" + i);
			Global.hurtSfx[i] = base.Content.Load<SoundEffect>("Sound//SFX//Hurt" + i);
			Global.jumpSfx[i] = base.Content.Load<SoundEffect>("Sound//SFX//Jump" + i);
			Global.powerupSfx[i] = base.Content.Load<SoundEffect>("Sound//SFX//Powerup" + i);
			Global.powerdownSfx[i] = base.Content.Load<SoundEffect>("Sound//SFX//Powerdown" + i);
			Global.scoreSfx[i] = base.Content.Load<SoundEffect>("Sound//SFX//Score" + i);
		}
		for (int i = 0; i < 3; i++)
		{
			Global.landSfx[i] = base.Content.Load<SoundEffect>("Sound//SFX//Land" + i);
		}
		Global.boostSfx = base.Content.Load<SoundEffect>("Sound//SFX//Booster Loop");
		Global.transitionSfx = base.Content.Load<SoundEffect>("Sound//SFX//Level Transition");
		Global.countdownSfx = base.Content.Load<SoundEffect>("Sound//SFX//Level Countdown");
		Global.goSfx = base.Content.Load<SoundEffect>("Sound//SFX//Level Start");
		Global.deathSfx = base.Content.Load<SoundEffect>("Sound//SFX//Death");
		Global.menuSelectSfx = base.Content.Load<SoundEffect>("Sound//SFX//Menu//Menu Select");
		Global.menuScrollSfx = base.Content.Load<SoundEffect>("Sound//SFX//Menu//Menu Scroll");
		Global.menuBackSfx = base.Content.Load<SoundEffect>("Sound//SFX//Menu//Menu Back");
		Global.menuStartSfx = base.Content.Load<SoundEffect>("Sound//SFX//Menu//Menu Start");
		Global.menuYeahSfx = base.Content.Load<SoundEffect>("Sound//SFX//Menu//Rolo Yeah");
		Game1.songManager_ = new SongManager(this);
		this.fpsDisplay_ = new FPSDisplay();
		this.menuManager_ = new MenuManager();
		this.stage_ = new Stage(base.Content, base.GraphicsDevice);
	}

	protected override void UnloadContent()
	{
	}

	protected override void Update(GameTime gameTime)
	{
		//Global.IsTrialMode = Guide.IsTrialMode;
		float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
		this.fpsDisplay_.Update(dt);
		this.currKState_ = Keyboard.GetState();
		if (!Global.PlayerIndex.HasValue)
		{
			for (PlayerIndex playerIndex = PlayerIndex.One; playerIndex <= PlayerIndex.Four; playerIndex++)
			{
				GamePadState state = GamePad.GetState(playerIndex);
				if ((this.menuManager_.MenuState == MenuState.INTRO && (state.IsButtonDown(Buttons.A) || state.IsButtonDown(Buttons.Start))) || (this.menuManager_.MenuState == MenuState.MAIN && (state.IsButtonDown(Buttons.A) || state.IsButtonDown(Buttons.DPadDown) || state.ThumbSticks.Left.Y < -0.5f || state.IsButtonDown(Buttons.DPadUp) || state.ThumbSticks.Left.Y > 0.5f)))
				{
					Global.PlayerIndex = playerIndex;
					this.currPState_ = state;
					break;
				}
			}
		}
		else
		{
			this.currPState_ = GamePad.GetState(Global.PlayerIndex.Value);
		}
		Game1.songManager_.Update(dt);
		if (this.menuManager_.MenuState == MenuState.PLAY)
		{
			this.stage_.Update(dt, this.currKState_, this.prevKState_, this.currPState_, this.prevPState_);
		}
		if (this.menuManager_.MenuState != MenuState.PAUSE && this.menuManager_.MenuState != MenuState.TRIALPAUSE)
		{
			this.stage_.Background.UpdateStars(dt);
		}
		this.menuManager_.Update(dt, this.stage_.MiniGame, this.stage_.Character, Game1.songManager_, this.currKState_, this.prevKState_, this.currPState_, this.prevPState_);
		if (this.menuManager_.MenuState != this.menuManager_.PrevMenuState)
		{
			if (this.menuManager_.MenuState == MenuState.PLAY && this.menuManager_.PrevMenuState == MenuState.INSTRUCTIONS)
			{
				Game1.songManager_.SetGame();
				this.stage_.SetCharacter(this.menuManager_.CharacterIndex);
				this.stage_.SetMode((OverallMode)this.menuManager_.ModeIndex);
			}
			if (this.menuManager_.MenuState == MenuState.MAIN && this.menuManager_.PrevMenuState == MenuState.PAUSE)
			{
				Game1.songManager_.SetMenu();
			}
			if (this.menuManager_.MenuState == MenuState.QUIT)
			{
				base.Exit();
			}
		}
		this.prevKState_ = this.currKState_;
		this.prevPState_ = this.currPState_;
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		base.GraphicsDevice.Clear(new Color(0, 40, 62));
		if (this.menuManager_.MenuState == MenuState.PLAY || this.menuManager_.MenuState == MenuState.PAUSE || this.menuManager_.MenuState == MenuState.TRIALPAUSE)
		{
			this.stage_.Draw(this.spriteBatch, this.graphics, base.GraphicsDevice);
		}
		this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
		if (this.menuManager_.MenuState == MenuState.PLAY || this.menuManager_.MenuState == MenuState.PAUSE || this.menuManager_.MenuState == MenuState.TRIALPAUSE)
		{
			this.stage_.DrawUI(this.spriteBatch);
		}
		this.menuManager_.Draw(this.spriteBatch, this.stage_.Background, this.stage_.Character, this.stage_.ScoreSystem);
		this.fpsDisplay_.Draw(spriteBatch);
		this.spriteBatch.End();
		base.Draw(gameTime);
	}
}
