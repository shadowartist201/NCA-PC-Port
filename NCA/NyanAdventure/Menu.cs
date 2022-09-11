using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal class Menu
{
	private MenuItem titleItem_;

	protected List<MenuItem> menuItems_;

	protected int index_;

	protected Menu(string title)
	{
		this.titleItem_ = new MenuItem(title, new Vector2(640f, 290f), Color.White);
		this.titleItem_.DefaultColor = new Color(0, 246, 255);
		this.menuItems_ = new List<MenuItem>();
	}

	protected void AddMenuItem(MenuItem menuItem)
	{
		this.menuItems_.Add(menuItem);
	}

	protected void Update(float dt, KeyboardState currKState, KeyboardState prevKState, GamePadState currPState, GamePadState prevPState)
	{
		for (int i = 0; i < this.menuItems_.Count; i++)
		{
			this.menuItems_[i].Update(dt, i == this.index_);
		}
		if (((currKState.IsKeyDown(Keys.Down) && prevKState.IsKeyUp(Keys.Down)) || (currPState.IsButtonDown(Buttons.DPadDown) && prevPState.IsButtonUp(Buttons.DPadDown)) || (currPState.ThumbSticks.Left.Y < -0.5f && prevPState.ThumbSticks.Left.Y >= -0.5f)) && this.index_ + 1 < this.menuItems_.Count)
		{
			this.index_++;
			Global.PlayMenuScroll();
		}
		if (((currKState.IsKeyDown(Keys.Up) && prevKState.IsKeyUp(Keys.Up)) || (currPState.IsButtonDown(Buttons.DPadUp) && prevPState.IsButtonUp(Buttons.DPadUp)) || (currPState.ThumbSticks.Left.Y > 0.5f && prevPState.ThumbSticks.Left.Y <= 0.5f)) && this.index_ > 0)
		{
			this.index_--;
			Global.PlayMenuScroll();
		}
	}

	protected void Draw(SpriteBatch spriteBatch, Background background)
	{
		background.DrawStars(spriteBatch);
		spriteBatch.Draw(Global.titleBGTex, new Vector2(0f, 85f), Color.White);
		spriteBatch.Draw(Global.titleTex, new Vector2(465f, -10f), Color.White);
		this.titleItem_.Draw(spriteBatch, selected: false);
		for (int i = 0; i < this.menuItems_.Count; i++)
		{
			this.menuItems_[i].Draw(spriteBatch, i == this.index_);
		}
	}
}
