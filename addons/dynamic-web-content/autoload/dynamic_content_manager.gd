extends Node
# autoload DynamicContentManager
# manages event for requesting a content refresh, also houses some information relevant for the dynamic content such as whether they are allowed to refresh on load or not


signal request_content_refresh

var auto_refresh_on_ready := false
var cache_enabled := false

const CACHE_PATH := "user://dyn-content-cache/"
const STG_USE_CACHE_NAME = "DynamicContentLoading/cache/use_cache"
const STG_AUTO_REFRESH = "DynamicContentLoading/cache/content_auto_refresh"

func _ready() -> void:
	DirAccess.make_dir_recursive_absolute(CACHE_PATH)
	auto_refresh_on_ready = ProjectSettings.get_setting(STG_AUTO_REFRESH, false) as bool
	cache_enabled = ProjectSettings.get_setting(STG_USE_CACHE_NAME, false) as bool

# Creates an event that will cause all currently active dynamic content to refresh their information and 
func refresh_dynamic_content() -> void:
	request_content_refresh.emit()

func clear_cache_data() -> void:
	push_warning("All cache data about to be cleared!")
	OS.move_to_trash(ProjectSettings.globalize_path(CACHE_PATH))
