shader_type canvas_item;

uniform float thickness;
uniform float strength;
uniform vec2 sprite_size;
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_linear;

void fragment() {
    vec2 uv = UV - vec2(0.5);
    float dist = length(uv);
    
    float ring = smoothstep(0.5 - thickness, 0.5, dist) * 
                 smoothstep(0.5, 0.5 - thickness * 0.5, dist);
    
    vec2 normal = normalize(uv);
    vec2 distorted_uv = SCREEN_UV + normal * ring * strength * (sprite_size / vec2(textureSize(screen_texture, 0)));
    
    vec4 screen = texture(screen_texture, distorted_uv);
    COLOR = vec4(screen.rgb, ring);
}