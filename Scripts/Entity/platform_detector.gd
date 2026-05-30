class_name PlatformDetector
extends Node2D
var platform: PhysicsBody2D = null
var prev_transform: Transform2D
var local_point: Vector2  # player's position in platform-local space

func update_from_collisions(body: CharacterBody2D) -> void:
	platform = null
	if body.is_on_floor():
		for i in body.get_slide_collision_count():
			var col = body.get_slide_collision(i)
			if col.get_normal().dot(body.up_direction) > cos(body.floor_max_angle):
				var collider = col.get_collider()
				if collider is PhysicsBody2D:
					platform = collider
					break
	
	if platform:
		prev_transform = platform.global_transform
		local_point = platform.global_transform.affine_inverse() * body.global_position

func apply(body: CharacterBody2D) -> Vector2:
	if not platform or not is_instance_valid(platform):
		platform = null
		return Vector2.ZERO
	
	var _current_xf = platform.global_transform
	var _new_global = _current_xf * local_point
	var _displacement = _new_global - body.global_position
	
	body.global_position = _new_global
	return _displacement
