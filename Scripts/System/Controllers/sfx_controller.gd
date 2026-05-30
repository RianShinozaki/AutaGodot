class_name SFXController extends Node

@export var channels: int = 32
static var instance: SFXController
var playing_streams: Array[AudioStreamPlayer2D]
var stopped_streams: Array[AudioStreamPlayer2D]

func _enter_tree():
	instance = self
	
func _ready():
	for i in range(channels):
		var _audio = AudioStreamPlayer2D.new()
		add_child(_audio)
		stopped_streams.append(_audio)

static func play_sound(_audio: AudioStream, _position: Vector2, _volume: float = 1):
	instance.play_sound_instance(_audio, _position, _volume)
	
func play_sound_instance(_audio: AudioStream, _position: Vector2, _volume: float = 1):
	var _player: AudioStreamPlayer2D = null
	if stopped_streams.size() > 0:
		_player = stopped_streams[0]
		stopped_streams.erase(_player)
	else:
		_player = playing_streams[0]
		_player.stop()
		playing_streams.erase(_player)
	_player.volume_linear = _volume
	_player.global_position = _position
	_player.stream = _audio
	_player.bus = "Entities"
	_player.play()
	playing_streams.append(_player)
	return _player
