using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class Background
{
	private BackgroundMode lastMode_ = BackgroundMode.SPACE;

	private BackgroundMode mode_ = BackgroundMode.SPACE;

	private Sprite[] staticElements_ = new Sprite[2];

	private float darkness_ = 1f;

	private float darknessRate_ = 0.3f;

	private float darknessMin_ = 0f;

	private float darknessMax_ = 1f;

	private BackgroundScroller backgroundScroller_;

	private float scrollerHeight_ = 916f;

	private float scrollerHeightRate_ = 80f;

	private float scrollerHeightMin_ = 671f;

	private float scrollerHeightMax_ = 916f;

	private BWScroller bwScroller_;

	private List<BackgroundElement> spriteElements_ = new List<BackgroundElement>();

	private List<BackgroundElement> backgroundElements_ = new List<BackgroundElement>();

	private List<BackgroundElement> sharedBackgroundElements_ = new List<BackgroundElement>();

	private float modeTimer_ = 0f;

	private float[] modeTimes_ = new float[3] { 10f, 16f, 5f };

	public Background()
	{
		this.staticElements_[0] = new Sprite(Global.bgLightTex, null, new Vector2(Global.bgLightTex.Width / 2, Global.bgLightTex.Height / 2));
		this.staticElements_[1] = new Sprite(Global.bgDarkTex, null, new Vector2(Global.bgDarkTex.Width / 2, Global.bgDarkTex.Height / 2));
		this.backgroundScroller_ = new BackgroundScroller();
		this.bwScroller_ = new BWScroller();
		this.InitializeStars();
		this.InitializeSpace(first: true);
		this.InitializeBSprites();
	}

	public void Update(float dt)
	{
		if (this.mode_ != this.lastMode_)
		{
			if (this.lastMode_ == BackgroundMode.SPACE)
			{
				if (this.mode_ == BackgroundMode.CITY)
				{
					for (int num = this.backgroundElements_.Count; num > 0; num--)
					{
						if (!this.backgroundElements_[num - 1].Visible)
						{
							this.backgroundElements_.RemoveAt(num - 1);
						}
					}
					if (this.backgroundElements_.Count == 0)
					{
						this.darknessRate_ = -0.3f;
						this.scrollerHeightRate_ = -80f;
						this.InitializeCity();
						this.lastMode_ = this.mode_;
					}
				}
				else
				{
					this.lastMode_ = this.mode_;
				}
			}
			if (this.lastMode_ == BackgroundMode.CITY)
			{
				if (this.mode_ == BackgroundMode.SPACE)
				{
					this.darkness_ += this.darknessRate_ * dt;
					if (this.darkness_ < this.darknessMin_)
					{
						this.darkness_ = this.darknessMin_;
					}
					if (this.darkness_ > this.darknessMax_)
					{
						this.darkness_ = this.darknessMax_;
					}
					this.staticElements_[1].Alpha = this.darkness_;
					this.scrollerHeight_ += this.scrollerHeightRate_ * dt;
					if (this.scrollerHeight_ < this.scrollerHeightMin_)
					{
						this.scrollerHeight_ = this.scrollerHeightMin_;
					}
					if (this.scrollerHeight_ > this.scrollerHeightMax_)
					{
						this.scrollerHeight_ = this.scrollerHeightMax_;
						this.InitializeSpace(first: false);
						this.lastMode_ = this.mode_;
					}
				}
				else
				{
					this.lastMode_ = this.mode_;
				}
			}
			if (this.lastMode_ == BackgroundMode.BW)
			{
				if (this.mode_ == BackgroundMode.SPACE)
				{
					this.darkness_ += this.darknessRate_ * dt;
					if (this.darkness_ < this.darknessMin_)
					{
						this.darkness_ = this.darknessMin_;
					}
					if (this.darkness_ > this.darknessMax_)
					{
						this.darkness_ = this.darknessMax_;
					}
					this.staticElements_[1].Alpha = this.darkness_;
					this.scrollerHeight_ += this.scrollerHeightRate_ * dt;
					if (this.scrollerHeight_ < this.scrollerHeightMin_)
					{
						this.scrollerHeight_ = this.scrollerHeightMin_;
					}
					if (this.scrollerHeight_ > this.scrollerHeightMax_)
					{
						this.scrollerHeight_ = this.scrollerHeightMax_;
						this.InitializeSpace(first: false);
						this.lastMode_ = this.mode_;
					}
				}
				else
				{
					this.lastMode_ = this.mode_;
				}
			}
		}
		else
		{
			this.modeTimer_ += dt;
			if (this.modeTimer_ > this.modeTimes_[(int)this.mode_])
			{
				this.SetMode((BackgroundMode)((int)(this.mode_ + 1) % 2));
			}
			if (this.mode_ == BackgroundMode.SPACE || this.mode_ == BackgroundMode.CITY)
			{
				this.darkness_ += this.darknessRate_ * dt;
				if (this.darkness_ < this.darknessMin_)
				{
					this.darkness_ = this.darknessMin_;
				}
				if (this.darkness_ > this.darknessMax_)
				{
					this.darkness_ = this.darknessMax_;
				}
				this.staticElements_[1].Alpha = this.darkness_;
				this.scrollerHeight_ += this.scrollerHeightRate_ * dt;
				if (this.scrollerHeight_ < this.scrollerHeightMin_)
				{
					this.scrollerHeight_ = this.scrollerHeightMin_;
				}
				if (this.scrollerHeight_ > this.scrollerHeightMax_)
				{
					this.scrollerHeight_ = this.scrollerHeightMax_;
				}
			}
		}
		this.backgroundScroller_.Update(dt, this.scrollerHeight_);
		this.bwScroller_.Update(dt);
		foreach (BackgroundElement item in this.spriteElements_)
		{
			item.Update(dt);
		}
		foreach (BackgroundElement item2 in this.backgroundElements_)
		{
			item2.Update(dt);
		}
	}

	public void UpdateStars(float dt)
	{
		foreach (BackgroundElement item in this.sharedBackgroundElements_)
		{
			item.Update(dt);
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		Sprite[] array = this.staticElements_;
		foreach (Sprite sprite in array)
		{
			sprite.Draw(spriteBatch);
		}
		foreach (BackgroundElement item in this.sharedBackgroundElements_)
		{
			item.Draw(spriteBatch);
		}
		this.backgroundScroller_.Draw(spriteBatch);
		foreach (BackgroundElement item2 in this.backgroundElements_)
		{
			item2.Draw(spriteBatch);
		}
		this.bwScroller_.Draw(spriteBatch);
		foreach (BackgroundElement item3 in this.spriteElements_)
		{
			item3.Draw(spriteBatch);
		}
	}

	public void DrawStars(SpriteBatch spriteBatch)
	{
		foreach (BackgroundElement item in this.sharedBackgroundElements_)
		{
			item.Draw(spriteBatch);
		}
	}

	public void SetMode(BackgroundMode mode)
	{
		this.lastMode_ = this.mode_;
		this.mode_ = mode;
		if (this.lastMode_ == BackgroundMode.BW)
		{
			this.bwScroller_.End();
		}
		switch (mode)
		{
		case BackgroundMode.SPACE:
			this.darknessRate_ = 0.3f;
			this.scrollerHeightRate_ = 80f;
			break;
		case BackgroundMode.BW:
			this.bwScroller_.Begin();
			break;
		}
		this.modeTimer_ = 0f;
	}

	private void InitializeStars()
	{
		for (int i = 0; i < 40; i++)
		{
			this.sharedBackgroundElements_.Add(new BackgroundElement(initialize: true, Global.starTex, new Rectangle(0, 0, 35, 36), 6, 0.1f, 0f, 0f, 0f, Global.ScreenSize.Y, -100f, -100f, 0f));
		}
	}

	private void InitializeSpace(bool first)
	{
		this.backgroundElements_.Add(new BackgroundElement(first, Global.planetTex[0], null, 1, 1f, 0.1f, 0.1f, 720 - Global.planetTex[0].Height, 720f, -200f, -100f, 0f));
		this.backgroundElements_.Add(new BackgroundElement(first, Global.planetTex[1], null, 1, 1f, 0.1f, 0.1f, 0f, Global.planetTex[1].Height, -200f, -100f, 0f));
		for (int i = 0; i < 4; i++)
		{
			this.backgroundElements_.Add(new BackgroundElement(first, Global.asteroidTex[i], null, 1, 1f, 0.1f, 0.1f, 0f, Global.ScreenSize.Y, -300f, -200f, 0f));
		}
		for (int i = 1; i < 4; i += 2)
		{
			this.backgroundElements_.Add(new BackgroundElement(first, Global.asteroidTex[i], null, 1, 1f, 0.1f, 0.1f, 0f, Global.ScreenSize.Y, -300f, -200f, 0f));
		}
	}

	private void InitializeCity()
	{
	}

	private void InitializeBW()
	{
	}

	private void InitializeBSprites()
	{
		int[] array = new int[6] { 5, 6, 6, 6, 6, 6 };
		for (int i = 0; i < 6; i++)
		{
			this.spriteElements_.Add(new BackgroundElement(initialize: true, Global.bSpriteTex[i], new Rectangle(0, 0, Global.bSpriteTex[i].Width / array[i], Global.bSpriteTex[i].Height), array[i], 0.075f, 0f, 0f, 0f, Global.ScreenSize.Y, -400f, -300f, 0f));
		}
		for (int i = 2; i < 4; i++)
		{
			this.spriteElements_.Add(new BackgroundElement(initialize: true, Global.bSpriteTex[i], new Rectangle(0, 0, Global.bSpriteTex[i].Width / array[i], Global.bSpriteTex[i].Height), array[i], 0.075f, 0f, 0f, 0f, Global.ScreenSize.Y, -400f, -300f, 0f));
		}
	}
}
