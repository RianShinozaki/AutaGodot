class_name EntityMovementParameters extends ParameterSet

@export var max_speed: float
@export var initial_speed: float
@export var initial_speed_in_air: float
@export var acceleration: float
@export var acceleration_in_air: float
@export var deceleration: float
@export var deceleration_in_air: float
@export var deceleration_above_speed: float
@export var deceleration_above_speed_in_air: float
@export var collision_shape: Shape2D
@export var collision_shape_position: Vector2

func get_max_speed() -> float:
	return max_speed
	
func get_acceleration(_entity: CollisionEntity) -> float:
	if abs(_entity.velocity.x) > max_speed:
		return deceleration_above_speed if _entity.is_on_floor() else deceleration_above_speed_in_air
	else: return acceleration if _entity.is_on_floor() else acceleration_in_air
	
func get_deceleration(_entity: CollisionEntity) -> float:
	return deceleration if _entity.is_on_floor() else deceleration_in_air

func get_deceleration_above_speed(_entity: CollisionEntity) -> float:
	return deceleration_above_speed if _entity.is_on_floor() else deceleration_above_speed_in_air
