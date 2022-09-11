using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class FPSDisplay
{
	private float fpsTimer;

	private int fps;

	private int frameCount;

	public void Update(float dt)
	{
		this.fpsTimer += dt;
		this.frameCount++;
		if (this.fpsTimer >= 1f)
		{
			this.fps = this.frameCount;
			this.fpsTimer = 0f;
			this.frameCount = 0;
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.DrawString(Global.menuFontTex, this.fps.ToString(), Vector2.Zero, Color.White);
	}
}
