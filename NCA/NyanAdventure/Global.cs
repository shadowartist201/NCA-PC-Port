using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NyanAdventure;

internal static class Global
{
	public static bool IsTrialMode;

	public static PlayerIndex? PlayerIndex = null;

	public static StorageDeviceManager DeviceManager;

	public static Texture2D introCharTex;

	public static Texture2D introToasterTex;

	public static Texture2D introToasterFront;

	public static Texture2D introHandleTex;

	public static Texture2D introTrailTex;

	public static Texture2D obstacleTex;

	public static Texture2D[] characterTex = new Texture2D[8];

	public static Texture2D characterListTex;

	public static Texture2D characterTrailTex;

	public static Texture2D[] bSpriteTex = new Texture2D[6];

	public static Texture2D bgLightTex;

	public static Texture2D bgDarkTex;

	public static Texture2D collectableTex;

	public static Texture2D starTex;

	public static Texture2D bwBackgroundTex;

	public static Texture2D bwTransitionTex;

	public static Texture2D[] asteroidTex = new Texture2D[4];

	public static Texture2D[] planetTex = new Texture2D[2];

	public static Texture2D[] buildingTex = new Texture2D[6];

	public static Texture2D grassTex;

	public static Texture2D healthTex;

	public static Texture2D timerInsideTex;

	public static Texture2D timerOutlineTex;

	public static Texture2D splashBGTex;

	public static Texture2D splashFGTex;

	public static Texture2D offBarTex;

	public static Texture2D onBarTex;

	public static Texture2D sliderBarTex;

	public static Texture2D selectBarTex;

	public static Texture2D titleTex;

	public static Texture2D titleBGTex;

	public static Texture2D popupTex;

	public static Texture2D aButtonTex;

	public static Texture2D bButtonTex;

	public static Texture2D modeSymbolTex;

	public static Texture2D countdownTex;

	public static Texture2D transitionLineTex;

	public static SpriteFont fontTex;

	public static SpriteFont menuFontTex;

	public static SpriteFont creditsFontTex;

	public static float SFXVolume = 1f;

	public static SoundEffect[] explosionSfx = new SoundEffect[2];

	public static SoundEffect[] hurtSfx = new SoundEffect[4];

	public static SoundEffect[] jumpSfx = new SoundEffect[4];

	public static SoundEffect[] landSfx = new SoundEffect[3];

	public static SoundEffect[] powerupSfx = new SoundEffect[4];

	public static SoundEffect[] powerdownSfx = new SoundEffect[4];

	public static SoundEffect[] scoreSfx = new SoundEffect[4];

	public static SoundEffect boostSfx;

	public static SoundEffect transitionSfx;

	public static SoundEffect countdownSfx;

	public static SoundEffect goSfx;

	public static SoundEffect deathSfx;

	private static SoundEffectInstance boostInstance;

	public static SoundEffect menuSelectSfx;

	public static SoundEffect menuScrollSfx;

	public static SoundEffect menuBackSfx;

	public static SoundEffect menuStartSfx;

	public static SoundEffect menuYeahSfx;

	private static Vector2 screenSize = new Vector2(1280f, 720f);

	private static Random random_ = new Random();

	public static bool vibrationOn = true;

	private static bool vibrationPaused = false;

	private static float vibrationStrength = 0f;

	public static Vector2 ScreenSize => Global.screenSize;

	public static Random Random => Global.random_;

	public static void PlayExplosion()
	{
		Global.explosionSfx[Global.random_.Next(2)].Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayHurt()
	{
		Global.hurtSfx[Global.random_.Next(2)].Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayJump()
	{
		Global.jumpSfx[Global.random_.Next(2)].Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayPowerdown()
	{
		Global.powerdownSfx[Global.random_.Next(2)].Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayDeath()
	{
		Global.deathSfx.Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayLand()
	{
		Global.landSfx[Global.random_.Next(3)].Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayPowerup(int index)
	{
		Global.powerupSfx[index].Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayScore()
	{
		Global.scoreSfx[Global.random_.Next(2)].Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayCountdown()
	{
		Global.countdownSfx.Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayGo()
	{
		Global.goSfx.Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayBoost(float dt)
	{
		if (Global.boostInstance == null)
		{
			Global.boostInstance = Global.boostSfx.CreateInstance();
			Global.boostInstance.IsLooped = true;
			Global.boostInstance.Volume = 0.6f * Global.SFXVolume;
			Global.boostInstance.Play();
		}
		if (Global.boostInstance.State != 0)
		{
			Global.boostInstance.Volume = 0f;
			Global.boostInstance.Resume();
		}
		else if (Global.SFXVolume > 0f)
		{
			if (Global.boostInstance.Volume + dt * 2f > 0.6f)
			{
				Global.boostInstance.Volume = 0.6f;
			}
			else
			{
				Global.boostInstance.Volume += dt * 2f;
			}
		}
	}

	public static void StopBoost(float dt)
	{
		if (Global.boostInstance == null)
		{
			Global.boostInstance = Global.boostSfx.CreateInstance();
			Global.boostInstance.IsLooped = true;
			Global.boostInstance.Volume = 0.6f * Global.SFXVolume;
		}
		if (Global.boostInstance.State == SoundState.Playing)
		{
			if (Global.boostInstance.Volume - dt * 2f < 0f)
			{
				Global.boostInstance.Pause();
			}
			else
			{
				Global.boostInstance.Volume -= dt * 2f;
			}
		}
	}

	public static void TurnOffBoost()
	{
		if (Global.boostInstance == null)
		{
			Global.boostInstance = Global.boostSfx.CreateInstance();
			Global.boostInstance.IsLooped = true;
			Global.boostInstance.Volume = 0.6f * Global.SFXVolume;
		}
		if (Global.boostInstance.State == SoundState.Playing)
		{
			Global.boostInstance.Pause();
		}
	}

	public static void PlayTransition()
	{
		Global.transitionSfx.Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayMenuSelect()
	{
		Global.menuSelectSfx.Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayMenuScroll()
	{
		Global.menuScrollSfx.Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayMenuBack()
	{
		Global.menuBackSfx.Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayMenuStart()
	{
		Global.menuStartSfx.Play(Global.SFXVolume, 0f, 0f);
	}

	public static void PlayMenuYeah()
	{
		Global.menuYeahSfx.Play(Global.SFXVolume, 0f, 0f);
	}

	public static float RandomBetween(float min, float max)
	{
		return min + (float)Global.random_.NextDouble() * (max - min);
	}

	public static int RandomDirection()
	{
		return Global.Random.Next(0, 2) * 2 - 1;
	}

	public static bool CanBuyGame()
	{
		if (Global.PlayerIndex.HasValue)
		{
			//SignedInGamer signedInGamer = Gamer.SignedInGamers[Global.PlayerIndex.Value];
			//if (signedInGamer == null || !signedInGamer.IsSignedInToLive || Guide.IsVisible)
			//{
			//	return false;
			//}
			//return signedInGamer.Privileges.AllowPurchaseContent;
		}
		return false;
	}

	public static void SetVibration(float strength)
	{
		if (Global.vibrationOn)
		{
			Global.vibrationStrength = strength;
			if (Global.PlayerIndex.HasValue)
			{
				GamePad.SetVibration(Global.PlayerIndex.Value, Global.vibrationStrength, Global.vibrationStrength);
			}
			Global.vibrationPaused = false;
		}
	}

	public static void SetVibrationPaused(bool paused)
	{
		if (Global.vibrationOn)
		{
			if (Global.vibrationPaused && !paused && Global.PlayerIndex.HasValue)
			{
				GamePad.SetVibration(Global.PlayerIndex.Value, Global.vibrationStrength, Global.vibrationStrength);
			}
			if (!Global.vibrationPaused && paused && Global.PlayerIndex.HasValue)
			{
				GamePad.SetVibration(Global.PlayerIndex.Value, 0f, 0f);
			}
			Global.vibrationPaused = paused;
		}
	}

	public static void DrawRect(SpriteBatch spriteBatch, Rectangle rect)
	{
		spriteBatch.Draw(Global.obstacleTex, rect, (Rectangle?)new Rectangle(554, 188, 1, 1), Color.White * 0.5f);
	}

	public static void DrawStringCentered(SpriteBatch spriteBatch, string text)
	{
		Vector2 vector = Global.fontTex.MeasureString(text);
		spriteBatch.DrawString(Global.fontTex, text, new Vector2(Global.screenSize.X / 2f - vector.X / 2f - 2f, Global.screenSize.Y / 2f - vector.Y / 2f + 2f), Color.Black);
		spriteBatch.DrawString(Global.fontTex, text, new Vector2(Global.screenSize.X / 2f - vector.X / 2f, Global.screenSize.Y / 2f - vector.Y / 2f), Color.White);
	}

	public static void DrawStringCenteredLarge(SpriteBatch spriteBatch, string text, float scale)
	{
		Vector2 vector = Global.menuFontTex.MeasureString(text);
		spriteBatch.DrawString(Global.menuFontTex, text, new Vector2(Global.screenSize.X / 2f - 2f, Global.screenSize.Y / 2f + 2f), Color.Black, 0f, new Vector2(vector.X / 2f, vector.Y / 2f), scale, SpriteEffects.None, 0f);
		spriteBatch.DrawString(Global.menuFontTex, text, new Vector2(Global.screenSize.X / 2f, Global.screenSize.Y / 2f), Color.White, 0f, new Vector2(vector.X / 2f, vector.Y / 2f), scale, SpriteEffects.None, 0f);
	}
}
