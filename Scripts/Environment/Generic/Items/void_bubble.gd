class_name VoidBubble extends AnimatedSprite2D
@onready var anim_player: AnimationPlayer = $AnimationPlayer
@export var bubble_bump = -150
@export var pop_sound: AudioStream

func _on_area_2d_area_entered(area):
	if not area is Hitbox:
		Auta.instance.velocity.y = bubble_bump
	explode()
	
func explode():
	anim_player.play("Pop")
	await anim_player.animation_finished
	SFXController.play_sound(pop_sound, global_position)
	queue_free()


func _on_area_2d_body_entered(body):
	var vec_to_player: Vector2 = (Auta.instance.global_position-global_position).normalized()
	Auta.instance.velocity = -vec_to_player*bubble_bump
