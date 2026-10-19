class_name ChainLink
extends Node2D

var anchor: ChainAnchor
var active: bool = true
signal player_attached
signal player_detached
var chain_index: int = 0

const default_weight = 1

func _on_player_grabber_body_entered(body):
	if not active: return
	
	if body is Auta:
		if body.current_action_state.name == "NormalState" || body.current_action_state.name == "OrbState":
			Auta.instance.get_action_state_name("ChainClimbState").grabbed_chain = self
			Auta.instance.switch_action_state_name("ChainClimbState")
			get_node("RigidBody2D").linear_velocity += Auta.instance.velocity
			emit_signal("player_attached")
	
func deactivate():
	active = false
	$PlayerGrabber/CollisionShape2D.set_deferred("disabled", true)
	
func activate():
	active = true
	$PlayerGrabber/CollisionShape2D.set_deferred("disabled", false)

func vector_to_anchor() -> Vector2:
	return anchor.global_position - global_position
	
func player_just_detached():
	emit_signal("player_detached")

func get_higher_chain() -> ChainLink:
	var _return: ChainLink = anchor.get_link(chain_index - 1)
	if _return == null:
		return self
	else:
		return _return
	
func get_lower_chain() -> ChainLink:
	var _return: ChainLink = anchor.get_link(chain_index + 1)
	if _return == null:
		return self
	else:
		return _return
