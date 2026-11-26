extends Node

var wandering_transmission: AudioStream = preload("res://Music/WanderingTransmission.mp3")
var rhythmic: AudioStream = preload("res://Music/Temp/The Cyber Grind.mp3")

func _ready():
	if Global.get("tess_cutscene_3") and not Global.get("do_scanner_return_cutscene") and not Global.get("orb_tutorial_cutscene"):
		MusicController.start_music(wandering_transmission, 0.7)
	
	if Global.get("orb_tutorial_cutscene"):
		MusicController.start_music(rhythmic, 0.7)
