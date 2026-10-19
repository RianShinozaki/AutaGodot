class_name ChainAnchor extends StaticBody2D

const chain_link = preload("res://Levels/Common/Obstacles/Chain/chain_link.tscn")

var chain_link_instances: Array[ChainLink]
var last_link: ChainLink
@export var length: int = 5

var activation_lock: bool = false

func _ready():
	var _new_link: ChainLink
	var _previous_joint: Node2D = self
	var _pin_joint: PinJoint2D
	if get_parent() is ChainGear:
		length = get_parent().length
		
	for i in range(length):
		_new_link = chain_link.instantiate()
		_new_link.anchor = self
		_new_link.chain_index = i
		_new_link.player_attached.connect(on_player_attached)
		_new_link.player_detached.connect(on_player_detached)
		
		add_child(_new_link)
		_new_link.position = Vector2(0, 16 * i)
		
		_pin_joint = _new_link.get_node("PinJoint2D")
		_pin_joint.node_a = _pin_joint.get_path_to(_previous_joint)
		_pin_joint.node_b = _pin_joint.get_path_to(_new_link.get_node("RigidBody2D"))
		_previous_joint = _new_link.get_node("RigidBody2D")
		
		chain_link_instances.append(_new_link)
	
	last_link = _new_link
	_pin_joint.bias = 0.8

func set_activation_lock(value: bool):
	activation_lock = value
	
func deactivate_links():
	if activation_lock: return
	for link: ChainLink in chain_link_instances:
		link.deactivate()

func activate_links():
	if activation_lock: return
	for link: ChainLink in chain_link_instances:
		link.activate()

func make_last_light():
	last_link.get_node("RigidBody2D").mass = ChainLink.default_weight

func make_last_heavy():
	last_link.get_node("RigidBody2D").mass = get_parent().length/2 * ChainLink.default_weight

func on_player_attached():
	pass
	#deactivate_links()

func on_player_detached():
	deactivate_links()
	await get_tree().create_timer(0.5).timeout
	activate_links()

func get_link(_index: int) -> ChainLink:
	if _index < 0 || _index >= chain_link_instances.size():
		return null
	else:
		return chain_link_instances[_index]
