class_name SpriteDupe

extends Sprite2D

var target: Sprite2D
var active: bool = false

func _physics_process(_delta: float) -> void:
	if not target == null and active:
		texture = target.texture
		hframes = target.hframes
		vframes = target.vframes
		frame = target.frame
		flip_h = target.flip_h
		flip_v = target.flip_v
		offset = target.offset
		rotation = target.rotation
