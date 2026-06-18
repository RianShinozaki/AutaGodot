class_name CS_MusicEvent extends CutsceneEvent

@export var music: AudioStream
@export var volume: float = 0

func trigger(_caller: CutsceneSequence):
	if music:
		MusicController.start_music(music, volume)
	else:
		MusicController.stop_music()
