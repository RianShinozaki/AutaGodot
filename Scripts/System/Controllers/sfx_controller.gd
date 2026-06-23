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

static func play_sound(_audio: AudioStream, _position: Vector2, _volume: float = 1, _pitch: float = 1):
	instance.play_sound_instance(_audio, _position, _volume, _pitch)
	
func play_sound_instance(_audio: AudioStream, _position: Vector2, _volume: float = 1, _pitch: float = 1):
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
	_player.pitch_scale = _pitch
	_player.bus = "Entities"
	_player.play()
	playing_streams.append(_player)
	return _player

static func stop_sound_if_playing(_audio: AudioStream) -> bool:
	return instance.stop_sound_if_playing_inst(_audio)
	
func stop_sound_if_playing_inst(_audio: AudioStream) -> bool:
	var playing: AudioStreamPlayer2D = is_playing_inst(_audio)
	if playing:
		playing.stop()
		return true
	return false
	
static func is_playing(_audio: AudioStream) -> AudioStreamPlayer2D:
	return instance.is_playing_inst(_audio)
	
func is_playing_inst(_audio: AudioStream) -> AudioStreamPlayer2D:
	for i in range(playing_streams.size()):
		if playing_streams[i].stream == _audio and playing_streams[i].playing:
			return playing_streams[i]
	return null
