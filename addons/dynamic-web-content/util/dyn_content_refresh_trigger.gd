extends Node

func _ready() -> void:
	get_parent().ready.connect(_trigger)

func _trigger() -> void:
	DynamicContentManager.refresh_dynamic_content()
	await(RenderingServer.frame_post_draw)
	queue_free()
