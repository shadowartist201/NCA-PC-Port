using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class MiniGame
{
	private OverallMode overallMode_ = OverallMode.ALL;

	private GameMode gameMode_ = GameMode.PLATFORMER;

	private GameMode nextGameMode_ = GameMode.PLATFORMER;

	private bool paused_ = true;

	private bool catchingUp_ = false;

	private ScoreSystem scoreSystem_;

	private bool userControlledUnpause_ = true;

	private float unpauseTimer_;

	private float unpauseTime_ = 3f;

	private bool displayEndScore_ = false;

	private string endScoreMessage_;

	private bool displayTimedMessage_;

	private string timedMessage_;

	private float messageTimer_;

	private float messageTime_;

	private bool[] collectablePlace_ = new bool[5];

	private float[] collectableTimer_ = new float[5];

	private float[] collectableTime_ = new float[5];

	private float[] collectableTimeMin_ = new float[5] { 24f, 10f, 17f, 3f, 6f };

	private float[] collectableTimeMax_ = new float[5] { 32f, 18f, 25f, 3f, 10f };

	private int difficulty_ = 0;

	private float difficultyTimeFactor_ = 1f;

	private float nextDifficultyTimeFactor_ = 1f;

	private float maxDifficultyTimeFactor_ = 1.51f;

	private float prevDividerPosition_;

	private float prevCharacterPosition_;

	private AnimatedSprite[] dividerSprites_ = new AnimatedSprite[10];

	private float modeTimer_ = 0f;

	private float timeBetweenModes_;

	private UI uInterface_;

	private float[] timeFactors_ = new float[4] { 1.3f, 2.5f, 1.5f, 1f };

	private float slowmoTimeFactor_ = 1f;

	private float slowmoTimer_;

	private float slowmoTime_ = 6f;

	private Character character_ = new Character();

	private List<Obstacle> obstacles_ = new List<Obstacle>();

	private Queue<Obstacle> freeObstacles_ = new Queue<Obstacle>();

	private List<Barrel> barrels_ = new List<Barrel>();

	private Queue<Barrel> freeBarrels_ = new Queue<Barrel>();

	private List<Collectable> collectables_ = new List<Collectable>();

	private Queue<Collectable> freeCollectables_ = new Queue<Collectable>();

	private int lastPlatformIndex_;

	private int lastBarrelIndex_;

	private int blockSizeX_ = 80;

	private int blockSizeY_ = 70;

	private int spikeSize_ = 16;

	public static float tunnelSpeed_ = -380f;

	private int elevation_ = 0;

	private int elevationMin_ = 0;

	private int elevationMax_ = 3;

	private int elevationDirection_ = 1;

	private float platformHeightFrequency_ = 0.125f;

	private float[] platformFrequencies_ = new float[3] { 0.425f, 0.85f, 1f };

	private float[][] platformerDifficultyFrequencies_ = new float[4][]
	{
		new float[1] { 1f },
		new float[3] { 0.333333f, 0.666666f, 1f },
		new float[6] { 0.25f, 0.4f, 0.65f, 0.75f, 0.9f, 1f },
		new float[2] { 0.65f, 1f }
	};

	private int botElevation_ = 0;

	private int topElevation_ = 9;

	private int botElevationMin_ = 0;

	private int topElevationMax_ = 9;

	private int deltaElevationMin_ = 7;

	private int deltaElevationMax_ = 9;

	private float gravityHeightFrequency_ = 0.2f;

	private float[] gravityFrequencies_ = new float[3] { 0.3333f, 0.6666f, 1f };

	private float[][] gravityDifficultyFrequencies_ = new float[4][]
	{
		new float[1] { 1f },
		new float[2] { 0.5f, 1f },
		new float[3] { 0.333f, 0.666f, 1f },
		new float[1] { 1f }
	};

	private float[][] helicopterDifficultyFrequencies_ = new float[3][]
	{
		new float[3] { 0.333f, 0.666f, 1f },
		new float[3] { 0.333f, 0.666f, 1f },
		new float[3] { 0.333f, 0.666f, 1f }
	};

	private float[] barrelDifficultyFrequencies_ = new float[8] { 0.1267f, 0.1267f, 0.05f, 0.1267f, 0.14333f, 0.14333f, 0.14333f, 0.14f };

	private bool[] nextAllowed_ = new bool[8] { true, true, true, true, true, true, true, true };

	private int barrelSize_ = 60;

	private float minRotationRate_ = 3.3f;

	private float maxRotationRate_ = 4.5f;

	private float minBDistance_ = 200f;

	private float maxBDistance_ = 600f;

	private float minMinBarrelDistance_ = 200f;

	private float maxMinBarrelDistance_ = 350f;

	private float minDeltaDistance_ = 120f;

	private float maxDeltaDistance_ = 600f;

	private float minDeltaRate_ = 350f;

	private float maxDeltaRate_ = 550f;

	public Character Character => this.character_;

	public ScoreSystem ScoreSystem => this.scoreSystem_;

	public float SlowmoTimeFactor => this.slowmoTimeFactor_;

	private float dividerPosition_
	{
		get
		{
			return this.dividerSprites_[0].PositionX;
		}
		set
		{
			for (int i = 0; i < this.dividerSprites_.Length; i++)
			{
				this.dividerSprites_[i].PositionX = value;
			}
		}
	}

	private int DeltaElevation => this.topElevation_ - this.botElevation_;

	public MiniGame()
	{
		this.scoreSystem_ = new ScoreSystem();
		this.uInterface_ = new UI();
		for (int i = 0; i < this.dividerSprites_.Length; i++)
		{
			this.dividerSprites_[i] = new AnimatedSprite(Global.transitionLineTex, new Rectangle(0, 0, 4, 720), 45, 0.01f * ((float)(i + 1) / 2f), new Vector2(-100f, 360f));
			this.dividerSprites_[i].ScaleX = 2f;
		}
		this.ResetOverallMode(this.overallMode_);
	}

	public void Update(float dt, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		if (this.paused_ && this.userControlledUnpause_ && ((prevKState.IsKeyUp(Keys.Space) && currKState.IsKeyDown(Keys.Space)) || (prevPState.IsButtonUp(Buttons.A) && currPState.IsButtonDown(Buttons.A))))
		{
			this.paused_ = false;
			Global.PlayGo();
		}
		this.unpauseTimer_ += dt;
		if (this.paused_ && !this.userControlledUnpause_ && this.unpauseTimer_ > this.unpauseTime_)
		{
			this.paused_ = false;
		}
		this.uInterface_.Update(dt);
		AnimatedSprite[] array = this.dividerSprites_;
		foreach (AnimatedSprite animatedSprite in array)
		{
			animatedSprite.Update(dt);
		}
		if (this.slowmoTimeFactor_ < 1f)
		{
			this.character_.Update(dt * this.slowmoTimeFactor_, this.gameMode_, this.paused_);
		}
		else
		{
			this.character_.Update(dt, this.gameMode_, this.paused_);
		}
		if (!this.paused_)
		{
			this.slowmoTimer_ += dt;
			if (this.slowmoTimer_ > this.slowmoTime_)
			{
				Game1.songManager_.SetEndSlowmo();
				this.slowmoTimeFactor_ = 1f;
			}
			this.scoreSystem_.Update(dt);
			if (this.displayTimedMessage_ && this.character_.Position.X > this.dividerPosition_)
			{
				this.messageTimer_ += dt;
				if (this.messageTimer_ > this.messageTime_)
				{
					this.displayTimedMessage_ = false;
				}
			}
			if (this.gameMode_ == this.nextGameMode_)
			{
				this.modeTimer_ += dt;
			}
			if (this.modeTimer_ > this.timeBetweenModes_)
			{
				this.modeTimer_ = 0f;
				if (this.overallMode_ == OverallMode.ALL)
				{
					GameMode gameMode = (GameMode)((int)(this.gameMode_ + 1) % 4);
					if (gameMode == GameMode.PLATFORMER)
					{
						bool flag = false;
						if (this.difficulty_ + 1 < 3)
						{
							this.difficulty_++;
							flag = true;
						}
						else if (this.difficultyTimeFactor_ + 0.25f < this.maxDifficultyTimeFactor_)
						{
							this.nextDifficultyTimeFactor_ = this.difficultyTimeFactor_ + 0.25f;
							flag = true;
						}
						if (flag)
						{
							this.displayTimedMessage_ = true;
							this.timedMessage_ = "Difficulty Increased!";
							this.messageTimer_ = 0f;
							this.messageTime_ = 3f;
						}
					}
					this.AnticipateChangeMode(gameMode);
				}
				else
				{
					GameMode gameMode = this.gameMode_;
					bool flag = false;
					if (this.difficulty_ + 1 < 3)
					{
						this.difficulty_++;
						flag = true;
					}
					else if (this.difficultyTimeFactor_ + 0.25f < this.maxDifficultyTimeFactor_)
					{
						this.nextDifficultyTimeFactor_ = this.difficultyTimeFactor_ + 0.25f;
						flag = true;
					}
					if (flag)
					{
						this.AnticipateChangeMode(gameMode);
						this.displayTimedMessage_ = true;
						this.timedMessage_ = "Difficulty Increased!";
						this.messageTimer_ = 0f;
						this.messageTime_ = 3f;
					}
				}
			}
			for (int j = 0; j < 5; j++)
			{
				if (!this.collectablePlace_[j])
				{
					this.collectableTimer_[j] += dt;
					if (this.collectableTimer_[j] > this.collectableTime_[j])
					{
						this.collectableTime_[j] = Global.RandomBetween(this.collectableTimeMin_[j], this.collectableTimeMax_[j]);
						this.collectableTimer_[j] = 0f;
						this.collectablePlace_[j] = true;
					}
				}
			}
			foreach (Collectable item in this.collectables_)
			{
				if (item.Visible)
				{
					item.Update(dt);
				}
			}
			if (!this.catchingUp_)
			{
				this.character_.UpdateXProgress(dt, this.gameMode_);
			}
			dt *= this.timeFactors_[(int)this.gameMode_] * this.difficultyTimeFactor_ * this.slowmoTimeFactor_;
			if (!this.catchingUp_)
			{
				this.character_.HandleInput(this.gameMode_, dt, currKState, prevKState, currPState, prevPState);
			}
			this.dividerPosition_ += MiniGame.tunnelSpeed_ * dt;
			this.UpdateObstacles(dt);
			this.AdjustCollisionCollectables();
			if (!this.catchingUp_)
			{
				this.character_.UpdateX(dt);
			}
			else
			{
				this.character_.VelocityX = MiniGame.tunnelSpeed_;
				this.character_.UpdateX(dt);
				this.character_.OffsetTrail(MiniGame.tunnelSpeed_ * dt);
				if (this.character_.PositionX < 400f)
				{
					this.catchingUp_ = false;
					this.character_.PositionX = 400f;
				}
			}
			if (!this.character_.Attached)
			{
				this.AdjustCollisionHorizontal();
			}
			if (!this.catchingUp_)
			{
				this.character_.UpdateY(dt);
			}
			if (!this.character_.Attached)
			{
				this.AdjustCollisionVertical(dt);
			}
			if (!this.character_.Attached)
			{
				this.AdjustCollisionBarrels();
			}
			if (this.AdjustCollisionScreen() || this.character_.Health <= 0)
			{
				this.paused_ = true;
				if (this.character_.Health <= 0)
				{
					this.scoreSystem_.End(this.overallMode_, this.character_.Index);
					this.endScoreMessage_ = "Score: " + this.scoreSystem_.Score.ToString() + "\nHigh: " + this.scoreSystem_.GetHighScore(this.overallMode_) + "\nPress     to Begin";
					this.ResetMode(this.gameMode_);
					this.displayEndScore_ = true;
				}
				else
				{
					this.userControlledUnpause_ = false;
					Global.PlayCountdown();
					if (this.character_.Position.X < this.dividerPosition_)
					{
						if (this.overallMode_ == OverallMode.ALL)
						{
							if (this.gameMode_ == GameMode.BARREL && this.difficulty_ > 0 && this.difficultyTimeFactor_ == 1f)
							{
								this.difficulty_--;
							}
						}
						else if (this.difficulty_ > 0 && this.difficultyTimeFactor_ == 1f)
						{
							this.difficulty_--;
						}
					}
					this.nextDifficultyTimeFactor_ = this.difficultyTimeFactor_;
					this.nextGameMode_ = this.gameMode_;
					this.unpauseTimer_ = 0f;
					this.modeTimer_ = 0f;
					this.displayTimedMessage_ = false;
					this.dividerPosition_ = -100f;
					this.ResetObstacles();
					this.uInterface_.Reset();
					this.slowmoTimeFactor_ = 1f;
					Stage.effectManager_.SetEffectOff();
					this.character_.Reset(fullReset: false);
				}
			}
			if (this.character_.Position.X > this.dividerPosition_ && this.prevCharacterPosition_ <= this.prevDividerPosition_ && this.dividerPosition_ > -10f && !this.catchingUp_)
			{
				Global.PlayTransition();
				Game1.songManager_.ContinueGame();
			}
			if ((this.gameMode_ != this.nextGameMode_ || this.difficultyTimeFactor_ != this.nextDifficultyTimeFactor_) && this.character_.Position.X > this.dividerPosition_)
			{
				if (this.gameMode_ != 0 || this.nextGameMode_ != GameMode.BARREL)
				{
				}
				if (this.gameMode_ != this.nextGameMode_)
				{
					if (this.nextGameMode_ != GameMode.BARREL)
					{
						this.ChangeMode();
					}
					else
					{
						if (this.gameMode_ != 0)
						{
							this.gameMode_ = GameMode.PLATFORMER;
							this.character_.ResetGravity();
						}
						if (this.character_.Attached)
						{
							this.ChangeMode();
						}
					}
				}
				if (this.difficultyTimeFactor_ != this.nextDifficultyTimeFactor_ && this.character_.Position.X > this.dividerPosition_)
				{
					this.difficultyTimeFactor_ = this.nextDifficultyTimeFactor_;
				}
			}
		}
		this.prevCharacterPosition_ = this.character_.PositionX;
		this.prevDividerPosition_ = this.dividerPosition_;
	}

	public bool UpdateTrial()
	{
		if (!this.paused_ && ((this.overallMode_ == OverallMode.ALL && this.scoreSystem_.Score > 14000) || (this.overallMode_ != 0 && this.scoreSystem_.Score > 10000)))
		{
			this.paused_ = true;
			if (this.overallMode_ == OverallMode.ALL)
			{
				this.scoreSystem_.End(this.overallMode_, this.character_.Index, 14000f);
			}
			else
			{
				this.scoreSystem_.End(this.overallMode_, this.character_.Index, 10000f);
			}
			this.endScoreMessage_ = "Score: " + this.scoreSystem_.Score.ToString() + "\nHigh: " + this.scoreSystem_.GetHighScore(this.overallMode_) + "\nPress     to Begin";
			this.ResetMode(this.gameMode_);
			this.displayEndScore_ = true;
			return true;
		}
		return false;
	}

	public void DrawGame(SpriteBatch spriteBatch)
	{
		foreach (Obstacle item in this.obstacles_)
		{
			item.Draw(spriteBatch);
		}
		foreach (Barrel item2 in this.barrels_)
		{
			item2.Draw(spriteBatch);
		}
		foreach (Collectable item3 in this.collectables_)
		{
			item3.Draw(spriteBatch);
		}
		this.character_.Draw(spriteBatch);
		AnimatedSprite[] array = this.dividerSprites_;
		foreach (AnimatedSprite animatedSprite in array)
		{
			animatedSprite.Draw(spriteBatch);
		}
	}

	public void DrawUI(SpriteBatch spriteBatch)
	{
		if (this.paused_)
		{
			if (this.userControlledUnpause_)
			{
				if (this.displayEndScore_)
				{
					spriteBatch.Draw(Global.popupTex, new Vector2(640f, 360f), (Rectangle?)null, Color.White, 0f, new Vector2(Global.popupTex.Width / 2, Global.popupTex.Height / 2), 1f, SpriteEffects.None, 0f);
					spriteBatch.Draw(Global.aButtonTex, new Vector2(615f, 406f), (Rectangle?)null, Color.White, 0f, new Vector2(Global.aButtonTex.Width / 2, Global.aButtonTex.Height / 2), 1f, SpriteEffects.None, 0f);
					Global.DrawStringCentered(spriteBatch, this.endScoreMessage_);
				}
				else
				{
					spriteBatch.Draw(Global.popupTex, new Vector2(640f, 360f), (Rectangle?)null, Color.White, 0f, new Vector2(Global.popupTex.Width / 2, Global.popupTex.Height / 2), 1f, SpriteEffects.None, 0f);
					Global.DrawStringCentered(spriteBatch, "Press     to Begin");
					spriteBatch.Draw(Global.aButtonTex, new Vector2(615f, 360f), (Rectangle?)null, Color.White, 0f, new Vector2(Global.aButtonTex.Width / 2, Global.aButtonTex.Height / 2), 1f, SpriteEffects.None, 0f);
				}
			}
			else
			{
				Global.DrawStringCenteredLarge(spriteBatch, ((int)(this.unpauseTime_ - this.unpauseTimer_) + 1).ToString(), (float)Math.Pow((this.unpauseTime_ - this.unpauseTimer_) % 1f, 2.0) * 1.5f);
			}
		}
		else if (this.nextGameMode_ != this.gameMode_ && this.character_.Position.X < this.dividerPosition_)
		{
			switch (this.nextGameMode_)
			{
			case GameMode.PLATFORMER:
				Global.DrawStringCentered(spriteBatch, "Incoming: Jump");
				break;
			case GameMode.GRAVITY:
				Global.DrawStringCentered(spriteBatch, "Incoming: Gravitate");
				break;
			case GameMode.HELICOPTER:
				Global.DrawStringCentered(spriteBatch, "Incoming: Flight");
				break;
			case GameMode.BARREL:
				Global.DrawStringCentered(spriteBatch, "Incoming Toast");
				break;
			}
		}
		else if (this.displayTimedMessage_ && this.character_.Position.X > this.dividerPosition_)
		{
			Global.DrawStringCentered(spriteBatch, this.timedMessage_);
		}
		if (this.unpauseTimer_ < this.unpauseTime_ + 1f)
		{
		}
		this.uInterface_.Draw(spriteBatch, this.character_.Health);
		this.scoreSystem_.Draw(spriteBatch, this.overallMode_, this.userControlledUnpause_ && this.paused_);
	}

	public void SetCharacter(int characterIndex)
	{
		this.character_.SetCharacter(characterIndex);
	}

	public void ResetOverallMode(OverallMode overallMode)
	{
		this.overallMode_ = overallMode;
		switch (this.overallMode_)
		{
		case OverallMode.ALL:
			this.ResetMode(GameMode.PLATFORMER);
			this.timeBetweenModes_ = 7f;
			break;
		case OverallMode.PLATFORMER:
			this.ResetMode(GameMode.PLATFORMER);
			this.timeBetweenModes_ = 14f;
			break;
		case OverallMode.GRAVITY:
			this.ResetMode(GameMode.GRAVITY);
			this.timeBetweenModes_ = 14f;
			break;
		case OverallMode.HELICOPTER:
			this.ResetMode(GameMode.HELICOPTER);
			this.timeBetweenModes_ = 14f;
			break;
		case OverallMode.BARREL:
			this.ResetMode(GameMode.BARREL);
			this.timeBetweenModes_ = 14f;
			break;
		}
	}

	private void ResetMode(GameMode gameMode)
	{
		this.gameMode_ = gameMode;
		this.nextGameMode_ = gameMode;
		this.paused_ = true;
		this.catchingUp_ = false;
		this.scoreSystem_.Reset();
		this.userControlledUnpause_ = true;
		this.displayTimedMessage_ = false;
		this.displayEndScore_ = false;
		for (int i = 0; i < 5; i++)
		{
			this.collectablePlace_[i] = false;
			this.collectableTimer_[i] = 0f;
			this.collectableTime_[i] = this.collectableTimeMin_[i] / 2f;
		}
		this.unpauseTimer_ = this.unpauseTime_ + 2f;
		this.difficulty_ = 0;
		this.difficultyTimeFactor_ = 1f;
		this.slowmoTimeFactor_ = 1f;
		this.nextDifficultyTimeFactor_ = 1f;
		this.modeTimer_ = 0f;
		this.dividerPosition_ = -100f;
		this.prevDividerPosition_ = -100f;
		this.ResetObstacles();
		this.uInterface_.Reset();
		this.slowmoTimer_ = this.slowmoTime_;
		Stage.effectManager_.SetEffectOff();
		Game1.songManager_.SetNormal();
		this.character_.Reset(fullReset: true);
	}

	private void AnticipateChangeMode(GameMode gameMode)
	{
		this.nextGameMode_ = gameMode;
		switch (this.gameMode_)
		{
		case GameMode.PLATFORMER:
			this.AnticipateChangeModePlatformer();
			break;
		case GameMode.GRAVITY:
			this.AnticipateChangeModeGravity();
			break;
		case GameMode.HELICOPTER:
			this.AnticipateChangeModeHelicopter();
			break;
		case GameMode.BARREL:
			this.AnticipateChangeModeBarrel();
			break;
		}
	}

	private void ChangeMode()
	{
		switch (this.nextGameMode_)
		{
		case GameMode.PLATFORMER:
			MiniGame.tunnelSpeed_ = -380f;
			break;
		case GameMode.GRAVITY:
			MiniGame.tunnelSpeed_ = -380f;
			break;
		case GameMode.HELICOPTER:
			MiniGame.tunnelSpeed_ = -380f;
			break;
		case GameMode.BARREL:
			MiniGame.tunnelSpeed_ = -240f;
			break;
		}
		if (this.gameMode_ == GameMode.BARREL && this.character_.PositionX > 400f)
		{
			Global.PlayTransition();
			Game1.songManager_.ContinueGame();
			this.catchingUp_ = true;
		}
		this.character_.ResetGravity();
		this.gameMode_ = this.nextGameMode_;
	}

	private void AdjustCollisionCollectables()
	{
		for (int i = 0; i < this.collectables_.Count; i++)
		{
			if (this.collectables_[i].Visible && this.collectables_[i].Consumable && this.character_.AdjustCollisionCollectable(this.collectables_[i]))
			{
				switch (this.collectables_[i].Type)
				{
				case CollectableType.POINTS:
					this.scoreSystem_.AddToScore(150f, this.collectables_[i].Position);
					Global.PlayPowerup(1);
					break;
				case CollectableType.MULTIPLIER_TWO:
					this.scoreSystem_.SetMultiplier(2);
					Global.PlayPowerup(0);
					break;
				case CollectableType.MULTIPLIER_THREE:
					this.scoreSystem_.SetMultiplier(3);
					Global.PlayPowerup(0);
					break;
				case CollectableType.HEALTH:
					this.character_.AddToHealth(1);
					this.uInterface_.AddHeart(this.character_.Health);
					Global.PlayPowerup(1);
					break;
				case CollectableType.INVINCIBILITY:
					Game1.songManager_.SetSpeedup();
					this.character_.SetInvincible(8f);
					this.uInterface_.SetTimer(8f);
					Global.PlayPowerup(1);
					break;
				case CollectableType.SLOWMO:
					this.slowmoTimeFactor_ = 0.6f;
					this.slowmoTimer_ = 0f;
					Game1.songManager_.SetSlowmo();
					this.uInterface_.SetTimer(6f);
					Global.PlayPowerup(0);
					break;
				case CollectableType.POWERDOWN:
					Stage.effectManager_.SetEffectRandom();
					Global.PlayPowerdown();
					break;
				}
				this.collectables_[i].Consume();
				break;
			}
		}
	}

	private void AdjustCollisionBarrels()
	{
		for (int i = 0; i < this.barrels_.Count; i++)
		{
			if (this.barrels_[i].Visible)
			{
				this.character_.AdjustCollisionBarrels(this.barrels_[i]);
			}
		}
	}

	private void AdjustCollisionHorizontal()
	{
		for (int i = 0; i < this.obstacles_.Count; i++)
		{
			if (this.obstacles_[i].Visible)
			{
				this.character_.AdjustCollisionHorizontal(this.obstacles_[i].CollisionRect, this.obstacles_[i].Deadly);
			}
		}
	}

	private void AdjustCollisionVertical(float dt)
	{
		for (int i = 0; i < this.obstacles_.Count; i++)
		{
			if (this.obstacles_[i].Visible)
			{
				this.character_.AdjustCollisionVertical(this.obstacles_[i].CollisionRect, this.obstacles_[i].Deadly);
			}
		}
	}

	private bool AdjustCollisionScreen()
	{
		return this.character_.AdjustCollisionScreen(this.gameMode_);
	}

	private void UpdateObstacles(float dt)
	{
		foreach (Obstacle item in this.obstacles_)
		{
			if (item.Visible)
			{
				item.Update(dt);
				if (!item.Visible)
				{
					this.freeObstacles_.Enqueue(item);
				}
			}
		}
		foreach (Barrel item2 in this.barrels_)
		{
			if (item2.Visible)
			{
				item2.Update(dt);
				if (!item2.Visible)
				{
					this.freeBarrels_.Enqueue(item2);
				}
			}
		}
		foreach (Collectable item3 in this.collectables_)
		{
			if (item3.Visible)
			{
				item3.UpdateX(dt);
				if (!item3.Visible)
				{
					this.freeCollectables_.Enqueue(item3);
				}
			}
		}
		switch (this.nextGameMode_)
		{
		case GameMode.PLATFORMER:
			this.GenerateNewObstaclesPlatformer();
			break;
		case GameMode.GRAVITY:
			this.GenerateNewObstaclesGravity();
			break;
		case GameMode.HELICOPTER:
			this.GenerateNewObstaclesHelicopter();
			break;
		case GameMode.BARREL:
			this.GenerateNewObstaclesBarrel();
			break;
		}
	}

	private void ResetObstacles()
	{
		this.nextGameMode_ = this.gameMode_;
		this.HideObstacles();
		switch (this.gameMode_)
		{
		case GameMode.PLATFORMER:
			this.ResetObstaclesPlatformer();
			break;
		case GameMode.GRAVITY:
			this.ResetObstaclesGravity();
			break;
		case GameMode.HELICOPTER:
			this.ResetObstaclesHelicopter();
			break;
		case GameMode.BARREL:
			this.ResetObstaclesBarrel();
			break;
		}
	}

	private void HideObstacles()
	{
		foreach (Obstacle item in this.obstacles_)
		{
			if (item.Visible)
			{
				item.Hide();
				this.freeObstacles_.Enqueue(item);
			}
		}
		foreach (Barrel item2 in this.barrels_)
		{
			if (item2.Visible)
			{
				item2.Hide();
				this.freeBarrels_.Enqueue(item2);
			}
		}
		foreach (Collectable item3 in this.collectables_)
		{
			if (item3.Visible)
			{
				item3.Hide();
				this.freeCollectables_.Enqueue(item3);
			}
		}
	}

	private void AddObstacle(Vector2 position, bool deadly, int direction)
	{
		Obstacle obstacle;
		if (this.freeObstacles_.Count > 0)
		{
			obstacle = this.freeObstacles_.Dequeue();
		}
		else
		{
			obstacle = new Obstacle();
			this.obstacles_.Add(obstacle);
		}
		if (!deadly)
		{
			Rectangle sourceRect = new Rectangle(0, 0, this.blockSizeX_, this.blockSizeY_);
			obstacle.Reset(sourceRect, position, deadly);
		}
		else
		{
			obstacle.ResetWithAnim(direction switch
			{
				0 => new Rectangle(250, 300, this.blockSizeX_, this.spikeSize_), 
				1 => new Rectangle(250, 320, this.blockSizeX_, this.spikeSize_), 
				2 => new Rectangle(250, 340, this.spikeSize_, this.blockSizeY_ - 6), 
				3 => new Rectangle(300, 340, this.spikeSize_, this.blockSizeY_ - 6), 
				_ => new Rectangle(0, 0, this.blockSizeX_, this.blockSizeY_), 
			}, 3, 0.05f, position, deadly);
		}
		if (!deadly)
		{
			this.lastPlatformIndex_ = this.obstacles_.IndexOf(obstacle);
		}
	}

	private void AddObstacleD(int dx, int dy)
	{
		this.AddObstacle(new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X + (float)(dx * this.blockSizeX_), Global.ScreenSize.Y - (float)(this.blockSizeY_ / 2) - (float)(this.blockSizeY_ * (this.elevation_ + dy))), deadly: false, 0);
	}

	private void AddObstacleB(int dx, int dy)
	{
		this.AddObstacle(new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X + (float)(dx * this.blockSizeX_), Global.ScreenSize.Y - (float)(this.blockSizeY_ / 2) - (float)(this.blockSizeY_ * dy)), deadly: false, 0);
	}

	private void AddSpikeD(int direction)
	{
		this.AddObstacle(direction switch
		{
			0 => new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X, this.obstacles_[this.lastPlatformIndex_].Position.Y - (float)(this.blockSizeY_ / 2) - (float)(this.spikeSize_ / 2)), 
			1 => new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X, this.obstacles_[this.lastPlatformIndex_].Position.Y + (float)(this.blockSizeY_ / 2) + (float)(this.spikeSize_ / 2)), 
			2 => new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X - (float)(this.blockSizeX_ / 2) - (float)(this.spikeSize_ / 2), this.obstacles_[this.lastPlatformIndex_].Position.Y), 
			3 => new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X + (float)(this.blockSizeX_ / 2) + (float)(this.spikeSize_ / 2), this.obstacles_[this.lastPlatformIndex_].Position.Y), 
			_ => new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X, this.obstacles_[this.lastPlatformIndex_].Position.Y - (float)(this.blockSizeY_ / 2) - (float)(this.spikeSize_ / 2)), 
		}, deadly: true, direction);
	}

	private void AddBarrel(Vector2 referencePosition, MovementMode movementMode, float movementVelocity, float movementPositionMax, RotationMode rotationMode, float rotation, float rotationValue, bool automatic)
	{
		Barrel barrel;
		if (this.freeBarrels_.Count > 0)
		{
			barrel = this.freeBarrels_.Dequeue();
		}
		else
		{
			barrel = new Barrel();
			this.barrels_.Add(barrel);
		}
		barrel.Reset(referencePosition, movementMode, movementVelocity, movementPositionMax, rotationMode, rotation + (float)Math.PI / 2f, rotationValue, automatic);
		this.lastBarrelIndex_ = this.barrels_.IndexOf(barrel);
	}

	private void AddCollectable(CollectableType type, Vector2 position)
	{
		if (type != 0)
		{
			Collectable collectable;
			if (this.freeCollectables_.Count > 0)
			{
				collectable = this.freeCollectables_.Dequeue();
			}
			else
			{
				collectable = new Collectable();
				this.collectables_.Add(collectable);
			}
			collectable.Reset(type, position);
		}
	}

	private void AddCollectableD(CollectableType type, float dx, float dy)
	{
		this.AddCollectable(type, new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X + dx * (float)this.blockSizeX_, Global.ScreenSize.Y - (float)(this.blockSizeY_ / 2) - (float)this.blockSizeY_ * ((float)this.elevation_ + dy)));
	}

	private void AddCollectableB(CollectableType type, float dx, float dy)
	{
		this.AddCollectable(type, new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X + dx * (float)this.blockSizeX_, Global.ScreenSize.Y - (float)(this.blockSizeY_ / 2) - (float)this.blockSizeY_ * dy));
	}

	private CollectableType GetCollectableType()
	{
		for (int i = 0; i < 5; i++)
		{
			if (!this.collectablePlace_[i])
			{
				continue;
			}
			this.collectablePlace_[i] = false;
			switch (i)
			{
			case 0:
				return (CollectableType)Global.Random.Next(2, 4);
			case 1:
				if (this.character_.Health < 6)
				{
					return CollectableType.HEALTH;
				}
				break;
			case 2:
				if (this.difficulty_ > 0)
				{
					return (CollectableType)Global.Random.Next(5, 7);
				}
				return CollectableType.SLOWMO;
			case 3:
				return CollectableType.POINTS;
			case 4:
				return CollectableType.POWERDOWN;
			}
		}
		return CollectableType.NONE;
	}

	private void GenerateNewObstaclesPlatformer()
	{
		if (this.obstacles_.Count == 0)
		{
			this.ResetObstaclesPlatformer();
		}
		if (!(this.obstacles_[this.lastPlatformIndex_].Position.X < Global.ScreenSize.X - (float)(this.blockSizeX_ / 2)))
		{
			return;
		}
		float num = Global.RandomBetween(0f, 1f);
		int num2 = 0;
		if (num < this.platformHeightFrequency_)
		{
			num2 = 0;
		}
		else
		{
			num = Global.RandomBetween(0f, 1f);
			for (int i = 0; i <= this.difficulty_; i++)
			{
				if (num <= this.platformFrequencies_[i] / this.platformFrequencies_[this.difficulty_])
				{
					num2 = i + 1;
					break;
				}
			}
		}
		num = Global.RandomBetween(0f, 1f);
		for (int i = 0; i < this.platformerDifficultyFrequencies_[num2].Length; i++)
		{
			if (num <= this.platformerDifficultyFrequencies_[num2][i])
			{
				CollectableType collectableType = this.GetCollectableType();
				if (num2 == 3 && i == 1)
				{
					this.AddComboPlatformer(i, 6, num2, collectableType);
				}
				else
				{
					this.AddComboPlatformer(i, 1, num2, collectableType);
				}
				break;
			}
		}
	}

	private void AddComboPlatformer(int index, int width, int difficulty, CollectableType collectableType)
	{
		switch (difficulty)
		{
		case 0:
			if (index == 0)
			{
				if (this.elevation_ == this.elevationMin_)
				{
					this.elevationDirection_ = 1;
				}
				if (this.elevation_ == this.elevationMax_)
				{
					this.elevationDirection_ = -1;
				}
				this.AddObstacleD(1, 0);
				this.elevation_ += this.elevationDirection_;
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				if (collectableType == CollectableType.POINTS)
				{
					this.AddCollectableD(collectableType, -1f, 1f);
					this.AddCollectableD(collectableType, 0f, 1f);
					this.AddCollectableD(collectableType, 1f, 1f);
				}
				else
				{
					this.AddCollectableD(collectableType, 0f, 1f);
				}
				this.AddObstacleD(1, 0);
			}
			break;
		case 1:
			switch (index)
			{
			case 0:
			{
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				for (int i = 0; i < width; i++)
				{
					this.AddObstacleD(1, 0);
					if (collectableType == CollectableType.POINTS)
					{
						this.AddCollectableD(collectableType, -1f, 1f);
						this.AddCollectableD(collectableType, 0f, 1f);
						this.AddCollectableD(collectableType, 1f, 1f);
					}
					else
					{
						this.AddCollectableD(collectableType, 0f, 1f);
					}
					this.AddObstacleD(0, 2);
					this.AddObstacleD(0, 3);
				}
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				break;
			}
			case 1:
			{
				this.AddObstacleD(1, 0);
				for (int i = 0; i < width; i++)
				{
					this.AddObstacleD(1, 0);
					this.AddObstacleD(0, 1);
					this.AddCollectableD(collectableType, 0f, 2f);
				}
				break;
			}
			case 2:
			{
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				for (int i = 0; i < width; i++)
				{
					this.AddObstacleD(1, 0);
					this.AddObstacleD(0, 1);
					this.AddObstacleD(0, 2);
					this.AddCollectableD(collectableType, 0f, 3f);
				}
				this.AddObstacleD(1, 0);
				break;
			}
			}
			break;
		case 2:
			switch (index)
			{
			case 0:
			{
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				for (int i = 0; i < width; i++)
				{
					this.AddObstacleD(1, 0);
					if (collectableType == CollectableType.POINTS)
					{
						this.AddCollectableD(collectableType, -1f, 2f);
						this.AddCollectableD(collectableType, 0f, 2.5f);
						this.AddCollectableD(collectableType, 1f, 2f);
					}
					else
					{
						this.AddCollectableD(collectableType, 0f, 2.5f);
					}
					if (i == 0)
					{
						this.AddSpikeD(0);
					}
				}
				this.AddObstacleD(1, 0);
				break;
			}
			case 1:
			{
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				for (int i = 0; i < width; i++)
				{
					this.AddObstacleD(1, 0);
					if (collectableType == CollectableType.POINTS)
					{
						this.AddCollectableD(collectableType, -1f, 1f);
						this.AddCollectableD(collectableType, 0f, 1f);
						this.AddCollectableD(collectableType, 1f, 1f);
					}
					else
					{
						this.AddCollectableD(collectableType, 0f, 1f);
					}
					this.AddObstacleD(0, 2);
					this.AddSpikeD(0);
				}
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				break;
			}
			case 2:
			{
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				for (int i = 0; i < width; i++)
				{
					this.AddObstacleD(1, 0);
					this.AddObstacleD(0, 2);
					this.AddCollectableD(collectableType, 0f, 3f);
					this.AddSpikeD(1);
				}
				this.AddObstacleD(1, 0);
				break;
			}
			case 3:
			{
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				for (int i = 0; i < width; i++)
				{
					this.AddObstacleD(1, 0);
					if (collectableType == CollectableType.POINTS)
					{
						this.AddCollectableD(collectableType, -1f, 1f);
						this.AddCollectableD(collectableType, 0f, 1f);
						this.AddCollectableD(collectableType, 1f, 1f);
					}
					else
					{
						this.AddCollectableD(collectableType, 0f, 1f);
					}
					this.AddObstacleD(0, 3);
					this.AddSpikeD(1);
				}
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				break;
			}
			case 4:
			{
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				for (int i = 0; i < width; i++)
				{
					this.AddObstacleD(1, 0);
					this.AddCollectableD(collectableType, 0f, 3f);
					this.AddObstacleD(0, 1);
					if (i == 0)
					{
						this.AddSpikeD(2);
					}
					this.AddObstacleD(0, 2);
					if (i == 0)
					{
						this.AddSpikeD(2);
					}
				}
				this.AddObstacleD(1, 0);
				break;
			}
			case 5:
			{
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				for (int i = 0; i < width; i++)
				{
					this.AddObstacleD(1, 0);
					if (collectableType == CollectableType.POINTS)
					{
						this.AddCollectableD(collectableType, -1f, 1f);
						this.AddCollectableD(collectableType, 0f, 1f);
						this.AddCollectableD(collectableType, 1f, 1f);
					}
					else
					{
						this.AddCollectableD(collectableType, 0f, 1f);
					}
					this.AddObstacleD(0, 2);
					if (i == 0)
					{
						this.AddSpikeD(2);
					}
					this.AddObstacleD(0, 3);
					if (i == 0)
					{
						this.AddSpikeD(2);
					}
				}
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				this.AddObstacleD(1, 0);
				break;
			}
			}
			break;
		case 3:
			switch (index)
			{
			case 0:
			{
				if (collectableType == CollectableType.POINTS)
				{
					this.AddCollectableD(collectableType, 1f, 2f);
					this.AddCollectableD(collectableType, 2f, 2.5f);
					this.AddCollectableD(collectableType, 3f, 2f);
				}
				else
				{
					this.AddCollectableD(collectableType, 2f, 2f);
				}
				this.AddObstacleD(4, 0);
				for (int i = 1; i < width; i++)
				{
					this.AddObstacleD(1, 0);
				}
				break;
			}
			case 1:
			{
				for (int i = 0; i < width; i++)
				{
					int num = Global.RandomDirection();
					if (this.elevation_ == this.elevationMax_)
					{
						num = -1;
					}
					if (this.elevation_ == this.elevationMin_)
					{
						num = 1;
					}
					if (num == 1)
					{
						if (Global.Random.Next(2) == 0 || this.elevation_ + 2 > this.elevationMax_)
						{
							this.AddObstacleD(4, 1);
							this.elevation_++;
						}
						else
						{
							this.AddObstacleD(3, 2);
							this.elevation_ += 2;
						}
					}
					else if (Global.Random.Next(2) == 0 || this.elevation_ - 2 < this.elevationMin_)
					{
						this.AddObstacleD(3, -1);
						this.elevation_--;
					}
					else
					{
						this.AddObstacleD(3, -2);
						this.elevation_ -= 2;
					}
					if (i == 0)
					{
						this.AddCollectableD(collectableType, 0f, 1f);
					}
					if (collectableType == CollectableType.POINTS)
					{
						this.AddCollectableD(collectableType, 0f, 1f);
					}
				}
				break;
			}
			}
			break;
		}
	}

	private void ResetObstaclesPlatformer()
	{
		this.elevation_ = 0;
		MiniGame.tunnelSpeed_ = -380f;
		for (int i = 0; i < (int)(Global.ScreenSize.X / (float)this.blockSizeX_) + 1; i++)
		{
			this.AddObstacle(new Vector2(i * this.blockSizeX_, Global.ScreenSize.Y - (float)(this.blockSizeY_ / 2) - (float)(this.blockSizeY_ * this.elevation_)), deadly: false, 0);
		}
	}

	private void AnticipateChangeModePlatformer()
	{
		if (this.nextGameMode_ != 0)
		{
			this.AddObstacleD(1, 0);
		}
		this.dividerPosition_ = this.obstacles_[this.lastPlatformIndex_].Position.X + (float)(this.blockSizeX_ / 2);
		if (this.nextGameMode_ == GameMode.GRAVITY || this.nextGameMode_ == GameMode.HELICOPTER)
		{
			if (this.elevation_ == this.elevationMax_)
			{
				this.botElevation_ = 2;
			}
			else
			{
				this.botElevation_ = this.elevation_;
			}
			this.topElevation_ = 9;
			for (int i = 0; i < 10; i++)
			{
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
			}
		}
		if (this.nextGameMode_ == GameMode.BARREL)
		{
			this.AddBarrel(new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X + (float)(this.blockSizeX_ / 2) + (float)this.barrelSize_, this.obstacles_[this.lastPlatformIndex_].Position.Y - (float)(this.blockSizeY_ / 2) - (float)this.barrelSize_), MovementMode.HORIZONTAL, 0f, 0f, RotationMode.NONE, 0f, 0f, automatic: false);
			this.AddComboBarrel(0, 400f, 0, CollectableType.NONE);
		}
	}

	private void GenerateNewObstaclesGravity()
	{
		if (this.obstacles_.Count == 0)
		{
			this.ResetObstaclesGravity();
		}
		if (!(this.obstacles_[this.lastPlatformIndex_].Position.X < Global.ScreenSize.X - (float)(this.blockSizeX_ / 2)))
		{
			return;
		}
		float num = Global.RandomBetween(0f, 1f);
		int num2 = 0;
		if (num < this.gravityHeightFrequency_)
		{
			num2 = 0;
		}
		else
		{
			num = Global.RandomBetween(0f, 1f);
			for (int i = 0; i <= this.difficulty_; i++)
			{
				if (num <= this.gravityFrequencies_[i] / this.gravityFrequencies_[this.difficulty_])
				{
					num2 = i + 1;
					break;
				}
			}
		}
		num = Global.RandomBetween(0f, 1f);
		for (int i = 0; i < this.gravityDifficultyFrequencies_[num2].Length; i++)
		{
			if (num <= this.gravityDifficultyFrequencies_[num2][i])
			{
				CollectableType collectableType = this.GetCollectableType();
				this.AddComboGravity(i, 1, num2, collectableType);
				break;
			}
		}
	}

	private void AddComboGravity(int index, int width, int difficulty, CollectableType collectableType)
	{
		switch (difficulty)
		{
		case 0:
		{
			if (index != 0)
			{
				break;
			}
			if (this.DeltaElevation <= this.deltaElevationMin_)
			{
				this.elevationDirection_ = -1;
			}
			else if (this.DeltaElevation >= this.deltaElevationMax_)
			{
				this.elevationDirection_ = 1;
			}
			else
			{
				this.elevationDirection_ = Global.RandomDirection();
			}
			int num7 = 0;
			int num8 = 0;
			if (this.elevationDirection_ == -1)
			{
				int num9 = Global.Random.Next(0, 3);
				int num10 = this.botElevation_ - this.botElevationMin_;
				int num11 = this.topElevationMax_ - this.topElevation_;
				if ((num9 == 0 || num9 == 2 || num11 < 1) && num10 >= 1)
				{
					num7 = -Global.Random.Next(1, num10 + 1);
				}
				if ((num9 == 1 || num9 == 2 || num10 < 1) && num11 >= 1)
				{
					num8 = Global.Random.Next(1, num11 + 1);
				}
			}
			else
			{
				int num12 = this.DeltaElevation - this.deltaElevationMin_;
				int num13 = 0;
				num13 = ((num12 == 1) ? 1 : Global.Random.Next(1, num12));
				num7 = Global.Random.Next(0, num13 + 1);
				num8 = -(num13 - num7);
			}
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			for (int i = 0; i < Math.Abs(num7); i++)
			{
				this.AddObstacleB(0, this.botElevation_ + Math.Sign(num7) * (i + 1));
			}
			for (int i = 0; i < Math.Abs(num8); i++)
			{
				this.AddObstacleB(0, this.topElevation_ + Math.Sign(num8) * (i + 1));
			}
			this.botElevation_ += num7;
			this.topElevation_ += num8;
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			if (collectableType == CollectableType.POINTS)
			{
				this.AddCollectableB(collectableType, 0f, (float)this.botElevation_ + (float)this.DeltaElevation / 2f - 0.5f);
				this.AddCollectableB(collectableType, 0f, (float)this.botElevation_ + (float)this.DeltaElevation / 2f + 0.5f);
			}
			else
			{
				this.AddCollectableB(collectableType, 0f, (float)this.botElevation_ + (float)this.DeltaElevation / 2f);
			}
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			break;
		}
		case 1:
			switch (index)
			{
			case 0:
			{
				int num4 = ((this.DeltaElevation <= 7) ? 4 : 5);
				int num5 = this.DeltaElevation - num4;
				int num6 = Global.Random.Next(0, this.DeltaElevation - num4);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				if (collectableType == CollectableType.POINTS)
				{
					this.AddCollectableB(collectableType, 0f, (float)(this.botElevation_ + num6) + 0.5f + (float)num4 / 2f - 0.5f);
					this.AddCollectableB(collectableType, 0f, (float)(this.botElevation_ + num6) + 0.5f + (float)num4 / 2f + 0.5f);
				}
				else
				{
					this.AddCollectableB(collectableType, 0f, (float)(this.botElevation_ + num6) + 0.5f + (float)num4 / 2f);
				}
				for (int i = 0; i < num6; i++)
				{
					this.AddObstacleB(0, this.botElevation_ + (i + 1));
				}
				for (int i = num6 + num4; i < this.DeltaElevation - 1; i++)
				{
					this.AddObstacleB(0, this.botElevation_ + (i + 1));
				}
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				break;
			}
			case 1:
			{
				int num3 = this.DeltaElevation - 2;
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddCollectableB(collectableType, 0f, this.botElevation_ + 1);
				this.AddCollectableB(collectableType, 0f, this.topElevation_ - 1);
				for (int i = this.botElevation_ + 2; i < this.topElevation_ - 1; i++)
				{
					this.AddObstacleB(0, i);
				}
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				break;
			}
			}
			break;
		case 2:
			switch (index)
			{
			case 0:
			{
				int num = Global.Random.Next(0, 3);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				int num2 = 0;
				num2 = ((num != 0 && num != 1) ? (2 + (this.DeltaElevation - 6)) : (4 + (this.DeltaElevation - 6)));
				if (collectableType == CollectableType.POINTS)
				{
					this.AddCollectableB(collectableType, 0.5f + (float)num2 / 2f, (float)this.botElevation_ + (float)(this.topElevation_ - this.botElevation_) / 2f + 0.5f);
					this.AddCollectableB(collectableType, 0.5f + (float)num2 / 2f, (float)this.botElevation_ + (float)(this.topElevation_ - this.botElevation_) / 2f - 0.5f);
				}
				else
				{
					this.AddCollectableB(collectableType, 0.5f + (float)num2 / 2f, (float)this.botElevation_ + (float)(this.topElevation_ - this.botElevation_) / 2f);
				}
				for (int i = 0; i < num2; i++)
				{
					this.AddObstacleB(1, this.botElevation_);
					if (num == 0 || num == 2)
					{
						this.AddSpikeD(0);
					}
					this.AddObstacleB(0, this.topElevation_);
					if (num == 1 || num == 2)
					{
						this.AddSpikeD(1);
					}
				}
				if (num == 2)
				{
					this.AddObstacleB(1, this.botElevation_);
					this.AddObstacleB(0, this.topElevation_);
					this.AddObstacleB(1, this.botElevation_);
					this.AddObstacleB(0, this.topElevation_);
					this.AddObstacleB(1, this.botElevation_);
					this.AddObstacleB(0, this.topElevation_);
				}
				break;
			}
			case 1:
			{
				int num4 = ((this.DeltaElevation <= 7) ? 4 : 5);
				int num5 = this.DeltaElevation - num4;
				int num6 = Global.Random.Next(0, this.DeltaElevation - num4);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				if (collectableType == CollectableType.POINTS)
				{
					this.AddCollectableB(collectableType, 0f, (float)(this.botElevation_ + num6) + 0.5f + (float)num4 / 2f - 0.5f);
					this.AddCollectableB(collectableType, 0f, (float)(this.botElevation_ + num6) + 0.5f + (float)num4 / 2f + 0.5f);
				}
				else
				{
					this.AddCollectableB(collectableType, 0f, (float)(this.botElevation_ + num6) + 0.5f + (float)num4 / 2f);
				}
				for (int i = 0; i < num6; i++)
				{
					this.AddObstacleB(0, this.botElevation_ + (i + 1));
					this.AddSpikeD(2);
					if (i == num6 - 1)
					{
						this.AddSpikeD(0);
					}
				}
				for (int i = num6 + num4; i < this.DeltaElevation - 1; i++)
				{
					this.AddObstacleB(0, this.botElevation_ + (i + 1));
					this.AddSpikeD(2);
					if (i == num6 + num4)
					{
						this.AddSpikeD(1);
					}
				}
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				break;
			}
			case 2:
			{
				int num3 = this.DeltaElevation - 2;
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddCollectableB(collectableType, 0f, this.botElevation_ + 1);
				this.AddCollectableB(collectableType, 0f, this.topElevation_ - 1);
				for (int i = this.botElevation_ + 2; i < this.topElevation_ - 1; i++)
				{
					this.AddObstacleB(0, i);
					this.AddSpikeD(2);
				}
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
				break;
			}
			}
			break;
		case 3:
		{
			if (index != 0)
			{
				break;
			}
			int num = Global.Random.Next(0, 3);
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			int num2 = 0;
			num2 = ((num != 0 && num != 1) ? (4 + (this.DeltaElevation - 6)) : (4 + (this.DeltaElevation - 6)));
			if (num == 0 || num == 1)
			{
				if (collectableType == CollectableType.POINTS)
				{
					this.AddCollectableB(collectableType, 0.5f + (float)num2 / 2f, (float)this.botElevation_ + (float)(this.topElevation_ - this.botElevation_) / 2f - 0.5f);
					this.AddCollectableB(collectableType, 0.5f + (float)num2 / 2f, (float)this.botElevation_ + (float)(this.topElevation_ - this.botElevation_) / 2f + 0.5f);
				}
				else
				{
					this.AddCollectableB(collectableType, 0.5f + (float)num2 / 2f, (float)this.botElevation_ + (float)(this.topElevation_ - this.botElevation_) / 2f);
				}
				for (int i = 0; i < num2; i++)
				{
					if (num == 0)
					{
						this.AddObstacleB(1, this.botElevation_);
					}
					else
					{
						this.AddObstacleB(1, this.topElevation_);
					}
				}
				break;
			}
			if (collectableType == CollectableType.POINTS)
			{
				this.AddCollectableB(collectableType, (float)num2 / 2f, (float)this.botElevation_ + (float)(this.topElevation_ - this.botElevation_) / 2f - 0.5f);
				this.AddCollectableB(collectableType, (float)num2 / 2f, (float)this.botElevation_ + (float)(this.topElevation_ - this.botElevation_) / 2f + 0.5f);
			}
			else
			{
				this.AddCollectableB(collectableType, (float)num2 / 2f, (float)this.botElevation_ + (float)(this.topElevation_ - this.botElevation_) / 2f);
			}
			this.AddObstacleB(num2, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			this.AddObstacleB(1, this.botElevation_);
			this.AddObstacleB(0, this.topElevation_);
			break;
		}
		}
	}

	private void ResetObstaclesGravity()
	{
		MiniGame.tunnelSpeed_ = -380f;
		this.botElevation_ = this.botElevationMin_;
		this.topElevation_ = this.topElevationMax_;
		for (int i = 0; i < (int)(Global.ScreenSize.X / (float)this.blockSizeX_) + 1; i++)
		{
			this.AddObstacle(new Vector2(i * this.blockSizeX_, Global.ScreenSize.Y - (float)(this.blockSizeY_ / 2) - (float)(this.blockSizeY_ * this.botElevation_)), deadly: false, 0);
			this.AddObstacle(new Vector2(i * this.blockSizeX_, Global.ScreenSize.Y - (float)(this.blockSizeY_ / 2) - (float)(this.blockSizeY_ * this.topElevation_)), deadly: false, 0);
		}
	}

	private void AnticipateChangeModeGravity()
	{
		if (this.nextGameMode_ != GameMode.GRAVITY)
		{
			for (int i = 0; i < 3; i++)
			{
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
			}
		}
		this.dividerPosition_ = this.obstacles_[this.lastPlatformIndex_].Position.X + (float)(this.blockSizeX_ / 2);
		if (this.nextGameMode_ == GameMode.PLATFORMER)
		{
			this.elevation_ = this.botElevation_;
			for (int i = 0; i < 10; i++)
			{
				this.AddObstacleD(1, 0);
			}
		}
		if (this.nextGameMode_ == GameMode.HELICOPTER)
		{
			for (int i = 0; i < 10; i++)
			{
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
			}
		}
		if (this.nextGameMode_ == GameMode.BARREL)
		{
			for (int i = 0; i < 10; i++)
			{
				this.AddObstacleB(1, this.botElevation_);
			}
			this.AddBarrel(new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X + (float)(this.blockSizeX_ / 2) + (float)this.barrelSize_, this.obstacles_[this.lastPlatformIndex_].Position.Y - (float)(this.blockSizeY_ / 2) - (float)this.barrelSize_), MovementMode.HORIZONTAL, 0f, 0f, RotationMode.NONE, 0f, 0f, automatic: false);
			this.AddComboBarrel(0, 400f, 0, CollectableType.NONE);
		}
	}

	private void GenerateNewObstaclesHelicopter()
	{
		if (this.obstacles_.Count == 0)
		{
			this.ResetObstaclesHelicopter();
		}
		if (!(this.obstacles_[this.lastPlatformIndex_].Position.X < Global.ScreenSize.X - (float)(this.blockSizeX_ / 2)))
		{
			return;
		}
		float num = Global.RandomBetween(0f, 1f);
		for (int i = 0; i < this.helicopterDifficultyFrequencies_[this.difficulty_].Length; i++)
		{
			if (num <= this.helicopterDifficultyFrequencies_[this.difficulty_][i])
			{
				CollectableType collectableType = this.GetCollectableType();
				this.AddComboHelicopter(i, 1, this.difficulty_, collectableType);
				break;
			}
		}
	}

	private void AddComboHelicopter(int index, int width, int difficulty, CollectableType collectableType)
	{
		switch (index)
		{
		case 0:
		{
			if (this.DeltaElevation <= this.deltaElevationMin_)
			{
				this.elevationDirection_ = -1;
			}
			else if (this.DeltaElevation >= this.deltaElevationMax_)
			{
				this.elevationDirection_ = 1;
			}
			else
			{
				this.elevationDirection_ = Global.RandomDirection();
			}
			int num6 = 0;
			int num7 = 0;
			if (this.elevationDirection_ == -1)
			{
				int num8 = Global.Random.Next(0, 3);
				int num9 = this.botElevation_ - this.botElevationMin_;
				int num10 = this.topElevationMax_ - this.topElevation_;
				if ((num8 == 0 || num8 == 2 || num10 < 1) && num9 >= 1)
				{
					num6 = -Global.Random.Next(1, num9 + 1);
				}
				if ((num8 == 1 || num8 == 2 || num9 < 1) && num10 >= 1)
				{
					num7 = Global.Random.Next(1, num10 + 1);
				}
			}
			else
			{
				int num11 = this.DeltaElevation - this.deltaElevationMin_;
				int num12 = 0;
				num12 = ((num11 == 1) ? 1 : Global.Random.Next(1, num11));
				num6 = Global.Random.Next(0, num12 + 1);
				num7 = -(num12 - num6);
			}
			for (int i = 0; i < 4; i++)
			{
				this.AddObstacleB(1, this.botElevation_);
				if (difficulty == 2)
				{
					this.AddSpikeD(0);
				}
				this.AddObstacleB(0, this.topElevation_);
				if (difficulty == 2)
				{
					this.AddSpikeD(1);
				}
				if (i == 1)
				{
					if (collectableType == CollectableType.POINTS)
					{
						this.AddCollectableB(collectableType, 0.5f, (float)this.botElevation_ + (float)this.DeltaElevation / 2f - 0.5f);
						this.AddCollectableB(collectableType, 0.5f, (float)this.botElevation_ + (float)this.DeltaElevation / 2f + 0.5f);
					}
					else
					{
						this.AddCollectableB(collectableType, 0.5f, (float)this.botElevation_ + (float)this.DeltaElevation / 2f);
					}
				}
			}
			this.AddObstacleB(1, this.botElevation_);
			if (difficulty == 2)
			{
				if (num6 <= 0)
				{
					this.AddSpikeD(0);
				}
				if (num6 < 0)
				{
					this.AddSpikeD(3);
				}
			}
			for (int i = 0; i < Math.Abs(num6); i++)
			{
				this.AddObstacleB(0, this.botElevation_ + Math.Sign(num6) * (i + 1));
				if (difficulty == 2)
				{
					if (num6 > 0)
					{
						this.AddSpikeD(2);
					}
					else
					{
						this.AddSpikeD(3);
					}
				}
			}
			if (difficulty == 2 && num6 > 0)
			{
				this.AddSpikeD(0);
			}
			this.AddObstacleB(0, this.topElevation_);
			if (difficulty == 2)
			{
				if (num7 >= 0)
				{
					this.AddSpikeD(1);
				}
				if (num7 > 0)
				{
					this.AddSpikeD(3);
				}
			}
			for (int i = 0; i < Math.Abs(num7); i++)
			{
				this.AddObstacleB(0, this.topElevation_ + Math.Sign(num7) * (i + 1));
				if (difficulty == 2)
				{
					if (num7 > 0)
					{
						this.AddSpikeD(3);
					}
					else
					{
						this.AddSpikeD(2);
					}
				}
			}
			if (difficulty == 2 && num7 < 0)
			{
				this.AddSpikeD(1);
			}
			this.botElevation_ += num6;
			this.topElevation_ += num7;
			break;
		}
		case 1:
		{
			int num3 = ((this.DeltaElevation <= 7) ? 3 : 4);
			int num4 = this.DeltaElevation - num3;
			int num5 = Global.Random.Next(0, this.DeltaElevation - num3);
			for (int i = 0; i < 5; i++)
			{
				this.AddObstacleB(1, this.botElevation_);
				if (difficulty == 2)
				{
					this.AddSpikeD(0);
				}
				this.AddObstacleB(0, this.topElevation_);
				if (difficulty == 2)
				{
					this.AddSpikeD(1);
				}
			}
			this.AddObstacleB(1, this.botElevation_);
			if (collectableType == CollectableType.POINTS)
			{
				this.AddCollectableB(collectableType, 0f, (float)(this.botElevation_ + num5) + 0.5f + (float)num3 / 2f - 0.5f);
				this.AddCollectableB(collectableType, 0f, (float)(this.botElevation_ + num5) + 0.5f + (float)num3 / 2f + 0.5f);
			}
			else
			{
				this.AddCollectableB(collectableType, 0f, (float)(this.botElevation_ + num5) + 0.5f + (float)num3 / 2f);
			}
			for (int i = 0; i < num5; i++)
			{
				this.AddObstacleB(0, this.botElevation_ + (i + 1));
				if (difficulty == 1 || difficulty == 2)
				{
					this.AddSpikeD(2);
					this.AddSpikeD(3);
				}
			}
			if ((difficulty == 1 && num5 > 0) || difficulty == 2)
			{
				this.AddSpikeD(0);
			}
			this.AddObstacleB(0, this.topElevation_);
			if (difficulty == 2 && num5 + num3 >= this.DeltaElevation - 1)
			{
				this.AddSpikeD(1);
			}
			for (int i = num5 + num3; i < this.DeltaElevation - 1; i++)
			{
				this.AddObstacleB(0, this.botElevation_ + (i + 1));
				if (difficulty == 1 || difficulty == 2)
				{
					this.AddSpikeD(2);
					this.AddSpikeD(3);
					if (i == num5 + num3)
					{
						this.AddSpikeD(1);
					}
				}
			}
			break;
		}
		case 2:
		{
			for (int i = 0; i < 7; i++)
			{
				this.AddObstacleB(1, this.botElevation_);
				if (difficulty == 2)
				{
					this.AddSpikeD(0);
				}
				this.AddObstacleB(0, this.topElevation_);
				if (difficulty == 2)
				{
					this.AddSpikeD(1);
				}
			}
			int num;
			int num2;
			if (this.DeltaElevation > 7)
			{
				num = this.botElevation_ + 1 + 3;
				num2 = this.topElevation_ - 3;
			}
			else
			{
				num = ((Global.Random.Next(0, 2) != 0) ? (this.topElevation_ - 1 - 3) : (this.botElevation_ + 1 + 3));
				num2 = num + 1;
			}
			if (collectableType == CollectableType.POINTS)
			{
				this.AddCollectableB(collectableType, 0f, (float)this.botElevation_ + (float)(num - this.botElevation_) / 2f - 0.5f);
				this.AddCollectableB(collectableType, 0f, (float)this.botElevation_ + (float)(num - this.botElevation_) / 2f + 0.5f);
				this.AddCollectableB(collectableType, 0f, -0.5f + (float)this.topElevation_ - (float)(this.topElevation_ - num2) / 2f - 0.5f);
				this.AddCollectableB(collectableType, 0f, -0.5f + (float)this.topElevation_ - (float)(this.topElevation_ - num2) / 2f + 0.5f);
			}
			else
			{
				this.AddCollectableB(collectableType, 0f, (float)this.botElevation_ + (float)(num - this.botElevation_) / 2f);
				this.AddCollectableB(collectableType, 0f, -0.5f + (float)this.topElevation_ - (float)(this.topElevation_ - num2) / 2f);
			}
			for (int i = num; i < num2; i++)
			{
				this.AddObstacleB(0, i);
				if (difficulty == 1 || difficulty == 2)
				{
					this.AddSpikeD(2);
					this.AddSpikeD(3);
					if (i == num)
					{
						this.AddSpikeD(1);
					}
					if (i == num2 - 1)
					{
						this.AddSpikeD(0);
					}
				}
			}
			break;
		}
		}
	}

	private void ResetObstaclesHelicopter()
	{
		MiniGame.tunnelSpeed_ = -380f;
		this.botElevation_ = this.botElevationMin_;
		this.topElevation_ = this.topElevationMax_;
		for (int i = 0; i < (int)(Global.ScreenSize.X / (float)this.blockSizeX_) + 1; i++)
		{
			this.AddObstacle(new Vector2(i * this.blockSizeX_, Global.ScreenSize.Y - (float)(this.blockSizeY_ / 2) - (float)(this.blockSizeY_ * this.botElevation_)), deadly: false, 0);
			this.AddObstacle(new Vector2(i * this.blockSizeX_, Global.ScreenSize.Y - (float)(this.blockSizeY_ / 2) - (float)(this.blockSizeY_ * this.topElevation_)), deadly: false, 0);
		}
	}

	private void AnticipateChangeModeHelicopter()
	{
		if (this.nextGameMode_ != GameMode.HELICOPTER)
		{
			for (int i = 0; i < 3; i++)
			{
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
			}
		}
		this.dividerPosition_ = this.obstacles_[this.lastPlatformIndex_].Position.X + (float)(this.blockSizeX_ / 2);
		if (this.nextGameMode_ == GameMode.PLATFORMER)
		{
			this.elevation_ = this.botElevation_;
			for (int i = 0; i < 10; i++)
			{
				this.AddObstacleD(1, 0);
			}
		}
		if (this.nextGameMode_ == GameMode.GRAVITY)
		{
			for (int i = 0; i < 10; i++)
			{
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
			}
		}
		if (this.nextGameMode_ == GameMode.BARREL)
		{
			for (int i = 0; i < 10; i++)
			{
				this.AddObstacleB(1, this.botElevation_);
			}
			this.AddBarrel(new Vector2(this.obstacles_[this.lastPlatformIndex_].Position.X + (float)(this.blockSizeX_ / 2) + (float)this.barrelSize_, this.obstacles_[this.lastPlatformIndex_].Position.Y - (float)(this.blockSizeY_ / 2) - (float)this.barrelSize_), MovementMode.HORIZONTAL, 0f, 0f, RotationMode.NONE, 0f, 0f, automatic: false);
			this.AddComboBarrel(0, 400f, 0, CollectableType.NONE);
		}
	}

	private void GenerateNewObstaclesBarrel()
	{
		if (this.obstacles_.Count == 0)
		{
			this.ResetObstaclesBarrel();
		}
		if (!(this.barrels_[this.lastBarrelIndex_].PositionRangeX.Y < Global.ScreenSize.X - (float)this.barrelSize_))
		{
			return;
		}
		switch (this.difficulty_)
		{
		case 0:
		{
			this.nextAllowed_[1] = false;
			for (int i = 4; i < 8; i++)
			{
				this.nextAllowed_[i] = false;
			}
			break;
		}
		case 1:
		{
			for (int i = 7; i < 8; i++)
			{
				this.nextAllowed_[i] = false;
			}
			break;
		}
		}
		float num = Global.RandomBetween(0f, 1f);
		float num2 = 0f;
		for (int i = 0; i < 8; i++)
		{
			if (this.nextAllowed_[i])
			{
				num2 += this.barrelDifficultyFrequencies_[i];
			}
		}
		float num3 = 0f;
		for (int i = 0; i < 8; i++)
		{
			if (!this.nextAllowed_[i])
			{
				continue;
			}
			num3 += this.barrelDifficultyFrequencies_[i];
			if (num <= num3 / num2)
			{
				float width = Global.RandomBetween(this.minBDistance_, this.maxBDistance_);
				CollectableType collectableType = this.GetCollectableType();
				switch (i)
				{
				case 0:
					this.AddComboBarrel(0, width, 0, collectableType);
					break;
				case 1:
					this.AddComboBarrel(1, width, 0, collectableType);
					break;
				case 2:
					this.AddComboBarrel(2, width, 0, collectableType);
					break;
				case 3:
					this.AddComboBarrel(3, width, 0, collectableType);
					break;
				case 4:
					this.AddComboBarrel(0, width, 1, collectableType);
					break;
				case 5:
					this.AddComboBarrel(1, width, 1, collectableType);
					break;
				case 6:
					this.AddComboBarrel(2, width, 1, collectableType);
					break;
				case 7:
					this.AddComboBarrel(0, width, 2, collectableType);
					break;
				}
				for (int j = 0; j < 8; j++)
				{
					this.nextAllowed_[j] = true;
				}
				switch (i)
				{
				case 0:
					this.nextAllowed_[0] = false;
					break;
				case 1:
					this.nextAllowed_[4] = false;
					this.nextAllowed_[3] = false;
					this.nextAllowed_[5] = false;
					this.nextAllowed_[6] = false;
					this.nextAllowed_[7] = false;
					break;
				case 2:
					break;
				case 3:
					this.nextAllowed_[3] = false;
					this.nextAllowed_[4] = false;
					this.nextAllowed_[5] = false;
					this.nextAllowed_[6] = false;
					this.nextAllowed_[7] = false;
					break;
				case 4:
					this.nextAllowed_[4] = false;
					this.nextAllowed_[5] = false;
					this.nextAllowed_[7] = false;
					break;
				case 5:
					break;
				case 6:
					this.nextAllowed_[3] = false;
					this.nextAllowed_[4] = false;
					this.nextAllowed_[5] = false;
					this.nextAllowed_[6] = false;
					this.nextAllowed_[7] = false;
					break;
				case 7:
					this.nextAllowed_[2] = false;
					this.nextAllowed_[3] = false;
					this.nextAllowed_[4] = false;
					this.nextAllowed_[5] = false;
					this.nextAllowed_[6] = false;
					this.nextAllowed_[7] = false;
					break;
				}
				break;
			}
		}
	}

	private void AddComboBarrel(int index, float width, int difficulty, CollectableType collectableType)
	{
		float num = Global.RandomBetween(this.minDeltaDistance_, this.maxDeltaDistance_);
		float num2 = Global.RandomBetween(this.minMinBarrelDistance_, this.maxMinBarrelDistance_);
		float rotationValue = Global.RandomBetween(this.minRotationRate_, this.maxRotationRate_);
		float movementVelocity = Global.RandomBetween(this.minDeltaRate_, this.maxDeltaRate_);
		bool flag = (index < 2 && difficulty == 0) || (index < 1 && difficulty == 1) || (index < 1 && difficulty == 2);
		MovementMode movementMode = MovementMode.HORIZONTAL;
		if ((index > 2 && difficulty == 0) || (index > 1 && difficulty == 1) || (index > 0 && difficulty == 2))
		{
			movementMode = MovementMode.VERTICAL;
		}
		Vector2 vector = this.barrels_[this.lastBarrelIndex_].Position;
		if (this.barrels_[this.lastBarrelIndex_].MovementMode == MovementMode.VERTICAL)
		{
			if (this.barrels_[this.lastBarrelIndex_].RotationMode == RotationMode.NONE)
			{
				vector.Y = 0f;
			}
			else
			{
				vector.Y = 1f;
			}
		}
		float x = this.barrels_[this.lastBarrelIndex_].PositionRangeX.X;
		float y = this.barrels_[this.lastBarrelIndex_].PositionRangeX.Y;
		float x2 = this.barrels_[this.lastBarrelIndex_].PositionRangeY.X;
		float y2 = this.barrels_[this.lastBarrelIndex_].PositionRangeY.Y;
		float num3 = y + width;
		float num4 = this.barrelSize_;
		float num5 = 720 - this.barrelSize_;
		if (this.barrels_[this.lastBarrelIndex_].RotationMode == RotationMode.NONE)
		{
			float num6 = this.barrels_[this.lastBarrelIndex_].Rotation - (float)Math.PI / 2f;
			float num7 = num3 - x;
			float num8 = num3 - y;
			float val = x2 + num7 * (float)Math.Tan(num6);
			float val2 = x2 + num8 * (float)Math.Tan(num6);
			float val3 = y2 + num7 * (float)Math.Tan(num6);
			float val4 = y2 + num8 * (float)Math.Tan(num6);
			float num9 = Math.Min(val, Math.Min(val2, Math.Min(val3, val4)));
			float num10 = Math.Max(val, Math.Max(val2, Math.Max(val3, val4)));
			if (flag)
			{
				num4 = num9;
				num5 = num10;
			}
			else if (movementMode == MovementMode.HORIZONTAL)
			{
				float num11 = num;
				float val5 = x2 + (num7 + num11) * (float)Math.Tan(num6);
				float val6 = x2 + (num8 + num11) * (float)Math.Tan(num6);
				float val7 = y2 + (num7 + num11) * (float)Math.Tan(num6);
				float val8 = y2 + (num8 + num11) * (float)Math.Tan(num6);
				float val9 = Math.Min(val5, Math.Min(val6, Math.Min(val7, val8)));
				float val10 = Math.Max(val5, Math.Max(val6, Math.Max(val7, val8)));
				num4 = Math.Min(num9, val9);
				num5 = Math.Max(num10, val10);
			}
			else
			{
				num4 = num9;
				num5 = num10;
			}
		}
		if (num4 < (float)this.barrelSize_)
		{
			num4 = this.barrelSize_;
		}
		if (num5 > (float)(720 - this.barrelSize_))
		{
			num5 = 720 - this.barrelSize_;
		}
		if (movementMode == MovementMode.VERTICAL)
		{
			if (num4 > 720f - num - (float)this.barrelSize_)
			{
				num4 = 720f - num - (float)this.barrelSize_;
			}
			if (num5 > 720f - num - (float)this.barrelSize_)
			{
				num5 = 720f - num - (float)this.barrelSize_;
			}
		}
		Vector2 vector2 = new Vector2(num3, Global.RandomBetween(num4, num5));
		if (vector.Y == 0f)
		{
			vector.Y = vector2.Y;
		}
		else if (vector.Y == 1f)
		{
			vector = vector2;
		}
		switch (difficulty)
		{
		case 0:
			switch (index)
			{
			case 0:
			{
				float rotation = 0f;
				this.AddCollectable(collectableType, vector + (vector2 - vector) / 2f);
				this.AddBarrel(vector2, MovementMode.HORIZONTAL, 0f, 0f, RotationMode.NONE, rotation, 0f, automatic: false);
				break;
			}
			case 1:
			{
				float rotation = 0f;
				this.AddCollectable(collectableType, vector + (vector2 - vector) / 2f);
				this.AddBarrel(vector2, MovementMode.HORIZONTAL, 0f, 0f, RotationMode.CONSTANT, rotation, rotationValue, automatic: false);
				break;
			}
			case 2:
			{
				float rotation = ((vector2.Y < num2) ? ((float)Math.PI / 2f) : ((!(vector2.Y > 720f - num2)) ? ((float)Global.RandomDirection() * ((float)Math.PI / 2f)) : (-(float)Math.PI / 2f)));
				this.AddBarrel(vector2, MovementMode.HORIZONTAL, movementVelocity, num, RotationMode.NONE, rotation, 0f, automatic: false);
				vector2 += new Vector2(num, (float)Math.Sign(rotation) * num2);
				this.AddCollectable(collectableType, vector2 - new Vector2(0f, (float)Math.Sign(rotation) * num2 / 2f));
				rotation = 0f;
				this.AddBarrel(vector2, MovementMode.HORIZONTAL, 0f, 0f, RotationMode.NONE, rotation, 0f, automatic: false);
				break;
			}
			case 3:
			{
				float rotation = 0f;
				this.AddCollectable(collectableType, vector2 + new Vector2(0f, num / 2f));
				this.AddBarrel(vector2, MovementMode.VERTICAL, movementVelocity, num, RotationMode.NONE, rotation, 0f, automatic: false);
				break;
			}
			}
			break;
		case 1:
			switch (index)
			{
			case 0:
			{
				float rotation = 0f;
				this.AddCollectable(collectableType, vector + (vector2 - vector) / 2f);
				this.AddBarrel(vector2, MovementMode.HORIZONTAL, 0f, 0f, RotationMode.NONE, rotation, 0f, automatic: true);
				break;
			}
			case 1:
			{
				float rotation = ((vector2.Y < num2) ? ((float)Math.PI / 2f) : ((!(vector2.Y > 720f - num2)) ? ((float)Global.RandomDirection() * ((float)Math.PI / 2f)) : (-(float)Math.PI / 2f)));
				this.AddBarrel(vector2, MovementMode.HORIZONTAL, movementVelocity, num, RotationMode.NONE, rotation, 0f, automatic: true);
				vector2 += new Vector2(num, (float)Math.Sign(rotation) * num2);
				rotation = Global.RandomBetween(0f - (float)Math.Tanh(vector2.Y / width), (float)Math.Tanh((720f - vector2.Y) / width));
				this.AddCollectable(collectableType, vector2 - new Vector2(0f, (float)Math.Sign(rotation) * num2 / 2f));
				rotation = 0f;
				this.AddBarrel(vector2, MovementMode.HORIZONTAL, 0f, 0f, RotationMode.NONE, rotation, 0f, automatic: false);
				break;
			}
			case 2:
			{
				float rotation = 0f;
				this.AddCollectable(collectableType, vector2 + new Vector2(0f, num / 2f));
				this.AddBarrel(vector2, MovementMode.VERTICAL, movementVelocity, num, RotationMode.CONSTANT, rotation, rotationValue, automatic: false);
				break;
			}
			}
			break;
		case 2:
			if (index == 0)
			{
				float rotation = 0f;
				this.AddCollectable(collectableType, vector + (vector2 - vector) / 2f);
				this.AddBarrel(vector2, MovementMode.HORIZONTAL, 0f, 0f, RotationMode.CONSTANT, rotation, rotationValue, automatic: true);
			}
			break;
		}
	}

	private void ResetObstaclesBarrel()
	{
		MiniGame.tunnelSpeed_ = -240f;
		for (int i = 0; i < 15; i++)
		{
			this.AddObstacle(new Vector2(i * this.blockSizeX_, 326 - this.blockSizeY_ / 2), deadly: false, 0);
			this.AddObstacle(new Vector2(i * this.blockSizeX_, 394 + this.blockSizeY_ / 2), deadly: false, 0);
		}
		this.AddBarrel(new Vector2(15 * this.blockSizeX_ + 20, 360f), MovementMode.HORIZONTAL, 0f, 0f, RotationMode.NONE, 0f, 0f, automatic: false);
	}

	private void AnticipateChangeModeBarrel()
	{
		if (this.nextGameMode_ != GameMode.BARREL)
		{
			this.AddComboBarrel(1, 400f, 0, CollectableType.NONE);
			this.AddComboBarrel(0, 400f, 0, CollectableType.POINTS);
			this.barrels_[this.lastBarrelIndex_].ReferencePosition = new Vector2(this.barrels_[this.lastBarrelIndex_].ReferencePosition.X, 720 - (this.blockSizeY_ + this.barrelSize_));
		}
		if (this.nextGameMode_ == GameMode.BARREL)
		{
			this.dividerPosition_ = this.barrels_[this.lastBarrelIndex_].Position.X + (float)this.barrelSize_;
		}
		else
		{
			this.dividerPosition_ = this.barrels_[this.lastBarrelIndex_].Position.X + (float)this.barrelSize_ + (float)this.blockSizeX_;
		}
		if (this.nextGameMode_ == GameMode.PLATFORMER)
		{
			this.elevation_ = 0;
			this.elevationDirection_ = 1;
			this.AddObstacle(new Vector2(this.barrels_[this.lastBarrelIndex_].Position.X, 720 - this.blockSizeY_ / 2), deadly: false, 0);
			for (int i = 0; i < 10; i++)
			{
				this.AddObstacleD(1, 0);
			}
		}
		if (this.nextGameMode_ == GameMode.GRAVITY || this.nextGameMode_ == GameMode.HELICOPTER)
		{
			this.botElevation_ = 0;
			this.topElevation_ = 9;
			this.AddObstacle(new Vector2(this.barrels_[this.lastBarrelIndex_].Position.X, 720 - this.blockSizeY_ / 2), deadly: false, 0);
			this.AddObstacleB(0, this.topElevation_);
			for (int i = 0; i < 10; i++)
			{
				this.AddObstacleB(1, this.botElevation_);
				this.AddObstacleB(0, this.topElevation_);
			}
		}
	}
}
