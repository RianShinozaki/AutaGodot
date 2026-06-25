class_name SpriteDupe

extends Sprite2D

@export var target: Sprite2D
@export var active: bool = false
@export var stick_to_cam: bool = false
@export var stick_offset: Vector2

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
		
		if stick_to_cam:
			position = target.global_position - GameCamera.instance.global_position + GameCamera.instance.get_camera_size()/2 + stick_offset
