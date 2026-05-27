extends CutsceneSequence

var auta: Auta

func _ready():
	auta = Auta.instance
	auta.get_node("GenericAttributes/InputManager").read_controller_input = false
	begin()

func end():
	super.end()
	auta = Auta.instance
	auta.get_node("GenericAttributes/InputManager").read_controller_input = true
