extends Node2D

@onready var status: EntityStatus = $"../../GenericAttributes/EntityStatus"
@onready var entity: CollisionEntity = $"../.."
@onready var pool: ObjectPool = $"../ObjectPools/DeathExplosion"
func _process(_delta: float) -> void:
	if status.health <= 0:
		var _burst = pool.spawn_object()
		pool.remove_child(_burst)
		entity.get_parent().add_child(_burst)
		_burst.global_position = entity.global_position
		entity.queue_free()
		
