extends Node

var active: bool = false
var triggered: bool = false
@export var killzone: Node2D
@export var rise_speed: float = 25
@export var scare_sound: AudioStream
@onready var alarm = $Alarm
@onready var voice = $Voice
@onready var light = $Panel

func _on_trigger_body_entered(body):
	if triggered: return
	
	if body is Auta:
		triggered = true
		alarm.play()
		voice.play()
		GameCamera.instance.shake_screen(1, 20)
		light.visible = true
		await get_tree().create_timer(0.5).timeout
		active = true
		MusicController.start_music(scare_sound)
		

func _process(delta):
	if active:
		if killzone.position.y > -584.0:
			killzone.global_position.y -= delta*rise_speed
		else:
			active = false
			alarm.stop()
			light.visible = false


func _on_killzone_area_entered(_area):
	active = false
	await Auta.instance.quick_respawned
	killzone.position.y = -8
	triggered = false
	alarm.stop()
	light.visible = false


func _on_trigger2_body_entered(_body):
	active = false
	killzone.position.y = -8
	triggered = false
	alarm.stop()
	light.visible = false
