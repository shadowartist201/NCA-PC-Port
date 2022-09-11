namespace NyanAdventure;

internal class MyRectangle
{
	public float X;

	public float Y;

	public float Width;

	public float Height;

	public MyRectangle(float x, float y, float width, float height)
	{
		this.X = x;
		this.Y = y;
		this.Width = width;
		this.Height = height;
	}

	public bool Intersects(MyRectangle rectangle2)
	{
		if (this.X + this.Width < rectangle2.X || this.X > rectangle2.X + rectangle2.Width || this.Y > rectangle2.Y + rectangle2.Height || this.Y + this.Height < rectangle2.Y)
		{
			return false;
		}
		return true;
	}
}
