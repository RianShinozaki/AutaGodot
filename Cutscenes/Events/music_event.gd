class_name CS_MusicEvent extends CutsceneEvent

@export var music: AudioStream
@export var volume: float = 0
@export var instant: bool = false

func trigger(_caller: CutsceneSequence):
	if music:
		MusicController.start_music(music, volume, instant)
	else:
		MusicController.stop_music(instant)
