extends Control
@export_file("*.tscn") var main_menu_file

func _ready() -> void:
	get_tree().paused = true
	Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
	
func _unhandled_input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"):
		_on_btn_continue_pressed()
		get_viewport().set_input_as_handled()

func _on_btn_continue_pressed() -> void:
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	get_tree().paused = false
	queue_free()

func _on_btn_main_menu_pressed() -> void:
	get_tree().paused = false
	get_tree().change_scene_to_file(main_menu_file)
