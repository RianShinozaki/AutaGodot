class_name PlayerUI

extends CanvasLayer

@onready var health_bar = $HPBar/HealthBar
@onready var enemy_health = $EnemyUI/EnemyHP
@onready var big_message = $BigMessage

var auta: Auta
var enemy_target: CollisionEntity
var enemy_ui_tween: Tween
var uncapture_enemy_time_max: float = 5
var uncapture_enemy_time: float = 0

static var instance: PlayerUI

func _ready():
	instance = self

func _process(_delta: float) -> void:
	if auta == null: auta = Auta.instance
	var _entstat: EntityStatus = auta.get_node("GenericAttributes/EntityStatus")
	health_bar.value = _entstat.health / _entstat.max_health
	
	if enemy_target != null and is_instance_valid(enemy_target):
		_entstat = enemy_target.get_node("GenericAttributes/EntityStatus")
		$EnemyUI/EnemyHP/HealthBar.value = _entstat.health / _entstat.max_health
		uncapture_enemy_time += _delta
		if uncapture_enemy_time > uncapture_enemy_time_max:
			enemy_target = null
	else:
		$EnemyUI.visible = false

func captured_enemy(_enemy_target: CollisionEntity):
	if enemy_target == _enemy_target: return
	$EnemyUI.visible = true
	$EnemyUI.global_position.y = -95.0
	$EnemyUI/EnemyTab/EnemyName.text = _enemy_target.name
	enemy_target = _enemy_target
	if enemy_ui_tween:
		enemy_ui_tween.kill()
	enemy_ui_tween = create_tween()
	enemy_ui_tween.tween_property($EnemyUI, "position", Vector2(0, 0), 0.2)
	uncapture_enemy_time = 0

static func play_big_message(_message: String):
	instance.play_big_message_instance(_message)
	
func play_big_message_instance(_message: String):
	var _TEXT_DISPLAY_DELAY: float = 5/60.0
	var _FADE_SPEED: float = 0.5/60.0
	
	big_message.text = _message
	big_message.visible_ratio = 0
	big_message.visible = true
	big_message.modulate.a = 1.0
	while(big_message.visible_characters < big_message.text.length()):
		big_message.visible_characters += 1
		await get_tree().create_timer(_TEXT_DISPLAY_DELAY).timeout
	await get_tree().create_timer(3).timeout
	while(big_message.modulate.a > 0):
		big_message.modulate.a -= _FADE_SPEED
		await get_tree().process_frame
	big_message.visible = false

func set_ui_visibility(_visible: bool):
	visible = _visible
