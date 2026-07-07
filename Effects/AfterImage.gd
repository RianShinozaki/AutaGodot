class_name AfterImage

extends Sprite2D

@onready var object_pool_object: ObjectPoolObject = $ObjectPoolObject

var shader_material: ShaderMaterial
var time: float = 0.01
var active: bool = false

func _ready() -> void:
	shader_material = material as ShaderMaterial
	visible = false
	
func OnSpawn() -> void:
	active = true
	time = 0.01
	visible = true

func _process(delta: float) -> void:
	if !active: return
	shader_material.set_shader_parameter("t", time)
	time += delta * 2
	if time >= 1:
		visible = false
		object_pool_object.despawn()
		active = false
