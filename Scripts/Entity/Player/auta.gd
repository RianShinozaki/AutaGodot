class_name Auta

extends CollisionEntity

var can_orb: bool = true
var orb_timer: float = 0.0
var recharge_orb = false
var anim_skating := false
static var instance: Auta

signal orbed

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
