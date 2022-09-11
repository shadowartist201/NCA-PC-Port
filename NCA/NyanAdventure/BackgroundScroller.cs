using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class BackgroundScroller
{
	private Sprite stationaryElement_;

	private List<BackgroundScrollerElement> elements_ = new List<BackgroundScrollerElement>();

	private float scrollSpeed_ = 200f;

	private Vector2 basePosition_ = new Vector2(0f, 720f);

	private float minX_ = -230f;

	private float width_ = 2022f;

	public BackgroundScroller()
	{
		this.stationaryElement_ = new Sprite(Global.grassTex, null, new Vector2(Global.grassTex.Width / 2, 900f));
		int[] array = new int[18]
		{
			4, 4, 1, 2, 0, 0, 0, 5, 4, 3,
			0, 1, 2, 2, 5, 5, 5, 3
		};
		int[] array2 = new int[6] { 4, 9, 4, 15, 16, 18 };
		float[] array3 = new float[18]
		{
			13f, 85f, 182f, 446f, 501f, 567f, 632f, 698f, 840f, 930f,
			1052f, 1123f, 1386f, 1437f, 1489f, 1628f, 1766f, 1911f
		};
		int num = 0;
		int[] array4 = array;
		foreach (int num2 in array4)
		{
			this.elements_.Add(new BackgroundScrollerElement(Global.buildingTex[num2], new Rectangle(0, 0, Global.buildingTex[num2].Width / array2[num2], Global.buildingTex[num2].Height), array2[num2], 0.07f, new Vector2((float)(Global.buildingTex[num2].Width / array2[num2] / 2) + array3[num], -Global.buildingTex[num2].Height / 2)));
			num++;
		}
	}

	public void Update(float dt, float positionY)
	{
		this.basePosition_.Y = positionY;
		this.basePosition_.X -= this.scrollSpeed_ * dt;
		if (this.basePosition_.X < this.minX_)
		{
			this.basePosition_.X += this.width_;
		}
		foreach (BackgroundScrollerElement item in this.elements_)
		{
			item.Update(dt, this.basePosition_, this.minX_, this.width_);
		}
		this.stationaryElement_.PositionY = this.basePosition_.Y - (float)(Global.grassTex.Height / 2) + 49f;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		this.stationaryElement_.Draw(spriteBatch);
		foreach (BackgroundScrollerElement item in this.elements_)
		{
			item.Draw(spriteBatch);
		}
	}
}
