class_name GameWorld extends CanvasModulate

@export var do_cutscenes: bool = true

@export var bgm: AudioStream
@export var bgm_volume: float
static var instance: GameWorld

func _ready():
	if bgm:
		MusicController.start_music(bgm, bgm_volume)
	instance = self
