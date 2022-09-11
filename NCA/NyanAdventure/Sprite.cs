using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class Sprite
{
	protected Texture2D texture_;

	protected Vector2 position_;

	protected Rectangle? sourceRectangle_;

	protected Color color_;

	protected float rotation_;

	protected Vector2 origin_;

	protected Vector2 scale_;

	protected SpriteEffects spriteEffects_;

	protected float layerDepth_;

	protected bool visible_;

	public Vector2 Position
	{
		get
		{
			return this.position_;
		}
		set
		{
			this.position_ = value;
		}
	}

	public float PositionX
	{
		get
		{
			return this.position_.X;
		}
		set
		{
			this.position_.X = value;
		}
	}

	public float PositionY
	{
		get
		{
			return this.position_.Y;
		}
		set
		{
			this.position_.Y = value;
		}
	}

	public Color Color
	{
		get
		{
			return this.color_;
		}
		set
		{
			this.color_ = value;
		}
	}

	public float Alpha
	{
		set
		{
			this.color_.A = (byte)(value * 255f);
		}
	}

	public float Rotation
	{
		get
		{
			return this.rotation_;
		}
		set
		{
			this.rotation_ = value;
		}
	}

	public float ScaleX
	{
		set
		{
			this.scale_.X = value;
		}
	}

	public Vector2 Scale
	{
		set
		{
			this.scale_ = value;
		}
	}

	public bool Visible => this.visible_;

	public Sprite(Texture2D texture, Rectangle? sourceRectangle, Vector2 position)
	{
		this.texture_ = texture;
		this.sourceRectangle_ = sourceRectangle;
		this.position_ = position;
		this.color_ = Color.White;
		this.rotation_ = 0f;
		if (this.sourceRectangle_.HasValue)
		{
			this.origin_ = new Vector2(this.sourceRectangle_.Value.Width / 2, this.sourceRectangle_.Value.Height / 2);
		}
		else
		{
			this.origin_ = new Vector2(texture.Width / 2, texture.Height / 2);
		}
		this.scale_ = Vector2.One;
		this.spriteEffects_ = SpriteEffects.None;
		this.layerDepth_ = 0f;
		this.visible_ = true;
	}

	public void SetSourceRect(Rectangle sourceRectangle)
	{
		this.sourceRectangle_ = sourceRectangle;
		this.origin_ = new Vector2(this.sourceRectangle_.Value.Width / 2, this.sourceRectangle_.Value.Height / 2);
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		if (this.visible_)
		{
			spriteBatch.Draw(this.texture_, this.position_, this.sourceRectangle_, this.color_, this.rotation_, this.origin_, this.scale_, this.spriteEffects_, this.layerDepth_);
		}
	}

	public void DrawMirrored(SpriteBatch spriteBatch)
	{
		if (this.visible_)
		{
			spriteBatch.Draw(this.texture_, new Vector2(1280f - this.position_.X, this.position_.Y), this.sourceRectangle_, this.color_, this.rotation_, this.origin_, this.scale_, SpriteEffects.FlipHorizontally, this.layerDepth_);
		}
	}
}
