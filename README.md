# BombermanTheGame

This game is a modern remake of the Bomberman series. It is a local multiplayer game where two characters can compete on the same keyboard. Players navigate a stage with indestructable walls and destructible bricks. Bombs can be placed on the field to destroy bricks, and sometimes power-ups appear when bricks are destroyed. Players must also be careful not to be caught in explosions from their own or their opponent's bombs. The objective is to eliminate the other player to win.

**Player Movement**: Players move using either WASD keys or the arrow keys. Movement is restricted green playable areas; solid walls and destructible bricks cannot be crossed. 

**Collision Detection**: Players can place bombs that explode after a short delay, destroying nearby destructible bricks. Bombs have a blast radius that can hit both players and bricks. Initially, players can move away from a placed bomb, but afterward they collide with it. Bombs can also roll slightly when colliding with players.

**Life System**: Each player starts with 3 lives. Being caught in a bomb blast causes the player to lose one life. Losing all three lives triggers Game Over.

**Multiplayer**: The game supports two-player local multiplayer, allowing people to compete directly on the same screen.

**Game States**: Players can start the game or exit from a Start menu, offering a simple interface.

**Powerups**: The game includes three types of power-ups that enhance gameplay. Extra Bomb increases the number of bombs a player can place. Blast Radius expands the bomb's blast area, making it easier to hit the opponent or destroy bricks. Speed Boost increases the player's movement speed for faster navigation around the stage.

**Animation**: The game features multiple animation effects to enhance visual feedback. Firstly, player sprites change based on movement direction, simulating running. Second, players blink when hit by a bomb blast to indicate damage. Third, bombs pulsate to signal their countdown before detonation. Finally, when a player loses all lives, a death animation plays before the game ends. 



https://github.com/user-attachments/assets/22888754-ecfd-46f0-b351-0e1ad7cb68e5

