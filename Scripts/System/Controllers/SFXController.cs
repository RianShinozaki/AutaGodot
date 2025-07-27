using Godot;
using System;

[GlobalClass]
public partial class SFXController : Node
{
	[Export] public int numChannels;
	public static SFXController sfx;
    public override void _Ready()
    {
		sfx = this;
		for(int i = 0; i < numChannels; i++) {
			AudioStreamPlayer2D player = new AudioStreamPlayer2D();
			AddChild(player);
		}
    }

	//Call this with a loaded sound to play it
	public static AudioStreamPlayer2D PlaySound(AudioStream sound, Vector2 position, float dbLinear = 1f, float pitchScale = 1f) {
		foreach(var node in sfx.GetChildren()) {
			AudioStreamPlayer2D audio = node as AudioStreamPlayer2D;
			if(audio.Playing) {
				if (audio.Stream == sound)
				{
					audio.Stop();
				}
			}
		}
		foreach (var node in sfx.GetChildren())
		{
			AudioStreamPlayer2D audio = node as AudioStreamPlayer2D;
			if (!audio.Playing)
			{
				audio.Stream = sound;
				audio.Play();
				audio.VolumeDb = Mathf.LinearToDb(dbLinear);
				audio.PitchScale = pitchScale;
				audio.GlobalPosition = position;
				return audio;
			}
		}
		AudioStreamPlayer2D oldestAudio = sfx.GetChild<AudioStreamPlayer2D>(0);
		float longestTimePlayed = -1;
		foreach(var node in sfx.GetChildren()) {
			AudioStreamPlayer2D audio = node as AudioStreamPlayer2D;
			float thisTime = audio.GetPlaybackPosition();
			if(thisTime > longestTimePlayed) {
				longestTimePlayed = thisTime;
				oldestAudio = audio;
			}
		}
		oldestAudio.Stop();
		oldestAudio.Stream = sound;
		oldestAudio.Play();
		oldestAudio.GlobalPosition = position;
		oldestAudio.VolumeDb = Mathf.LinearToDb(dbLinear);
		oldestAudio.PitchScale = pitchScale;
		return oldestAudio;
	}
}