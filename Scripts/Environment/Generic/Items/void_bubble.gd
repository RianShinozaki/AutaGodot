class_name VoidBubble extends AnimatedSprite2D
@onready var anim_player: AnimationPlayer = $AnimationPlayer
@export var bubble_bump = -150
@export var pop_sound: AudioStream
@export var obj_void_star: PackedScene
var stars: int = 5

func _on_area_2d_area_entered(area):
	if not area is Hitbox:
		Auta.instance.velocity.y = bubble_bump
		explode()
	else:
		if stars == 1:
			explode()
		else:
			anim_player.call_deferred("play", "Pop")
			await anim_player.animation_finished
			var _star_inst = obj_void_star.instantiate()
			get_parent().add_child(_star_inst)
			_star_inst.global_position = global_position
			_star_inst.launch(randf_range(0, 2*PI))
			stars -= 1
	
func explode():
	anim_player.call_deferred("play", "Pop")
	await anim_player.animation_finished
	SFXController.play_sound(pop_sound, global_position)
	
	var _angle = deg_to_rad(360 / stars)
	
	for i in range(stars):
		var _star_inst = obj_void_star.instantiate()
		get_parent().add_child(_star_inst)
		_star_inst.global_position = global_position
		_star_inst.launch(_angle * i)
		
	queue_free()


func _on_area_2d_body_entered(body):
	var vec_to_player: Vector2 = (Auta.instance.global_position-global_position).normalized()
	Auta.instance.velocity = -vec_to_player*bubble_bump
