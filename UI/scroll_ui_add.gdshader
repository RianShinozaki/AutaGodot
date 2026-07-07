shader_type canvas_item;
render_mode blend_add;
uniform float scroll_speed_x = 0.1; // Adjust scrolling speed
uniform float scroll_speed_y = 0.1; // Adjust scrolling speed

void vertex() {
    // Offset UV coordinates based on time and speed
    UV.x += TIME * scroll_speed_x;
    // For vertical scrolling, use UV.y instead
    UV.y += TIME * scroll_speed_y;
}