extends Node2D

@onready var collider = $Collider
@onready var trigger = $Collider/GravityEffectorArea

@export var rotate_speed: float

func _physics_process(delta):
	collider.rotate(rotate_speed * delta * rotate_speed)
	trigger.set_gravity_direction = -Vector2.from_angle(collider.rotation-PI/2)
