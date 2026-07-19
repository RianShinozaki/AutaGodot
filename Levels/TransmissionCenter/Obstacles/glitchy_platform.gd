extends AnimatableBody2D

func _on_break_point_area_entered(_area):
	call_deferred("queue_free")
