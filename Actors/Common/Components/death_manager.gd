extends Node2D

@onready var status: EntityStatus = $"../../GenericAttributes/EntityStatus"
@onready var entity: CollisionEntity = $"../.."
@onready var pool: ObjectPool = $"../ObjectPools/DeathExplosion"

func _ready():
	$"../HurtManager".hurt.connect(query_death)
	
func query_death():
	if status.health <= 0:
		if status.stunned or not status.can_be_stunned:
			var _burst = pool.spawn_object()
			pool.remove_child(_burst)
			entity.get_parent().add_child(_burst)
			_burst.global_position = entity.global_position
			entity.queue_free()
		elif not status.stunned and status.can_be_stunned:
			entity.switch_action_state_name("StunState")
			if entity is AIEntity:
				entity.current_ai_state.active = false
		
