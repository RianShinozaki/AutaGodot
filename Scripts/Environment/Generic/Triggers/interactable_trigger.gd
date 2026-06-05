extends Area2D

var entered: bool = false
var active: bool = true
signal trigger

func _on_body_entered(body):
	entered = true


func _on_body_exited(body):
	entered = false

func _input(event):
	if active and entered and event.is_action_pressed("Interact"):
		emit_signal("trigger")
		active = false
		await get_parent().cutscene_finished
		active = true
