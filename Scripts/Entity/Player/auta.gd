class_name Auta

extends CollisionEntity

@export var unlocked_orb = false

var can_orb: bool = true
var orb_timer: float = 0.0
var recharge_orb = false
var anim_skating := false
var quick_respawn_pos: Vector2
static var instance: Auta

signal orbed
signal quick_respawned

func _ready() -> void:
	GameCamera.instance.targets.append(self)
	instance = self
	super._ready()

func _physics_process(delta: float) -> void:
	super._physics_process(delta)
	modulate = Color.WHITE
	if not can_orb and not recharge_orb and is_on_floor() and collision_mode == COLLISION_MODE_FLOOR:
		recharge_orb = true
	
	if not can_orb and recharge_orb:
		orb_timer = move_toward(orb_timer, 0.0, delta)
		if ceili(orb_timer * 10) % 2 == 1:
			modulate = Color.CYAN
		if orb_timer == 0:
			can_orb = true

func emit_orb_signal():
	emit_signal("orbed")

func on_impact(_hitbox_data: HitboxData, _hurtbox: Hurtbox):
	var _knockback_total: Vector2 = Vector2(_hitbox_data.x_knockback, _hitbox_data.y_knockback)
	var _len = _knockback_total.length() / 60
	var _dir = _knockback_total.normalized()
	inflict_hitstun(_len, _dir, _hitbox_data.hitstun_duration, 0)
	if not is_on_floor() and velocity.y > -30:
		velocity.y = -30

func process_damage(_area):
	$SpecialAttributes/HurtManager.process_hurt(_area)

func set_quick_respawn():
	quick_respawn_pos = global_position
	
func quick_respawn():
	await get_tree().create_timer(0.5).timeout
	global_position = quick_respawn_pos
	GameCamera.instance.snap_to_target()
	emit_signal("quick_respawned")

func unlock_orb():
	unlocked_orb = true
	
func unset_all():
	switch_action_state_name("NormalState")
	velocity = Vector2.ZERO
	get_node("GenericAttributes/InputManager").set_input_direction(Vector2.ZERO)
