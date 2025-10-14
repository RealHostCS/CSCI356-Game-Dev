//4 Direction animation
var _totalImages = sprite_get_number(sprite_index)//Total Images in sprite
var _animatingImages = _totalImages/8;//Change 4 to number of directions
var _direction = direction div 45;//change 45 to 90 if 4 sides
var _start = _animatingImages*_direction;//Start of animation
var _end = _start+_animatingImages;//End of animation

image_index = clamp(image_index,_start,_end)//Clamp image index to stop glitching
//Basic animation loop
if image_index = _end
{
image_index = _start
}
