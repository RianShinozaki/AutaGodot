extends AnimationPlayer

@export var next_scene: PackedScene

	
func _on_animation_finished(_anim_name):
	GameSystem.instance.change_game_world(next_scene)
