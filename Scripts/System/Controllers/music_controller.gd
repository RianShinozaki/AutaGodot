class_name MusicController extends Node

signal music_started
signal music_stopped

static var instance: MusicController
static var audio: AudioStreamPlayer
static var music_lock: bool

var FADETIME: float = 0.5
var target_vol: float = 1.0
var vol_change_speed: float = 1

var magic_int: int = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	instance = self
	audio = AudioStreamPlayer.new()
	add_child(audio)

func _physics_process(delta):
	print(audio.volume_linear)
	if audio.volume_linear != target_vol:
		audio.volume_linear = move_toward(audio.volume_linear, target_vol, vol_change_speed * delta)
		if audio.volume_linear == target_vol and target_vol == 0:
			audio.stop()
			emit_signal("music_stopped")
	
static func start_music(_audio: AudioStream, _volume: float = 1.0, _instant: bool = false):
	instance.start_music_inst(_audio, _volume, _instant)

func start_music_inst(_audio: AudioStream, _volume: float, _instant: bool):
	magic_int += 1
	var _my_int = magic_int
	
	if audio.stream == _audio and audio.playing: return
	
	if audio.playing:
		instance.stop_music_inst(_instant)
		if not _instant:
			await music_stopped
	
	if _audio == null: return
	
	if _my_int == magic_int:
		audio.stream = _audio
		audio.volume_linear = _volume if _instant else 0.0
		target_vol = _volume
	
	audio.play()

static func stop_music(_instant: bool = false):
	instance.stop_music_inst(_instant)
			
func stop_music_inst(_instant: bool):
	if audio.playing:
		if _instant:
			audio.stop()
		else:
			target_vol = 0.0
