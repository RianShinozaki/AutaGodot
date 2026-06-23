class_name MusicController extends Node

signal music_started
signal music_stopped

static var instance: MusicController
static var audio: AudioStreamPlayer
static var music_lock: bool

var FADETIME: float = 0.5

# Called when the node enters the scene tree for the first time.
func _ready():
	instance = self
	audio = AudioStreamPlayer.new()
	add_child(audio)

static func start_music(_audio: AudioStream, _volume: float = 1.0):
	instance.start_music_inst(_audio, _volume)

func start_music_inst(_audio: AudioStream, _volume: float):
	if audio.playing:
		instance.stop_music_inst()
	audio.stream = _audio
	audio.volume_linear = _volume
	audio.play()

static func stop_music():
	instance.stop_music_inst()
			
func stop_music_inst():
	if audio.playing:
		audio.stop()
