using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class MenuItem
{
	private bool textured_;

	private Texture2D texture_;

	private Rectangle sourceRectangle_;

	private float scale_ = 1f;

	private float maxScale_ = 1f;

	private float scaleRate_ = 1f;

	private string text_;

	private Vector2 position_;

	private Vector2 origin_;

	private Color defaultColor_;

	private Color selectedColor_;

	private float rightStart_;

	private int rightEnd_ = 1280;

	private int leftEnd_ = 0;

	private float selectorOffset_;

	private float selectorOffsetMin_ = -20f;

	private float selectorOffsetMax_ = 0f;

	private float selectorOffsetRate_ = -120f;

	public Vector2 Position => this.position_;

	public Color DefaultColor
	{
		set
		{
			this.defaultColor_ = value;
		}
	}

	public MenuItem(string text, Vector2 position, Color selectedColor)
	{
		this.textured_ = false;
		this.text_ = text;
		this.origin_ = Global.menuFontTex.MeasureString(text) / 2f;
		this.position_ = position;
		this.defaultColor_ = Color.White;
		this.selectedColor_ = selectedColor;
	}

	public MenuItem(Texture2D texture, Rectangle sourceRectangle, Vector2 position, Color selectedColor, float scale)
	{
		this.textured_ = true;
		this.texture_ = texture;
		this.origin_ = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
		this.sourceRectangle_ = sourceRectangle;
		this.position_ = position;
		this.defaultColor_ = Color.White * 0.3f;
		this.selectedColor_ = selectedColor;
		this.maxScale_ = scale;
		this.scale_ = scale;
	}

	public MenuItem(string text, Vector2 position, Color selectedColor, float rightStart)
		: this(text, position, selectedColor)
	{
		this.rightStart_ = rightStart;
	}

	public MenuItem(string text, Vector2 position, Color selectedColor, int leftEnd, int rightEnd)
		: this(text, position, selectedColor)
	{
		this.leftEnd_ = leftEnd;
		this.rightEnd_ = rightEnd;
	}

	public void Update(float dt, bool selected)
	{
		if (selected)
		{
			this.scale_ += this.scaleRate_ * dt;
			if (this.scale_ > this.maxScale_)
			{
				this.scale_ = this.maxScale_;
				this.scaleRate_ = 0f - this.scaleRate_;
			}
			if (this.scale_ < this.maxScale_ * 0.8f)
			{
				this.scale_ = this.maxScale_ * 0.8f;
				this.scaleRate_ = 0f - this.scaleRate_;
			}
			this.selectorOffset_ += this.selectorOffsetRate_ * dt;
			if (this.selectorOffset_ < this.selectorOffsetMin_)
			{
				this.selectorOffset_ = this.selectorOffsetMin_;
				this.selectorOffsetRate_ = 0f - this.selectorOffsetRate_;
			}
			if (this.selectorOffset_ > this.selectorOffsetMax_)
			{
				this.selectorOffset_ = this.selectorOffsetMax_;
				this.selectorOffsetRate_ = 0f - this.selectorOffsetRate_;
			}
		}
	}

	public void Draw(SpriteBatch spriteBatch, bool selected)
	{
		if (!this.textured_)
		{
			spriteBatch.DrawString(Global.menuFontTex, this.text_, this.position_, selected ? this.selectedColor_ : this.defaultColor_, 0f, this.origin_, selected ? this.scale_ : this.maxScale_, SpriteEffects.None, 0f);
		}
		else
		{
			spriteBatch.Draw(this.texture_, this.position_, (Rectangle?)this.sourceRectangle_, selected ? this.selectedColor_ : this.defaultColor_, 0f, this.origin_, selected ? this.scale_ : (this.scale_ / 2f), SpriteEffects.None, 0f);
		}
		if (selected)
		{
			spriteBatch.Draw(Global.selectBarTex, new Rectangle(this.leftEnd_, (int)(this.position_.Y - this.origin_.Y * this.scale_ / 2f), (int)(this.position_.X - this.origin_.X * this.scale_ - 11f) + (int)this.selectorOffset_ - this.leftEnd_, 44), Color.White);
			if (this.rightStart_ == 0f)
			{
				spriteBatch.Draw(Global.selectBarTex, new Rectangle((int)(this.position_.X + this.origin_.X * this.scale_ + 6f) - (int)this.selectorOffset_, (int)(this.position_.Y - this.origin_.Y * this.scale_ / 2f), (int)((float)this.rightEnd_ - (this.position_.X + this.origin_.X * this.scale_ + 6f) + this.selectorOffset_), 44), Color.White);
			}
			else
			{
				spriteBatch.Draw(Global.selectBarTex, new Rectangle((int)(this.rightStart_ - this.selectorOffset_), (int)(this.position_.Y - this.origin_.Y * this.scale_ / 2f), (int)((float)this.rightEnd_ - this.rightStart_ + this.selectorOffset_), 44), Color.White);
			}
		}
	}
}
