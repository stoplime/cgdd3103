# Project 2
### by: Steffen Lim

## How To Play
'WSAD' to move around.  
'ESC' to open the controls menu.  
'Space' to jump and double jump.  
Collect items to your inventory.  
The little companion can help you against the Orange cubes.  
There are three healing items and two types of bullets.  

## Project Requirements
1. Using a script to create an inventory storage with minimum 6 different items, maximum 12 items. When there are less or equal than 6 items collected, the inventory menu will only show 6 items in the storage, even some of them are empty. When more than 6 but less or equal than 12 different items collected, the inventory menu will show the exact number of different items collected in the storage. If the 13th different item is collecting, show a message said the storage is full, cannot take any more. In the storage, you need to show the name and amount of the items collected. (The same item will be stack in the same spot and just increase the amount of the item) If all of one collected item is used, you should delete that item from the inventory storage, and shift the rest of the items to fill the empty spot. (20%)
2. The inventory storage menu should be controlled by the user to show and close the screen. (5%)
3. Using a script to create a customizable quick item storage with no less than 4 items. Users can choose any 4 different items stored in the inventory storage put them to one of the quick item storage. Users should be able to define and change the order of the quick item storage. You also need to show the item name and amount in the quick item storage. The items in the quick item storage should be able to access (use) by pressing 1, 2, 3, 4 keys on the keyboard AND by a single mouse click on the menu. The quick item storage menu should stay on the screen all the time. Make after accessing (using) on the quick item, you need to update the amount of items in both quick item storage and inventory storage. If all of one quick item is used, you should delete that item from the quick item storage, and show empty on that spot. You also need to delete that item from the inventory storage, see above requirements. (20%)
4. You need to have at least 3 different health increasing items that can increase the health value, such as increase 10, 20, or 30, and 3 different weapon items, such as bullets with a color of red, green, or blue. When access/uses the health increasing items, you should update your health value. When access/use the weapon items, you should be able to shoot different bullets. (15%)
5. Create at least 5 waypoints for AI characters and 12 different obstacles on the floor and BAKE the navigation mash. (5%)
6. Create at least 2 AI enemies. One of them must be idle when it is more than 10 units from your character. And if it is less or equal than 10 units from your character, it will start chasing you. If it is less then 5 units from your character, it will SHOOT a bullet to you. The other enemy must guard along the 5 waypoints when it is more than 10 units from your character. If it is less than 2 units from you it will attack your character by PUNCHING. (20%)
7. Create at least 1 AI friends. It will always follow your character and help you attacking (shooting or punching) the enemies when the enemies are closer than 5 units from YOUR character. (15%)
8. Extra Credit: If you can only save the name of the weapon items you collected instead of saving the gameObject, and instantiate one when you need to use the weapon items by the name only, you will get 10% extra credit of this project. (10%)


## Project Implementation

### 1. Inventory System
The inventory system has a dynamic set of slots where it first shows a set of 6 items. The maximum number of slots can grow as the player gains more items, up to a total max of 12 items. The inventory will only show extra items as the player collects more items. It will only show empty slots when the player has less than 6 items. Once the player's inventory is full, the player can no longer collect any items and a message will be displayed. The items can be stacked up to 64 items in a slot. Once 64 items are stacked a new stack will take place. If a new stack cannot be added, then the player will only take as much as it can (filling up existing stacks). Slots are emptied once a stack of items are used.

### 2. Inventory Menu
The menu system can be opened using the key 'E' or it can be customized in the controls menu by pressing 'ESC'. It also has an intuitive drag and drops feature where the player can organize the inventory.

### 3. Hotbar System
The Hotbar acts as windows to the actual inventory. They are not extra inventory slots, but rather represent a slot in the main inventory. In the menu, the hotbar representation of the items can be rearranged using the drag and drop system. While in game, the hotbar is slightly lower and out of the way where the player can then select an item on the hotbar by pressing 1, 2, 3, 4 numbers above the keyboard representing the location of the hotbar slot respectively. A single mouse click on the item did not fit with the game since the player's camera movement would not allow the player to move their mouse onto the hotbar. All actions done by the hotbar are done onto the main inventory items since the hotbar is simply an alias to the inventory (i.e. using an item on the hotbar will use the item in the inventory).

### 4. 6 Useable Items
There are three health items: the stretched epi-pen which heals for 10, the red health pack which heals for 50 and the white health pack which increases the max health by 10. There are also three different types of bullets although they have not been drawn correctly, they are the icons of a white can which represent the yellow bullets dealing 10 damage, the icon of a grey can which represent the red bullets dealing 20 damage, and the green can which represent the blue bullets dealing 50 damage.

### 5. Waypoints and AI navigation
There are many waypoints, which are represented as the white squares on the ground, all around the map. The Enemies are designed to be able to patrol through these waypoints randomly. As for the navigation map, the map has a second level that is walkable and allows the enemies to be able to traverse up a ramp on the side to reach the roof of the building. It does not have 12 obstacles but it does have a traversable roof.

### 6. Enemy AI Behaviour
There are 8 enemy AIs that can patrol, stand idle, attack, and flee from the player. The normal behaviors are patrol and Idle. There is a random chance the enemy will decide to stay idle for 5 seconds once they reach a waypoint. The waypoints are chosen randomly. Once the player is within a certain detection range, the AI will begin to attack the player by melee. Enemies are not able to shoot bullets at this current build. If the enemy is down to a quarter health, they can make a choice of continuing to attack the player or flee. By fleeing, the enemy will choose a random waypoint that does not have a line of sight by the player.

### 7. Friendly AI
The companion AI follows the player around the map and attacks any enemies within range of the character. In the current build, they attempt to attack but it won't inflict any damage due to a collision bug.

### 8. ID-Based Inventory System
Each item in the inventory is represented as an ID rather than a GameObject. The usage of an item will cause the actual item to be spawned such as a bullet.