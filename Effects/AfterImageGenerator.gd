class_name AfterImageGenerator

extends ObjectPool

@onready var sprite: Sprite2D = get_parent()
@export var time_between_after_images: float
var active: bool
var after_image_timer: float

func _process(delta: float) -> void:
	if not active: return
	after_image_timer += delta
	if after_image_timer >= time_between_after_images:
		var _spawn: AfterImage = spawn_object() as AfterImage
		if _spawn != null:
			_spawn.global_position = sprite.global_position
			_spawn.texture = sprite.texture
			_spawn.hframes = sprite.hframes
			_spawn.vframes = sprite.vframes
			_spawn.frame = sprite.frame
			_spawn.flip_h = sprite.flip_h
			_spawn.flip_v = sprite.flip_v
			_spawn.offset = sprite.offset
			_spawn.rotation = sprite.rotation
			var _material: ShaderMaterial = _spawn.material as ShaderMaterial
			if sprite.texture is CanvasTexture:
				_material.set_shader_parameter("Texture", sprite.texture.diffuse_texture)
			else: _material.set_shader_parameter("Texture", sprite.texture)
		after_image_timer = 0

func start_afterimages() -> void:
	after_image_timer = 0
	active = true
	
func stop_afterimages() -> void:
	active = false
