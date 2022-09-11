using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace NyanAdventure;

internal class SongManager
{
	private float volume_ = 1f;

	private SoundEffect[] baseSongs = new SoundEffect[4];

	private SoundEffectInstance[] songs = new SoundEffectInstance[4];

	private ContentManager Content;

	private int currentSongIndex_ = 0;

	private int lastSongIndex_ = 0;

	private float songTimer_;

	public float Volume
	{
		set
		{
			this.volume_ = value;
			this.songs[this.currentSongIndex_].Volume = this.volume_;
		}
	}

	public SongManager(Game1 game)
	{
		this.Content = new ContentManager((IServiceProvider)game.Services, "Content\\Sound\\Music");
		this.baseSongs[0] = this.Content.Load<SoundEffect>("Main Menu");
		this.baseSongs[1] = this.Content.Load<SoundEffect>("All Your Base");
		this.baseSongs[2] = this.Content.Load<SoundEffect>("Nyan Remix");
		this.baseSongs[3] = this.Content.Load<SoundEffect>("Techno Mix");
		for (int i = 0; i < 4; i++)
		{
			this.songs[i] = this.baseSongs[i].CreateInstance();
			this.songs[i].IsLooped = true;
		}
	}

	public void Update(float dt)
	{
		if (this.songs[this.currentSongIndex_].State == SoundState.Paused)
		{
			return;
		}
		this.songTimer_ += dt;
		if (this.currentSongIndex_ != this.lastSongIndex_)
		{
			if (this.songs[this.lastSongIndex_].Volume - 0.3f * dt <= 0f)
			{
				this.songs[this.lastSongIndex_].Stop();
				this.lastSongIndex_ = this.currentSongIndex_;
			}
			else
			{
				this.songs[this.lastSongIndex_].Volume -= 0.3f * dt;
			}
		}
		if (this.volume_ > 0.5f && this.songs[this.currentSongIndex_].Volume != 1f)
		{
			if (this.songs[this.currentSongIndex_].Volume + 0.3f * dt >= 1f)
			{
				this.songs[this.currentSongIndex_].Volume = 1f;
			}
			else
			{
				this.songs[this.currentSongIndex_].Volume += 0.3f * dt;
			}
		}
	}

	public void Pause()
	{
		if (this.currentSongIndex_ != this.lastSongIndex_)
		{
			this.songs[this.lastSongIndex_].Pause();
		}
		this.songs[this.currentSongIndex_].Pause();
	}

	public void Resume()
	{
		if (this.currentSongIndex_ != this.lastSongIndex_)
		{
			this.songs[this.lastSongIndex_].Resume();
		}
		this.songs[this.currentSongIndex_].Resume();
	}

	public void SetSlowmo()
	{
		if (this.songs[1].Pitch > -0.3f)
		{
			for (int i = 1; i < this.songs.Length; i++)
			{
				this.songs[i].Pitch = -0.3125f;
			}
		}
	}

	public void SetSpeedup()
	{
		if (this.songs[1].Pitch < 0.4f)
		{
			for (int i = 1; i < this.songs.Length; i++)
			{
				this.songs[i].Pitch = 0.5f;
			}
		}
	}

	public void SetEndSlowmo()
	{
		if (this.songs[1].Pitch < 0f)
		{
			for (int i = 1; i < this.songs.Length; i++)
			{
				this.songs[i].Pitch = 0f;
			}
		}
	}

	public void SetEndSpeedup()
	{
		if (this.songs[1].Pitch > 0f)
		{
			for (int i = 1; i < this.songs.Length; i++)
			{
				this.songs[i].Pitch = 0f;
			}
		}
	}

	public void SetNormal()
	{
		for (int i = 1; i < this.songs.Length; i++)
		{
			this.songs[i].Pitch = 0f;
		}
	}

	private void PlayNewSong(int index)
	{
		if (this.currentSongIndex_ != this.lastSongIndex_ && this.songs[this.lastSongIndex_].State == SoundState.Playing)
		{
			this.songs[this.lastSongIndex_].Stop();
		}
		if (this.songs[this.currentSongIndex_].State != SoundState.Stopped)
		{
			this.songs[this.currentSongIndex_].Stop();
		}
		this.currentSongIndex_ = index;
		this.lastSongIndex_ = index;
		this.songs[this.currentSongIndex_].Play();
		this.songs[this.currentSongIndex_].Volume = this.volume_;
		this.songTimer_ = 0f;
	}

	private void JoinNewSong(int index)
	{
		if (this.currentSongIndex_ != index)
		{
			if (this.currentSongIndex_ != this.lastSongIndex_ && this.songs[this.lastSongIndex_].State == SoundState.Playing)
			{
				this.songs[this.lastSongIndex_].Stop();
			}
			if (this.songs[this.currentSongIndex_].State == SoundState.Playing)
			{
				this.lastSongIndex_ = this.currentSongIndex_;
			}
			else
			{
				this.lastSongIndex_ = index;
			}
			this.currentSongIndex_ = index;
			this.songs[this.currentSongIndex_].Play();
			this.songs[this.currentSongIndex_].Volume = 0f;
			this.songTimer_ = 0f;
		}
	}

	public void BeginMenu()
	{
		this.PlayNewSong(0);
	}

	public void SetMenu()
	{
		if (this.currentSongIndex_ != 0)
		{
			this.PlayNewSong(0);
		}
	}

	public void SetGame()
	{
		if (this.currentSongIndex_ == 0)
		{
			this.PlayNewSong(Global.Random.Next(1, 4));
		}
	}

	public void ContinueGame()
	{
		if (this.songTimer_ > 39f)
		{
			if (this.currentSongIndex_ > 0)
			{
				this.JoinNewSong(1 + (this.currentSongIndex_ - 1 + Global.Random.Next(1, 3)) % 3);
			}
			else
			{
				this.JoinNewSong(Global.Random.Next(1, 4));
			}
		}
	}
}
