using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class BackgroundScrollerElement : AnimatedSprite
{
	private Vector2 scrollPosition_;

	public BackgroundScrollerElement(Texture2D texture, Rectangle? frameInfo, int frameCount, float frameTime, Vector2 position)
		: base(texture, frameInfo, frameCount, frameTime, position)
	{
		this.scrollPosition_ = position;
	}

	public void Update(float dt, Vector2 basePosition, float minX, float width)
	{
		base.position_ = new Vector2(minX + (this.scrollPosition_.X + basePosition.X - minX) % width, basePosition.Y + this.scrollPosition_.Y);
		base.Update(dt);
	}
}
