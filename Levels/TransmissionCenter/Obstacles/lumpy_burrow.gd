class_name LumpyBurrow
extends Sprite2D

var lumpy_object: PackedScene = preload("res://Actors/Enemies/Lumpy/Lumpy.tscn")
var lumpy_instance: Lumpy = null

@export var active: bool = true

var spawning_in_progress: bool = false

func _process(delta):
	if active:
		if lumpy_instance == null or not is_instance_valid(lumpy_instance):
			if spawning_in_progress: return
			spawning_in_progress = true
			await get_tree().create_timer(1).timeout
			
			lumpy_instance = lumpy_object.instantiate()
			add_child(lumpy_instance)
			lumpy_instance.position = Vector2.ONE*-1000
			
			await get_tree().process_frame
			lumpy_instance.do_burrow_animation()
			lumpy_instance.position = Vector2.ZERO
			await lumpy_instance.finished_burrowing
			if GameSystem.game_state != GameSystem.GameState.PLAY:
				lumpy_instance.process_mode = Node.PROCESS_MODE_DISABLED
				
			spawning_in_progress = false
