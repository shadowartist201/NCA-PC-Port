using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class Obstacle : AnimatedSprite
{
	private bool deadly_;

	public MyRectangle CollisionRect => new MyRectangle(base.position_.X - base.origin_.X, base.position_.Y - base.origin_.Y, base.sourceRectangle_.Value.Width, base.sourceRectangle_.Value.Height);

	public bool Deadly => this.deadly_;

	public Obstacle()
		: base(Global.obstacleTex, null, 1, 1f, Vector2.Zero)
	{
		base.visible_ = false;
	}

	public void Reset(Rectangle sourceRect, Vector2 position, bool deadly)
	{
		base.ResetAnimation(sourceRect, 1, 1f);
		base.position_ = position;
		this.deadly_ = deadly;
		base.visible_ = true;
	}

	public void ResetWithAnim(Rectangle sourceRect, int numFrames, float frameTime, Vector2 position, bool deadly)
	{
		base.ResetAnimation(sourceRect, numFrames, frameTime);
		base.position_ = position;
		this.deadly_ = deadly;
		base.visible_ = true;
	}

	public new void Update(float dt)
	{
		base.position_.X += MiniGame.tunnelSpeed_ * dt;
		if (base.position_.X + base.origin_.X < 0f)
		{
			base.visible_ = false;
		}
		base.Update(dt);
	}

	public new void Draw(SpriteBatch spriteBatch)
	{
		if (base.visible_)
		{
		}
		base.Draw(spriteBatch);
	}

	public void Hide()
	{
		base.visible_ = false;
	}
}
