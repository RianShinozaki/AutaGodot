class_name SavePoint
extends Sprite2D

func _on_area_2d_area_entered(area):
	Global.save_point_pos = global_position
	GameWorld.instance.save()
