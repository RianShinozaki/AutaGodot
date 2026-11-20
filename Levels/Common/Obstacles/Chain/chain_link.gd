class_name ChainLink
extends Node2D

var anchor: ChainAnchor
var active: bool = true
signal player_attached
signal player_detached
var chain_index: int = 0

@onready var hot_sprite = $RigidBody2D/Hot
@onready var player_grabber = $PlayerGrabber/CollisionShape2D
@onready var hitbox = $RigidBody2D/Hitbox/CollisionShape2D
var hot: bool = false
const default_weight = 1

var spider_on_chain = preload("res://Actors/Enemies/SpiderDrone/spider_on_chain.tscn")

func _on_player_grabber_body_entered(body):
	if not active: return
	
	if body is Auta:
		if body.current_action_state.name == "NormalState" || body.current_action_state.name == "OrbState":
			Auta.instance.get_action_state_name("ChainClimbState").grabbed_chain = self
			Auta.instance.switch_action_state_name("ChainClimbState")
			get_node("RigidBody2D").linear_velocity += Auta.instance.velocity
			emit_signal("player_attached")
	if body is SpiderDrone:
		if anchor.object_attached == null:
			print(anchor.object_attached)
			var _new_spider = spider_on_chain.instantiate()
			anchor.object_attached = _new_spider
			body.queue_free()
			_new_spider.attach_to_chain(self)
	
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

func become_hot():
	hot = true
	player_grabber.set_deferred("disabled", true)
	hitbox.set_deferred("disabled", false)
	get_tree().create_tween().tween_property(hot_sprite, "modulate", Color(1, 1, 1, 1), 0.5)

func become_cool():
	hot = false
	player_grabber.set_deferred("disabled", false)
	hitbox.set_deferred("disabled", true)
	get_tree().create_tween().tween_property(hot_sprite, "modulate", Color(1, 1, 1, 0), 0.5)
