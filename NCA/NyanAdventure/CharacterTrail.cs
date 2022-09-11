using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class CharacterTrail
{
	private bool visible_;

	private List<Sprite> parts_ = new List<Sprite>();

	private int partWidth_ = 12;

	private int partHeight_ = 56;

	private int fluctuationOffset_ = 0;

	private int fluctuationIndex_ = 0;

	private bool wasAttached_;

	public CharacterTrail()
	{
		for (int i = 0; i < 32; i++)
		{
			this.parts_.Add(new Sprite(Global.characterTrailTex, new Rectangle(0, 0, this.partWidth_, this.partHeight_), Vector2.Zero));
		}
	}

	public void Update(float dt, Vector2 characterPosition, bool attached)
	{
		this.parts_.Sort(CompareSpritesByPositionX);
		foreach (Sprite item in this.parts_)
		{
			if (!attached)
			{
				item.PositionX += MiniGame.tunnelSpeed_ * dt * 1.5f;
			}
			else if (item.PositionX > -64f)
			{
				Vector2 vector = characterPosition - item.Position;
				vector.Normalize();
				item.Position += vector * 1000f * dt;
				if (item.PositionX > characterPosition.X)
				{
					item.PositionX = -64 * this.partWidth_;
				}
			}
		}
		if (!attached)
		{
			for (int num = this.parts_.Count - 2; num >= 0; num--)
			{
				if (this.parts_[num].PositionX > 0f)
				{
					this.parts_[num].PositionX = this.parts_[num + 1].PositionX - (float)this.partWidth_ + 0.05f;
				}
			}
		}
		if (!attached)
		{
			float positionX = this.parts_[this.parts_.Count - 1].PositionX;
			if (positionX < characterPosition.X - (float)(3 * this.partWidth_))
			{
				int num2 = (int)((characterPosition.X - positionX - (float)(2 * this.partWidth_)) / (float)this.partWidth_);
				if (num2 > this.parts_.Count)
				{
					num2 = this.parts_.Count;
				}
				for (int num = 0; num < 1; num++)
				{
					this.fluctuationIndex_++;
					if (this.fluctuationIndex_ > 4)
					{
						this.fluctuationIndex_ = 0;
						this.fluctuationOffset_ = (this.fluctuationOffset_ + 1) % 2;
					}
					if (!this.wasAttached_)
					{
						this.parts_[num].PositionX = positionX + (float)((num + 1) * this.partWidth_);
					}
					else
					{
						this.parts_[num].PositionX = characterPosition.X - (float)(2 * this.partWidth_);
						this.wasAttached_ = false;
					}
					this.parts_[num].PositionY = characterPosition.Y + (float)(this.fluctuationOffset_ * 3);
				}
			}
		}
		this.wasAttached_ = attached;
	}

	private static int CompareSpritesByPositionX(Sprite x, Sprite y)
	{
		if (x.PositionX < y.PositionX)
		{
			return -1;
		}
		if (y.PositionX < x.PositionX)
		{
			return 1;
		}
		return 0;
	}

	public void Draw(SpriteBatch spriteBatch, Vector2 scale, Color color, bool attached)
	{
		if (!this.visible_)
		{
			return;
		}
		for (int i = 0; i < this.parts_.Count; i++)
		{
			if (!attached)
			{
				this.parts_[i].Scale = scale;
			}
			else
			{
				this.parts_[i].Scale = Vector2.One;
			}
			this.parts_[i].Color = color;
			this.parts_[i].Draw(spriteBatch);
		}
	}

	public void DrawMirrored(SpriteBatch spriteBatch, Vector2 scale, Color color, bool attached)
	{
		if (!this.visible_)
		{
			return;
		}
		for (int i = 0; i < this.parts_.Count; i++)
		{
			if (!attached)
			{
				this.parts_[i].Scale = scale;
			}
			else
			{
				this.parts_[i].Scale = Vector2.One;
			}
			this.parts_[i].Color = color;
			this.parts_[i].DrawMirrored(spriteBatch);
		}
	}

	public void SetTexture(int index)
	{
		if (index != -1)
		{
			this.visible_ = true;
			if (index != 3)
			{
				foreach (Sprite item in this.parts_)
				{
					item.SetSourceRect(new Rectangle(index * this.partWidth_ * 2, 0, this.partWidth_, this.partHeight_));
				}
				return;
			}
			for (int i = 0; i < this.parts_.Count; i++)
			{
				this.parts_[i].SetSourceRect(new Rectangle(3 * this.partWidth_ * 2 + this.partWidth_ * (i % 12), 0, this.partWidth_, this.partHeight_));
			}
		}
		else
		{
			this.visible_ = false;
		}
	}

	public void Reset(Vector2 characterPosition)
	{
		for (int i = 0; i < this.parts_.Count; i++)
		{
			this.fluctuationIndex_++;
			if (this.fluctuationIndex_ > 4)
			{
				this.fluctuationIndex_ = 0;
				this.fluctuationOffset_ = (this.fluctuationOffset_ + 1) % 2;
			}
			this.parts_[i].PositionX = characterPosition.X - (float)((i + 3) * this.partWidth_);
			this.parts_[i].PositionY = characterPosition.Y + (float)(this.fluctuationOffset_ * 3);
		}
		this.wasAttached_ = false;
	}

	public void Offset(float dx)
	{
		foreach (Sprite item in this.parts_)
		{
			item.PositionX += dx;
		}
	}
}
