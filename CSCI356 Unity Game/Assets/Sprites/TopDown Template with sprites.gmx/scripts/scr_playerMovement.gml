//Get movement
var movex = check_right-check_left;//Calculate sign horizontal direction
var movey = check_down-check_up;//Calculate sign vertical direction
var last_direction = direction;//Save Last direction

hsp = movespeed*movex;//Change HSP
vsp = movespeed*movey;//Change VSP

hsp = clamp(hsp,-movespeed+abs(movey/2),movespeed-abs(movey)/2)//Limit Hsp to remain at same speed when moving diagonally
vsp = clamp(vsp,-movespeed+abs(movex),movespeed-abs(movex))//Limit Vsp to remain at same speed when moving diagonally

//Change Direction
if movex=1 and movey = -1
{
direction = 45
last_direction = direction
}else
if movex=-1 and movey = -1
{
direction = 135
last_direction = direction
}else
if movex=-1 and movey = 1
{
direction = 225
last_direction = direction
}else
if movex=1 and movey = -1
{
direction = 315
last_direction = direction
}else
if movex = 1
{
direction = 0
last_direction = direction
}else
if movey = -1
{
direction = 90
last_direction = direction
}else
if movex = -1
{
direction = 180
last_direction = direction
}else
if movey = 1
{
direction = 270
last_direction = direction
}else
{
direction = last_direction
}

scr_collide()

//Animate
if hsp!=0 or vsp!=0
{
sprite_index = spr_player_run
}else
{
sprite_index = spr_player_idle
}



x+=hsp;//Change X cordinate based on hsp
y+=vsp;//Change Y cordinate based on vsp
