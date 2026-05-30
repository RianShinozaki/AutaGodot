extends CutsceneSequence

@export var activate_on_ready: bool = false

var auta: Auta

func _ready():
	if activate_on_ready:
		auta = Auta.instance
		auta.get_node("GenericAttributes/InputManager").read_controller_input = false
		begin()

func begin():
	PlayerUI.instance.set_ui_visibility(false)
	super.begin()
	
func end():
	super.end()
	auta = Auta.instance
	auta.get_node("GenericAttributes/InputManager").read_controller_input = true
	PlayerUI.instance.set_ui_visibility(true)
