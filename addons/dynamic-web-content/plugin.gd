@tool
extends EditorPlugin

const STG_WEB_ROOT_NAME = "DynamicContentLoading/links/webpage_root"
const STG_GAME_ID_NAME = "DynamicContentLoading/links/game_id"
const STG_USE_CACHE_NAME = "DynamicContentLoading/cache/use_cache"
const STG_AUTO_REFRESH = "DynamicContentLoading/cache/content_auto_refresh"

func _enter_tree() -> void:
	# ensure project settings exist
	var webpage_root := _get_setting_safe(STG_WEB_ROOT_NAME, "default-web-path") as String
	if webpage_root == "default-web-path":
		push_error("Be sure to assign your 'webpage_root' in the Project Settings!")
	_get_setting_safe(STG_GAME_ID_NAME, "default-game-id")
	_get_setting_safe(STG_USE_CACHE_NAME, true) as bool
	_get_setting_safe(STG_AUTO_REFRESH, false) as bool
	
	# add autoload
	add_autoload_singleton("DynamicContentManager", "res://addons/dynamic-web-content/autoload/dynamic_content_manager.gd")
	
	# add types
	var dyn_img_button := load("res://addons/dynamic-web-content/elements/dynamic_image_button.gd")
	add_custom_type("DynamicImageButton", "PanelContainer", dyn_img_button, null)
	var dyn_refresh_trigger := load("res://addons/dynamic-web-content/util/dyn_content_refresh_trigger.gd")
	add_custom_type("DynamicContentRefreshTrigger", "Node", dyn_refresh_trigger, null)
	
	add_tool_menu_item("Clear Dynamic Content Cache",_clear_cache)

func _exit_tree() -> void:
	remove_autoload_singleton("DynamicContentManager")
	remove_custom_type("DynamicImageButton")
	remove_custom_type("DynamicContentRefreshTrigger")
	remove_tool_menu_item("Clear Dynamic Content Cache")

func _get_setting_safe(var_name : String, default_value : Variant) -> Variant:
	if ProjectSettings.has_setting(var_name):
		return ProjectSettings.get_setting(var_name, default_value)
	ProjectSettings.set_setting(var_name, default_value)
	return default_value

const CACHE_PATH := "user://dyn-content-cache/"
func _clear_cache() -> void:
	push_warning("All cache data about to be cleared!")
	OS.move_to_trash(ProjectSettings.globalize_path(CACHE_PATH))
