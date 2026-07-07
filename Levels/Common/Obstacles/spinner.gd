extends AnimatableBody2D

@export var rotate_speed: float

func _physics_process(delta):
	rotate(rotate_speed * delta * rotate_speed)
