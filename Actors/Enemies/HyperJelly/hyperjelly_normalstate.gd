extends ActionState

var inp: InputManager
var anim: AnimationTree


@export var auto_flip: bool = true


func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	
	anim = entity.get_node("Art/AnimationTree")
	
	inp.action_b_just_pressed.connect(on_attack)

func _start() -> void:
	super._start()
	
func _process(delta: float) -> void:
	super._process(delta)
	if not active: return

func _end() -> void:
	super._end()
	

func play_animation_oneshot(_anim: String):
	anim.get("parameters/Grounded/playback").start(_anim, true)

func on_attack():
	if not active: return
	entity.attack()
