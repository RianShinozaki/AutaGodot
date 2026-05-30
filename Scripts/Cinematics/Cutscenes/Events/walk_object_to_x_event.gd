class_name CS_WalkObjectToX extends CutsceneEvent

@export var target: NodePath
@export var x_position: float
@export var speed_scale: float = 1

func trigger(_caller):
	async = true
	var target_node: Node = _caller.get_node(target)
	target_node.call("walk_to_x",x_position, speed_scale)
	await target_node.reached_x_position
	emit_signal("finished")
