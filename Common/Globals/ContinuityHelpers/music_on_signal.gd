class_name MusicOnSignal
extends Node

#What is my purpose

#You wait for a signal and then play music

#Oh my god

@export var music: AudioStream
@export var volume: float
func trigger():
	MusicController.start_music(music, volume)
