using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class ScoreSystem
{
	private ScoreInfo scoreInfo_;

	private float score_;

	private float setScore_;

	private Vector2 scorePosition_ = new Vector2(1080f, 52f);

	private Vector2 scoreOffset_;

	private float scoreScale_ = 1f;

	private int multiplier_ = 1;

	private float multiplierTimer_;

	private float multiplierTime_ = 8f;

	private List<int> addedScores_ = new List<int>();

	private List<Vector2> addedScorePositions_ = new List<Vector2>();

	public int Score => (int)this.score_;

	public ScoreSystem()
	{
		this.scoreInfo_ = new ScoreInfo(0);
	}

	public void Update(float dt)
	{
		this.scoreOffset_ += new Vector2(-200f, 0f) * dt;
		if (this.scoreOffset_.X < 0f)
		{
			this.scoreOffset_ = Vector2.Zero;
		}
		this.scoreScale_ -= 1.7f * dt;
		if (this.scoreScale_ < 1f)
		{
			this.scoreScale_ = 1f;
		}
		for (int i = 0; i < this.addedScores_.Count; i++)
		{
			Vector2 vector = this.scorePosition_ + new Vector2(0f, 18f) - this.addedScorePositions_[i];
			vector.Normalize();
			this.addedScorePositions_[i] += vector * 1500f * dt;
			if (this.addedScorePositions_[i].X > this.scorePosition_.X)
			{
				this.score_ += this.addedScores_[i];
				this.scoreOffset_ = new Vector2(30f, 0f);
				this.scoreScale_ += 0.5f;
				this.addedScorePositions_.RemoveAt(i);
				this.addedScores_.RemoveAt(i);
				Global.PlayScore();
				break;
			}
		}
		this.multiplierTimer_ += dt;
		if (this.multiplierTimer_ > this.multiplierTime_)
		{
			this.multiplier_ = 1;
		}
		this.score_ += 100f * dt * (float)this.multiplier_;
	}

	public void Draw(SpriteBatch spriteBatch, OverallMode overallMode, bool userPaused)
	{
		if (userPaused && this.setScore_ != 0f)
		{
			spriteBatch.DrawString(Global.fontTex, ((int)this.setScore_).ToString(), this.scorePosition_ + new Vector2(-2f, 2f) + this.scoreOffset_, Color.Black, 0f, new Vector2(0f, 0f), this.scoreScale_, SpriteEffects.None, 0f);
			spriteBatch.DrawString(Global.fontTex, ((int)this.setScore_).ToString(), this.scorePosition_ + this.scoreOffset_, Color.White, 0f, new Vector2(0f, 0f), this.scoreScale_, SpriteEffects.None, 0f);
		}
		else
		{
			spriteBatch.DrawString(Global.fontTex, ((int)this.score_).ToString(), this.scorePosition_ + new Vector2(-2f, 2f) + this.scoreOffset_, Color.Black, 0f, new Vector2(0f, 0f), this.scoreScale_, SpriteEffects.None, 0f);
			spriteBatch.DrawString(Global.fontTex, ((int)this.score_).ToString(), this.scorePosition_ + this.scoreOffset_, Color.White, 0f, new Vector2(0f, 0f), this.scoreScale_, SpriteEffects.None, 0f);
		}
		for (int i = 0; i < this.addedScores_.Count; i++)
		{
			spriteBatch.DrawString(Global.fontTex, this.addedScores_[i].ToString(), this.addedScorePositions_[i] + new Vector2(-2f, 2f), Color.Black, 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
			spriteBatch.DrawString(Global.fontTex, this.addedScores_[i].ToString(), this.addedScorePositions_[i], Color.White, 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
		}
		float num;
		float num2;
		if (this.multiplierTimer_ < 0.3f)
		{
			num = this.multiplierTimer_ * 10f / 3f * ((float)Math.PI * 2f);
			num2 = this.multiplierTimer_ * 10f / 3f;
		}
		else if (this.multiplierTimer_ > this.multiplierTime_ - 0.3f)
		{
			num = (this.multiplierTimer_ - (this.multiplierTime_ - 0.3f)) * 10f / 3f * ((float)Math.PI * 2f);
			num2 = 1f - (this.multiplierTimer_ - (this.multiplierTime_ - 0.3f)) * 10f / 3f;
		}
		else
		{
			num = 0f;
			num2 = 1f;
		}
		num2 *= 0.8f;
		Vector2 vector = new Vector2(this.scorePosition_.X + 2f + 37f, this.scorePosition_.Y + 26f);
		switch (this.multiplier_)
		{
		case 1:
			break;
		case 2:
			spriteBatch.Draw(Global.collectableTex, vector + new Vector2(82f * (float)Math.Cos(this.multiplierTimer_ * 6f), 44f * (float)Math.Sin(this.multiplierTimer_ * 6f)), (Rectangle?)new Rectangle(52, 0, 80, 39), Color.White, num, new Vector2(40f, 19.5f), num2, SpriteEffects.None, 0f);
			break;
		case 3:
			spriteBatch.Draw(Global.collectableTex, vector + new Vector2(82f * (float)Math.Cos(this.multiplierTimer_ * 6f), 44f * (float)Math.Sin(this.multiplierTimer_ * 6f)), (Rectangle?)new Rectangle(132, 0, 80, 39), Color.White, num, new Vector2(40f, 19.5f), num2, SpriteEffects.None, 0f);
			break;
		}
	}

	public void DrawLeaderboard(SpriteBatch spriteBatch)
	{
		this.scoreInfo_.DrawLeaderboard(spriteBatch);
	}

	public void AddToScore(float score, Vector2 position)
	{
		this.addedScores_.Add((int)(score * (float)this.multiplier_));
		this.addedScorePositions_.Add(position);
	}

	public void SetMultiplier(int multiplier)
	{
		this.multiplier_ = multiplier;
		this.multiplierTimer_ = 0f;
	}

	public void LoadInfo()
	{
		this.scoreInfo_ = ScoreInfo.LoadInfo();
	}

	public void SaveInfo()
	{
		this.scoreInfo_.SaveInfo();
	}

	public void Reset()
	{
		this.score_ = 0f;
		this.multiplier_ = 1;
		this.multiplierTimer_ = 0f;
		this.scoreOffset_ = Vector2.Zero;
		this.scoreScale_ = 1f;
		this.addedScores_.Clear();
		this.addedScorePositions_.Clear();
	}

	public void End(OverallMode overallMode, int characterIndex)
	{
		this.setScore_ = 0f;
		this.scoreInfo_.AddScore(overallMode, (int)this.score_, characterIndex);
	}

	public void End(OverallMode overallMode, int characterIndex, float score)
	{
		this.score_ = score;
		this.setScore_ = score;
		this.scoreInfo_.AddScore(overallMode, (int)this.score_, characterIndex);
	}

	public int GetHighScore(OverallMode overallMode)
	{
		return this.scoreInfo_.GetHighScore(overallMode);
	}
}
