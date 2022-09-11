using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class Stage
{
	private RenderTarget2D renderTarget_;

	public static Background background_;

	private MiniGame miniGame_;

	public static EffectManager effectManager_;

	public MiniGame MiniGame => this.miniGame_;

	public Character Character => this.miniGame_.Character;

	public Background Background => Stage.background_;

	public ScoreSystem ScoreSystem => this.miniGame_.ScoreSystem;

	public Stage(ContentManager content, GraphicsDevice graphicsDevice)
	{
		Stage.effectManager_ = new EffectManager(content, graphicsDevice);
		Stage.background_ = new Background();
		this.miniGame_ = new MiniGame();
		Stage.background_.SetMode(BackgroundMode.SPACE);
		this.renderTarget_ = new RenderTarget2D(graphicsDevice, 1280, 720, mipMap: false, SurfaceFormat.Color, DepthFormat.None);
	}

	public void Update(float dt, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		if (this.miniGame_.SlowmoTimeFactor < 1f)
		{
			Stage.background_.Update(dt * this.miniGame_.SlowmoTimeFactor);
		}
		else
		{
			Stage.background_.Update(dt);
		}
		this.miniGame_.Update(dt, currKState, prevKState, currPState, prevPState);
		Stage.effectManager_.Update(dt);
	}

	public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice)
	{
		graphicsDevice.SetRenderTarget(this.renderTarget_);
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
		Stage.background_.Draw(spriteBatch);
		this.miniGame_.DrawGame(spriteBatch);
		spriteBatch.End();
		Stage.effectManager_.Draw(spriteBatch, this.renderTarget_, graphics, graphicsDevice);
	}

	public void DrawUI(SpriteBatch spriteBatch)
	{
		this.miniGame_.DrawUI(spriteBatch);
	}

	public void SetMode(OverallMode overallMode)
	{
		this.miniGame_.ResetOverallMode(overallMode);
	}

	public void SetCharacter(int characterIndex)
	{
		this.miniGame_.SetCharacter(characterIndex);
	}
}
