extends Node

@onready var status: EntityStatus = $"../../GenericAttributes/EntityStatus"
@onready var entity: CollisionEntity = $"../.."

var game_over_scene = preload("res://UI/game_over.tscn")
var death_lock: bool = false
var anim: AnimationTree

func _ready():
	$"../HurtManager".hurt.connect(query_death)
	anim = entity.get_node("Art/AnimationTree")
	
func query_death(_hitbox: Hitbox):
	if status.health <= 0:
		var _len = entity.pre_move_velocity.length()/60
		var _dir = entity.pre_move_velocity.normalized()
		
		entity.inflict_hitstun(_len, _dir, 0.4)
		
		entity.velocity.x = -sign(entity.pre_move_velocity.x) * 200
		entity.velocity.y = -180
		
		anim.get("parameters/playback").start("Defeat", true)
		entity.get_node("Art/AfterImageGenerator").call("start_afterimages")
	
		death()
		
func death():
	if death_lock: return
	death_lock = true
	$"../PlayerHurtbox/CollisionShape2D".disabled = true
	
	var _cam: GameCamera = GameCamera.instance
	_cam.ignore_boundaries = true
	_cam.snap_to_target()
	_cam.lerp_zoom(10, 1)
	Engine.time_scale = 0.5
	var _tween = get_tree().create_tween().tween_property(Engine, "time_scale", 0.1, 0.6)
	await _tween.finished
	Engine.time_scale = 1
	GameSystem.instance.change_game_world(game_over_scene)
