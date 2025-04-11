using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NyanAdventure;

internal class EffectManager
{
	private Texture2D noiseTex;

	private float alpha;

	private float alphaRate;

	private float alphaMin;

	private float alphaMax;

	private Vector2 offset;

	private Vector2 offsetRate;

	private Vector2 offsetMax = Vector2.One;

	private Vector2 waveDimensions;

	private Vector2 waveDimensionsRate;

	private Vector2 waveDimensionsMin;

	private Vector2 waveDimensionsMax;

	private float theta;

	private float thetaRate;

	private float strength;

	private int effectIndex;

	private Effect[] effects;

	private int numEffects = 8;

	private float timer;

	private float effectTimer;

	private float effectTime;

	public EffectManager(ContentManager content, GraphicsDevice graphicsDevice)
	{
		this.noiseTex = content.Load<Texture2D>("Graphics//Effects//noise");
		this.effects = new Effect[this.numEffects];
		for (int i = 0; i < this.effects.Length; i++)
		{
			this.effects[i] = content.Load<Effect>("Effects//effect" + i);
		}
		this.SetEffect(-1);
	}

	public void Update(float dt)
	{
		this.effectTimer += dt;
		if (this.effectTimer > this.effectTime && this.effectIndex != -1)
		{
			this.SetEffect(-1);
		}
		this.timer += dt;
		if (this.timer > 108000f)
		{
			this.timer = 0f;
		}
		this.alpha += this.alphaRate * dt;
		if (this.alpha < this.alphaMin)
		{
			this.alpha = this.alphaMin;
			this.alphaRate = 0f - this.alphaRate;
		}
		if (this.alpha > this.alphaMax)
		{
			this.alpha = this.alphaMax;
			this.alphaRate = 0f - this.alphaRate;
		}
		this.offset += this.offsetRate * dt;
		this.offset.X %= this.offsetMax.X;
		this.offset.Y %= this.offsetMax.Y;
		this.waveDimensions += this.waveDimensionsRate * dt;
		if (this.waveDimensions.X < this.waveDimensionsMin.X)
		{
			this.waveDimensions.X = this.waveDimensionsMin.X;
			this.waveDimensionsRate.X = 0f - this.waveDimensionsRate.X;
		}
		if (this.waveDimensions.Y < this.waveDimensionsMin.Y)
		{
			this.waveDimensions.Y = this.waveDimensionsMin.Y;
			this.waveDimensionsRate.Y = 0f - this.waveDimensionsRate.Y;
		}
		if (this.waveDimensions.X > this.waveDimensionsMax.X)
		{
			this.waveDimensions.X = this.waveDimensionsMax.X;
			this.waveDimensionsRate.X = 0f - this.waveDimensionsRate.X;
		}
		if (this.waveDimensions.Y > this.waveDimensionsMax.Y)
		{
			this.waveDimensions.Y = this.waveDimensionsMax.Y;
			this.waveDimensionsRate.Y = 0f - this.waveDimensionsRate.Y;
		}
		this.theta += this.thetaRate * dt;
		this.theta %= (float)Math.PI * 2f;
	}

	public void Draw(SpriteBatch spriteBatch, RenderTarget2D renderTarget, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice)
	{
		graphicsDevice.SetRenderTarget(null);
		switch (this.effectIndex)
		{
		case 2:
			this.effects[this.effectIndex].Parameters["Offset"].SetValue(new Vector2((float)Math.Cos(this.theta), (float)Math.Sin(this.theta)));
			break;
		case 5:
			graphics.GraphicsDevice.Textures[1] = this.noiseTex;
			graphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
			this.effects[this.effectIndex].Parameters["Offset"].SetValue(this.offset);
			break;
		case 6:
			this.effects[this.effectIndex].Parameters["WaveDimensions"].SetValue(new Vector2(this.waveDimensions.X, this.waveDimensions.Y)); //wavy
			this.effects[this.effectIndex].Parameters["Timer"].SetValue(this.timer);
			break;
		case 7:
			this.effects[this.effectIndex].Parameters["Timer"].SetValue(this.timer);
			this.effects[this.effectIndex].Parameters["Strength"].SetValue(this.strength);
			break;
		}
		if (this.effectIndex == -1)
		{
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
			spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
			spriteBatch.End();
		}
		else
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, this.effects[this.effectIndex]);
			spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White * this.alpha);
			spriteBatch.End();
		}
	}

	public void SetEffectRandom()
	{
		this.SetEffect(Global.Random.Next(this.numEffects));
	}

	public void SetEffectOff()
	{
		this.SetEffect(-1);
	}

	private void SetEffect(int index)
	{
		if (index == -1)
		{
			Global.SetVibration(0f);
		}
		this.effectIndex = index;
		this.effectTimer = 0f;
		this.effectTime = 6f;
		switch (this.effectIndex)
		{
		case 0:
			this.alpha = 0.5f;
			this.alphaRate = 10f;
			this.alphaMin = 0.2f;
			this.alphaMax = 0.8f;
			Global.SetVibration(0.1f); //black white flash
			break;
		case 1:
			Global.SetVibration(0.1f); //invert colors filter
			break;
		case 2:
			this.alpha = 0.1f;
			this.alphaRate = 4f;
			this.alphaMin = 0.1f;
			this.alphaMax = 0.4f;
			this.theta = 0f;
			this.thetaRate = 8f;
			Global.SetVibration(0.3f); //shake in circles
			break;
		case 3:
			Global.SetVibration(0.1f); //edge detect dark
			break;
		case 4:
			this.alpha = 1f;
			this.alphaRate = 0f;
			this.alphaMin = 0.2f;
			this.alphaMax = 1f;
			Global.SetVibration(0.1f); //pixelate
			break;
		case 5:
			this.alpha = 0.15f;
			this.alphaRate = 3f;
			this.alphaMin = 0.05f;
			this.alphaMax = 0.3f;
			this.offsetRate = new Vector2(4f, 4f);
			Global.SetVibration(0.3f); //noise effect (like frosted glass)
			break;
		case 6:
			this.alpha = 1f;
			this.alphaRate = 0f;
			this.alphaMin = 0f;
			this.alphaMax = 1f;
			this.waveDimensions = new Vector2(10f, 0.03f);
			this.waveDimensionsRate = new Vector2(0f, 0f);
			this.waveDimensionsMin = new Vector2(0f, 0f);
			this.waveDimensionsMax = new Vector2(100f, 1f);
			Stage.background_.SetMode(BackgroundMode.BW);
			Global.SetVibration(0.3f); //sin waves
			break;
		case 7:
			this.alpha = 1f;
			this.alphaRate = 0f;
			this.alphaMin = 0f;
			this.alphaMax = 1f;
			this.strength = 0.015f;
			Global.SetVibration(0.3f); //drunk, shake with blur
			break;
		}
	}
}
