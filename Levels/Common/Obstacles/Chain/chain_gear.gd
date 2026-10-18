extends AnimatableBody2D

@export var rollup_on_start: bool = true
@export var debug_controls: bool = false
@export var roll_up_speed: float = 5
@export var length: int = 5

@onready var gear_collider = $CollisionShape2D
@onready var chain_anchor: ChainAnchor = $Anchor

enum state {
	ROLLED,
	UNROLLED,
	ROLLING_UP,
	UNROLLING,
}

var my_state: state = state.UNROLLED
var needed_roll_amount: float
var roll_amount: float
var roll_speed: float

var shake_amount = 0

func _ready():
	if rollup_on_start:
		roll_up()
	needed_roll_amount = (length)*16 / (2.0 * PI * 2.0)

func _unhandled_input(event: InputEvent) -> void:
	if debug_controls and event is InputEventKey and event.pressed and not event.echo:
		if event.keycode == KEY_K:
			match my_state:
				state.UNROLLED:
					roll_up()
				state.ROLLED:
					unroll()

func _physics_process(delta):
	if(shake_amount > 0):
		$ChainGear.offset = Vector2(randf_range(-shake_amount, shake_amount), randf_range(-shake_amount, shake_amount))
		shake_amount = move_toward(shake_amount, 0, 3*delta)
		if shake_amount == 0:
			$ChainGear.offset = Vector2.ZERO
			
	match my_state:
		state.ROLLING_UP:
			var _roll: float = roll_up_speed * delta
			rotate(_roll)
			roll_amount += _roll
			if roll_amount >= needed_roll_amount:
				my_state = state.ROLLED
				shake_amount = 2
		state.UNROLLING:
			var _roll: float = roll_up_speed * delta * 4
			rotate(-_roll)
			roll_amount += _roll
			if roll_amount >= needed_roll_amount*2:
				rotation = rotate_toward(rotation, 0, _roll)
				if rotation == 0:
					my_state = state.UNROLLED
					gear_collider.set_deferred("disabled", false)
					shake_amount = 2
					chain_anchor.activate_links()
					chain_anchor.make_last_light()

func roll_up():
	if not my_state == state.UNROLLED: return
	my_state = state.ROLLING_UP
	roll_amount = 0
	chain_anchor.deactivate_links()
	chain_anchor.make_last_heavy()

func unroll():
	if not my_state == state.ROLLED: return
	my_state = state.UNROLLING
	roll_amount = 0
	roll_speed = 10
	gear_collider.set_deferred("disabled", true)


func _on_hitbox_area_entered(area):
	unroll()
