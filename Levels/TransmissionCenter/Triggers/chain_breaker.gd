extends Area2D

@export var chain_anchor: ChainAnchor
var fired: bool = false

func _on_body_entered(body):
	if fired: return
	if body is Auta:
		var _auta: Auta = body as Auta
		if _auta.current_action_state.name == "ChainClimbState":
			chain_anchor.get_link(4).get_node("PinJoint2D").node_a = NodePath()
			chain_anchor.get_link(4).get_node("RigidBody2D").apply_impulse(Vector2.RIGHT)
			chain_anchor.deactivate_links()
			chain_anchor.set_activation_lock(true)
			fired = true
			_auta.switch_action_state_name("NormalState")
