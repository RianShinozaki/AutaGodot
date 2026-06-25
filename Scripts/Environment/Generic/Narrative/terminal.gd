extends AnimatedSprite2D

@export var dialogue_resource: DialogueResource
@export var fail_id: String
@export var start_id: String

@export var powered: bool = false

var entered: bool = false
var active: bool = true
signal trigger
var auta: Auta

func _ready():
	get_node("Area2D").body_entered.connect(_on_body_entered)
	get_node("Area2D").body_exited.connect(_on_body_exited)
	
func _on_body_entered(_body):
	entered = true


func _on_body_exited(_body):
	entered = false

func _input(event):
	if active and entered and event.is_action_pressed("Interact"):
		auta = Auta.instance
		if auta.is_on_floor() and auta.get_action_state_name("NormalState"):
			active = false
			GameCamera.instance.lerp_zoom(6)
			emit_signal("trigger")
			PlayerUI.instance.set_ui_visibility(false)
			auta.unset_all()
			auta.get_node("GenericAttributes/InputManager").read_controller_input = false
		
			DialogueManager.show_dialogue_balloon(dialogue_resource, start_id if powered else fail_id)
			await DialogueManager.dialogue_ended
		
			PlayerUI.instance.set_ui_visibility(true)
			auta.get_node("GenericAttributes/InputManager").read_controller_input = true
			active = true
			GameCamera.instance.lerp_zoom(4)
