shader_type canvas_item;

void fragment() {
    vec4 color = texture(TEXTURE, UV);
    
    float scanline = sin(FRAGCOORD.y * 2.0) * 0.1;
    COLOR = vec4(color.rgb - scanline, color.a);
}