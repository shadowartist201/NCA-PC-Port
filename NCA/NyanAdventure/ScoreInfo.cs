using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Storage;

namespace NyanAdventure;

public struct ScoreInfo
{
	public int[] scores_;

	public int[] characters_;

	public ScoreInfo(int someInt)
	{
		this.scores_ = new int[5];
		for (int i = 0; i < this.scores_.Length; i++)
		{
			this.scores_[i] = 0;
		}
		this.characters_ = new int[5];
		for (int i = 0; i < this.characters_.Length; i++)
		{
			this.characters_[i] = i;
		}
	}

	public void AddScore(OverallMode overallMode, int score, int characterIndex)
	{
		if (score > this.scores_[(int)overallMode])
		{
			this.scores_[(int)overallMode] = score;
			this.characters_[(int)overallMode] = characterIndex;
		}
	}

	public int GetHighScore(OverallMode overallMode)
	{
		return this.scores_[(int)overallMode];
	}

	public static ScoreInfo LoadInfo()
	{
		ScoreInfo result = new ScoreInfo(0);
        /*if (Global.DeviceManager.Device != null && Global.DeviceManager.Device.IsConnected)
		{
			IAsyncResult asyncResult = Global.DeviceManager.Device.BeginOpenContainer("NCA", (AsyncCallback)null, (object)null);
			asyncResult.AsyncWaitHandle.WaitOne();
			//StorageContainer storageContainer = Global.DeviceManager.Device.EndOpenContainer(asyncResult);
			asyncResult.AsyncWaitHandle.Close();
			string text = "ScoreInfo";
			result = new ScoreInfo(0);
            /*if (!storageContainer.FileExists(text))
			{
				result = new ScoreInfo(0);
				storageContainer.Dispose();
			}
			else
			{
				Stream stream = storageContainer.OpenFile(text, FileMode.Open);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ScoreInfo));
				result = (ScoreInfo)xmlSerializer.Deserialize(stream);
				stream.Close();
				storageContainer.Dispose();
			}
        }
		else
		{
			result = new ScoreInfo(0);
		}
		return result;*/
        return result;
    }

	public void SaveInfo()
	{
		/*if (Global.DeviceManager.Device != null && Global.DeviceManager.Device.IsConnected)
		{
			IAsyncResult asyncResult = Global.DeviceManager.Device.BeginOpenContainer("NCA", (AsyncCallback)null, (object)null);
			asyncResult.AsyncWaitHandle.WaitOne();
			//StorageContainer storageContainer = Global.DeviceManager.Device.EndOpenContainer(asyncResult);
			asyncResult.AsyncWaitHandle.Close();
			string text = "ScoreInfo";
			//if (storageContainer.FileExists(text))
			//{
			//	storageContainer.DeleteFile(text);
			//}
			//Stream stream = storageContainer.CreateFile(text);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ScoreInfo));
			//xmlSerializer.Serialize(stream, (object)this);
			//stream.Close();
			//storageContainer.Dispose();
		}*/
	}

	public void DrawLeaderboard(SpriteBatch spriteBatch)
	{
		Color[] array = new Color[6]
		{
			new Color(222, 90, 132),
			new Color(222, 132, 90),
			new Color(222, 181, 90),
			new Color(165, 222, 90),
			new Color(90, 214, 222),
			new Color(132, 132, 230)
		};
		spriteBatch.DrawString(Global.menuFontTex, "Mode", new Vector2(126f, 312f), array[0]);
		spriteBatch.DrawString(Global.menuFontTex, "Character", new Vector2(638f, 312f), array[0], 0f, new Vector2(Global.menuFontTex.MeasureString("Character").X / 2f, 0f), 1f, SpriteEffects.None, 0f);
		spriteBatch.DrawString(Global.menuFontTex, "Score", new Vector2(1022f, 312f), array[0]);
		spriteBatch.DrawString(Global.menuFontTex, "Mode", new Vector2(128f, 310f), Color.White);
		spriteBatch.DrawString(Global.menuFontTex, "Character", new Vector2(640f, 310f), Color.White, 0f, new Vector2(Global.menuFontTex.MeasureString("Character").X / 2f, 0f), 1f, SpriteEffects.None, 0f);
		spriteBatch.DrawString(Global.menuFontTex, "Score", new Vector2(1024f, 310f), Color.White);
		for (int i = 0; i < this.scores_.Length; i++)
		{
			string text = "";
			switch (i)
			{
			case 0:
				text = "Party";
				break;
			case 1:
				text = "Jump";
				break;
			case 2:
				text = "Gravitate";
				break;
			case 3:
				text = "Fly";
				break;
			case 4:
				text = "Toast";
				break;
			}
			spriteBatch.DrawString(Global.menuFontTex, text, new Vector2(126f, 372 + 50 * i), array[i + 1], 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
			spriteBatch.DrawString(Global.menuFontTex, text, new Vector2(128f, 370 + 50 * i), Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Global.characterListTex, new Vector2(640f, 410 + 50 * i), (Rectangle?)new Rectangle(152 * this.characters_[i], 0, 152, 80), Color.White, 0f, new Vector2(76f, 40f), 0.5f, SpriteEffects.None, 0f);
			spriteBatch.DrawString(Global.menuFontTex, this.scores_[i].ToString(), new Vector2(1022f, 372 + 50 * i), array[i + 1], 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
			spriteBatch.DrawString(Global.menuFontTex, this.scores_[i].ToString(), new Vector2(1024f, 370 + 50 * i), Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
		}
	}
}
