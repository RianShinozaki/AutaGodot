extends Area2D

@export var message: String

func _on_body_entered(body):
	PlayerUI.play_big_message(message)
	queue_free()
