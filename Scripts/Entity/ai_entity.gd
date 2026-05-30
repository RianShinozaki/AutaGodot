class_name AIEntity

extends CollisionEntity

@export_group("States")
@export var current_ai_state: AIState
@export var last_ai_state: AIState

@export_group("Parameters")
@export var ai_parameters: Dictionary[String, AIParameterSet]

@export var shove_entities: Array[Node2D]

var aggro_target: Node2D

func _process(_delta: float) -> void:
	for _entity in shove_entities:
		var dir: int = -1 if _entity.global_position > global_position else 1
		move(Vector2.RIGHT * 40 * dir, _delta, false)
	
func switch_ai_state_name(_state: String) -> AIState:
	var _ai_state = get_ai_state_name(_state)
	return switch_ai_state(_ai_state)

func get_ai_state_name(_state: String) -> AIState:
	return get_node("AIStates/"+_state)

func switch_ai_state(_state: AIState) -> AIState:
	last_ai_state = current_ai_state
	current_ai_state = _state
	last_ai_state._end()
	current_ai_state._start()
	return current_ai_state

func _on_shove_box_area_entered(area: Area2D) -> void:
	if not area in shove_entities: shove_entities.append(area)

func _on_shove_box_area_exited(area: Area2D) -> void:
	if area in shove_entities: shove_entities.erase(area)
