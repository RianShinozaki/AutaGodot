extends CharacterBody2D

@export_range(0, 1) var bounce_coef: float = 1.0
@export var initial_velocity: float = 150
@export var lifetime = 5
@export var gravity: float = 400

func _ready():
	await get_tree().create_timer(lifetime).timeout
	call_deferred("queue_free")

func _physics_process(delta):
	var _collision = move_and_collide(velocity * delta)
	if _collision:
		velocity = velocity.bounce(_collision.get_normal()) * bounce_coef
	velocity += Vector2.DOWN * gravity * delta
