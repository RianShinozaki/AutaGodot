extends Breakable

# Called when the node enters the scene tree for the first time.
func _ready():
	var _sm: ShaderMaterial = material as ShaderMaterial
	var _viewport = get_tree().get_nodes_in_group("Reflection")[0]
	_sm.set_shader_parameter("viewport_texture", _viewport.get_texture())
	super._ready()
	
