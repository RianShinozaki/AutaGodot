class_name ObjectPoolObject

extends Node

var object_pool: ObjectPool
var index: int

func spawn():
	get_parent().OnSpawn()

func despawn():
	object_pool.return_object_to_pool(self)
