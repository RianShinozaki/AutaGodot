extends Area2D
@export var music: AudioStream


func _on_body_entered(body):
	MusicController.start_music(music, 0.6)
