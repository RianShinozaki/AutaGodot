extends Area2D

@onready var shape: CollisionShape2D = $CollisionShape2D

@export var barriers: Array[Node2D]
@export var godrays: Array[Node2D]

func _on_body_entered(body):
	shape.set_deferred("disabled", true)
	GameCamera.instance.shake_screen(1, 2)
	for i in range(barriers.size()):
		await get_tree().create_timer(0.25).timeout
		var final_position: Vector2 = barriers[i].global_position - Vector2.RIGHT*128
		get_tree().create_tween().tween_property(barriers[i], "global_position", final_position, 1)
	
	var tween
	for i in range(godrays.size()):
		godrays[i].modulate = Color("ffffff2c")
		tween = get_tree().create_tween().tween_property(godrays[i], "modulate", Color("ffffff00"), 0.2)
	await tween.finished
	for i in range(godrays.size()):
		godrays[i].visible = false
	
func _on_rusty_barrier_4_just_broken():
	for i in range(godrays.size()):
		godrays[i].visible = true
		godrays[i].modulate = Color("ffffff00")
		get_tree().create_tween().tween_property(godrays[i], "modulate", Color("#ffffff2c"), 0.2)
