using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NCA_Android;

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

	protected void Update(float dt, InputState inputState)
	{
		for (int i = 0; i < this.menuItems_.Count; i++)
		{
			this.menuItems_[i].Update(dt, i == this.index_);
		}
		if (inputState.IsButtonPressed(Buttons.DPadDown) && this.index_ + 1 < this.menuItems_.Count)
		{
			this.index_++;
			Global.PlayMenuScroll();
		}
		if (inputState.IsButtonPressed(Buttons.DPadUp) && this.index_ > 0)
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
