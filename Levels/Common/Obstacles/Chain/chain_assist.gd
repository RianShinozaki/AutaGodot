extends Area2D

func _physics_process(delta):
	position = $"../RigidBody2D".position + Vector2.DOWN * 8
	rotation = $"../RigidBody2D".rotation
