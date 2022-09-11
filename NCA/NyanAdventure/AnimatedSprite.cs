using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class AnimatedSprite : Sprite
{
	protected Rectangle frameInfo_;

	private int currentFrame_;

	protected int frameCount_;

	private float frameTimer_;

	private float frameTime_;

	public Rectangle FrameInfo => this.frameInfo_;

	public AnimatedSprite(Texture2D texture, Rectangle? frameInfo, int frameCount, float frameTime, Vector2 position)
		: base(texture, frameInfo, position)
	{
		if (!frameInfo.HasValue)
		{
			this.frameInfo_ = new Rectangle(0, 0, texture.Width, texture.Height);
		}
		else
		{
			this.frameInfo_ = frameInfo.Value;
		}
		this.frameCount_ = frameCount;
		this.frameTime_ = frameTime;
		this.currentFrame_ = Global.Random.Next(frameCount);
		base.sourceRectangle_ = new Rectangle(this.frameInfo_.X + this.frameInfo_.Width * this.currentFrame_, this.frameInfo_.Y, this.frameInfo_.Width, this.frameInfo_.Height);
		this.frameTimer_ = 0f;
	}

	public void ResetAnimation(Texture2D texture, Rectangle frameInfo, int frameCount, float frameTime)
	{
		base.texture_ = texture;
		this.ResetAnimation(frameInfo, frameCount, frameTime);
	}

	public void ResetAnimation(Rectangle frameInfo, int frameCount, float frameTime)
	{
		this.frameInfo_ = frameInfo;
		this.frameCount_ = frameCount;
		this.frameTime_ = frameTime;
		this.currentFrame_ = Global.Random.Next(frameCount);
		Rectangle sourceRect = new Rectangle(this.frameInfo_.X + this.frameInfo_.Width * this.currentFrame_, this.frameInfo_.Y, this.frameInfo_.Width, this.frameInfo_.Height);
		base.SetSourceRect(sourceRect);
		this.frameTimer_ = 0f;
	}

	public void Update(float dt)
	{
		this.frameTimer_ += dt;
		if (this.frameTimer_ >= this.frameTime_)
		{
			this.frameTimer_ -= this.frameTime_;
			this.currentFrame_ = (this.currentFrame_ + 1) % this.frameCount_;
			base.sourceRectangle_ = new Rectangle(this.frameInfo_.X + this.frameInfo_.Width * this.currentFrame_, this.frameInfo_.Y, this.frameInfo_.Width, this.frameInfo_.Height);
		}
	}
}
