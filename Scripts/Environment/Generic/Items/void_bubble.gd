class_name VoidBubble extends CharacterBody2D
@export_range(0, 1) var bounce_coef: float = 1.0
@export var initial_velocity: float = 150

@onready var anim_player: AnimationPlayer = $AnimationPlayer
@export var bubble_bump = -150
@export var pop_sound: AudioStream

@export var obj_spawn: PackedScene
var to_spawn: int = 5

func _physics_process(delta):
	var _collision = move_and_collide(velocity * delta)
	if _collision:
		velocity = velocity.bounce(_collision.get_normal()) * bounce_coef
	velocity -= velocity * 3 * delta
			
func _on_area_2d_area_entered(area):
	if not area is Hitbox:
		Auta.instance.velocity.y = bubble_bump
		explode()
	else:
		if to_spawn == 1:
			explode()
		else:
			anim_player.call_deferred("play", "Pop")
			await anim_player.animation_finished
			var _star_inst = obj_spawn.instantiate()
			get_parent().add_child(_star_inst)
			_star_inst.global_position = global_position
			var _dir = Vector2.from_angle(randf_range(0, 2*PI))
			_star_inst.velocity = _dir * 150
			to_spawn -= 1
	
func explode():
	anim_player.call_deferred("play", "Pop")
	await anim_player.animation_finished
	SFXController.play_sound(pop_sound, global_position)
	
	var _angle = deg_to_rad(360.0 / to_spawn)
	
	for i in range(to_spawn):
		var _star_inst = obj_spawn.instantiate()
		get_parent().add_child(_star_inst)
		_star_inst.global_position = global_position
		var _dir = Vector2.from_angle(_angle * i)
		_star_inst.velocity = _dir * 150
		
	queue_free()

func _on_area_2d_body_entered(_body):
	if _body is Auta:
		var vec_to_player: Vector2 = (Auta.instance.global_position-global_position).normalized()
		Auta.instance.velocity = -vec_to_player*bubble_bump
