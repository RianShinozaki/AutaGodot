class_name PlayerUI

extends CanvasLayer

@onready var health_bar = $PlayUI/HPBar/HealthBar
@onready var enemy_health = $PlayUI/EnemyUI/EnemyHP
@onready var big_message = $PlayUI/BigMessage
@onready var pause_menu = $PauseMenu
@onready var play_ui = $PlayUI

var auta: Auta
var enemy_target: CollisionEntity
var enemy_ui_tween: Tween
var uncapture_enemy_time_max: float = 5
var uncapture_enemy_time: float = 0

static var instance: PlayerUI

var menu_active: bool = false

func _ready():
	instance = self
	pause_menu.visible = false

func _process(_delta: float) -> void:
	if auta == null: auta = Auta.instance
	if !is_instance_valid(auta): 
		visible = false
		return
	
	if get_tree().paused: return
	
	var _entstat: EntityStatus = auta.get_node("GenericAttributes/EntityStatus")
	health_bar.value = _entstat.health / _entstat.max_health
	
	if enemy_target != null and is_instance_valid(enemy_target):
		_entstat = enemy_target.get_node("GenericAttributes/EntityStatus")
		$PlayUI/EnemyUI/EnemyHP/HealthBar.value = _entstat.health / _entstat.max_health
		uncapture_enemy_time += _delta
		if uncapture_enemy_time > uncapture_enemy_time_max:
			enemy_target = null
	else:
		$PlayUI/EnemyUI.visible = false

func captured_enemy(_enemy_target: CollisionEntity):
	if enemy_target == _enemy_target: return
	$PlayUI/EnemyUI.visible = true
	$PlayUI/EnemyUI.position.y = -95.0
	$PlayUI/EnemyUI/EnemyTab/EnemyName.text = _enemy_target.name
	enemy_target = _enemy_target
	if enemy_ui_tween:
		enemy_ui_tween.kill()
	enemy_ui_tween = create_tween()
	enemy_ui_tween.tween_property($PlayUI/EnemyUI, "position", Vector2(24, 15), 0.2)
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
	
func _unhandled_input(event: InputEvent) -> void:
	if event.is_action_pressed("Menu"):
		toggle_menu()
		
func toggle_menu():
	menu_active = not menu_active
	get_tree().paused = menu_active
	pause_menu.visible = menu_active
	menu_active = menu_active
	play_ui.visible = not menu_active
	
