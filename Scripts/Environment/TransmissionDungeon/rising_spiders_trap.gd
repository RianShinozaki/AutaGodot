extends Node

var active: bool = false
var triggered: bool = false
@export var killzone: Node2D
@export var rise_speed: float = 25

func _on_trigger_body_entered(body):
	if triggered: return
	
	if body is Auta:
		triggered = true
		GameCamera.instance.shake_screen(1, 10)
		await get_tree().create_timer(0.5).timeout
		active = true
		

func _process(delta):
	if active:
		if killzone.position.y > -304:
			killzone.global_position.y -= delta*rise_speed
		else:
			active = false


func _on_killzone_area_entered(area):
	active = false
	await Auta.instance.quick_respawned
	killzone.position.y = -8
	triggered = false
