# Project 1
### by: Steffen Lim

## How To Play
'WSAD' to move around.  
Left mouse button to shoot.  
'ESC' to open the controls menu.  
Orange cubes are the enemy and you must kill them all.  

## Project Requirements
1. Using only the Main Camera to create a first person walking environment. Do not import Unity FPS package. You have to write your own script to control the Main Camera. Make sure you can use "WSAD" to walk in the gallery forward, back, left, right, use the mouse to change the walking direction and looking around. (10%)
2. Create another controller's profile that is using "TGFH" to walk in the gallery forward, back, left, right, use "Left" and "Right" arrow keys to change the walking direction and looking around.(10%)
3. Create a 2D GUI for your health and set it to 100 by default. (10%)
4. Create 2D GUI to switch between two controllers' profile. (10%)
5. Create 2D GUI to enable customizing every single input key. (10%)
6. Create a Cube in the environment as an enemy. When you hit the cube, you will lose 5 of your health, and show it from the 2D health GUI. (10%)
7. If your health is less than 100, your health will be automatically restored by 2 if you did not hit the cube more than 10 seconds. (Restored by 2 every 10 seconds if there is no hit) If you hit the cube within 10 seconds, the timer has to be reset. (10%)
8. Create a projectile script to use left mouse button to shoot one sphere randomly selected from two different sizes and colors to your front in the environment. And kill the sphere after 5 seconds or hit the enemy. The bigger size sphere will cause 20 damage to the enemy, and the smaller size sphere will cause 10 damage to the enemy. (20%)
9. Create a 3D GUI for the enemy's health. When the enemy was hit by the sphere, the enemy loses the corresponding health. If the enemy's health is less or equal to 0, it will be destroyed (disappear from the environment) (10%)


## Project Implementation

### 1. First Person Camera
Using a character Cube, the main camera follows the position and yaw of the character. The Camera then has a separate pitch controller script that allows the mouse to control the vertical movement of the camera.
Controls for movement of the Character can be defaulted to control with "WSAD" which changes both the main character and the camera.

### 2. Separate Control Profile
A control profile system is implemented using a togglable profile buttons in the control menu. This allows the player to toggle between two separate control schemes. Although the requirements specify that the Left and Right arrows be allowed to control the Yaw control of the character, I did not like the disoriented feeling for both acting mouse and keyboard control of the system.

### 3. GUI for Character Health
A display screen on the top left of the game displays the current health and maximum health of the character. The maximum health and starting health is set at default to 100.

### 4. Control Menu
The control menu allows the player to switch between the two control profiles.

### 5. Customizable Control
Each displayed button in the control menu can change keys. If a button is pressed, the next key pressed will be the assigned key for the control.

### 6. Enemy Cube
The enemy cube has its own health system which is displayed above the enemy. If the Player were to walk into the enemy, the player will lose 5 points of health.

### 7. Player Health Regen
The player has a Regen ability to gain two points of health every ten seconds. If the player takes damage, the player's regen timer will reset to the original ten seconds.

### 8. Player Projectiles
The player can shoot two types of projectiles at random. The smaller projectile will deal 10 points of damage onto the Enemy if it hits. The larger projectile will deal 20 points of damage onto the Enemy if it hits. Both projectiles will despawn if the projectile did not collide with an Enemy within 5 seconds.

### 9. Enemy takes Damage
The Enemy displays the damage it took by decreasing its health bar. If the enemy health is less than or equal to zero than the enemy will die.