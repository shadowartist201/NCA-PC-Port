using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class InstructionsMenu
{
	private MenuItem titleItem_;

	private Collectable[] collectables_ = new Collectable[6];

	private string[] collectableNames_ = new string[6] { "Points", "Score Multiplier", "Health", "Slowmo", "Invincibility", "Party Time" };

	private float aScale_ = 1f;

	private float aScaleRate_ = 1f;

	private float aScaleMin_ = 1f;

	private float aScaleMax_ = 1.2f;

	public InstructionsMenu()
	{
		this.titleItem_ = new MenuItem("Instructions", new Vector2(640f, 70f), Color.White);
		this.titleItem_.DefaultColor = new Color(0, 246, 255);
		for (int i = 0; i < this.collectables_.Length; i++)
		{
			this.collectables_[i] = new Collectable();
			CollectableType type = (CollectableType)((i >= 2) ? (i + 2) : (i + 1));
			this.collectables_[i].Reset(type, new Vector2(680f, 210 + i * 70));
		}
	}

	public void Update(float dt, ref MenuState menuState, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		this.aScale_ += this.aScaleRate_ * dt;
		if (this.aScale_ < this.aScaleMin_)
		{
			this.aScale_ = this.aScaleMin_;
			this.aScaleRate_ = 0f - this.aScaleRate_;
		}
		if (this.aScale_ > this.aScaleMax_)
		{
			this.aScale_ = this.aScaleMax_;
			this.aScaleRate_ = 0f - this.aScaleRate_;
		}
		Collectable[] array = this.collectables_;
		foreach (Collectable collectable in array)
		{
			collectable.Update(dt);
		}
		if ((currKState.IsKeyDown(Keys.Space) && prevKState.IsKeyUp(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A)))
		{
			menuState = MenuState.PLAY;
			Global.PlayMenuSelect();
		}
		if ((currKState.IsKeyDown(Keys.B) && prevKState.IsKeyUp(Keys.B)) || (prevPState.IsButtonUp(Buttons.B) && currPState.IsButtonDown(Buttons.B)))
		{
			menuState = MenuState.SELECTMODE;
			Global.PlayMenuBack();
		}
	}

	public void Draw(SpriteBatch spriteBatch, Background background)
	{
		background.DrawStars(spriteBatch);
		this.titleItem_.Draw(spriteBatch, selected: false);
		spriteBatch.DrawString(Global.menuFontTex, "Controls:", new Vector2(128f, 100f+20), Color.White);
		spriteBatch.Draw(Global.aButtonTex, new Vector2(149f, 210f + 20), (Rectangle?)null, Color.White, 0f, new Vector2(21f, 21f), this.aScale_, SpriteEffects.None, 0f);
		spriteBatch.DrawString(Global.menuFontTex, "Press/Hold", new Vector2(199f, 210f + 20), Color.White, 0f, new Vector2(0f, Global.menuFontTex.MeasureString("Press/Hold").Y / 2f), 0.8f, SpriteEffects.None, 0f);
		spriteBatch.DrawString(Global.menuFontTex, "Collectables:", new Vector2(640f, 100f + 20), Color.White);
		for (int i = 0; i < this.collectables_.Length; i++)
		{
			this.collectables_[i].Draw(spriteBatch);
			spriteBatch.DrawString(Global.menuFontTex, this.collectableNames_[i], this.collectables_[i].Position + new Vector2(100f, 0f), Color.White, 0f, new Vector2(0f, Global.menuFontTex.MeasureString(this.collectableNames_[i]).Y / 2f), 0.8f, SpriteEffects.None, 0f);
		}
		spriteBatch.Draw(Global.aButtonTex, new Vector2(128f, 606f), Color.White);
		spriteBatch.DrawString(Global.menuFontTex, "Continue", new Vector2(190f, 586f+20), Color.White);
		spriteBatch.Draw(Global.bButtonTex, new Vector2(1110f, 606f), Color.White);
		spriteBatch.DrawString(Global.menuFontTex, "Back", new Vector2(980f, 586f+20), Color.White);
	}
}
