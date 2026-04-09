# CountdownToMidnight

Note: The oldCommit branch contains the full commit history prior to March 21, 2026. Earlier work including core class design, tile map prototypes, and initial animations can be found there.

# What is our game?

Countdown to Midnight is a top-down post-apocalyptic RPG game that is set in the fictional Soviet Republic of Verstokia. Following a nuclear strike, rival factions have torn the country apart and the player must survive progessively difficult levels to reach the Global Accord Safe zone.

There are two factions: 
Rioters: Those who escaped from ruthless prisons with pistols who patrol open areas are spread chaos.
Buran Army: Nationalist zealots made up of former Spatnez and Red Army soldiers carrying assault rifles.

Core Game Progession: 
- Move through each level, dealing with enemies and other challenges
- Earn points for completing levels and defeating enemies
- Players will be able to visit the black market shop to spend points
- Players unlock new weapons as they aim to complete the 5 levels.

# Level Design:

Level 1 - Stealth (Hammad): Avoid enemies as they patrol the routes, and reach the end of the map.

Level 2 - Knife Attack (Victor): Eliminate all enemies with knife or reach the end zone of the level.

Level 3 - Sniper Defense (Jesse): Survive a 2 minute horde siege with a sniper.

Level 4 - Maze (Demir): Navigate through a maze with patrolling guards

Level 5 - Boss (Justin): Defeat the final boss using all weapons acquired.

# What has been implemented: (2026-03-25) --> Updated (2026-04-08)

Tilemap-based world with multiple layers, collision and a navigable environment

Player movement that is smooth and animated.

Camera Tracking

Majority of Scripts are near production level with OOP principles and modular set in mind.

UI/Menu is a work in progress but shows new interactivity by allowing a name to be written and a character 
select screen.


# Member Contributions Overview
Reminder: Please check the commit histories on both oldCommit and main for the full story.

Hammad (HDH-bytes): 
- Developed the Character base class and IDamageable interface. (Uses OOP, such as abstraction)
- Implemented Japan tilemap and player collision using RigidBody mechanic.
- Bulit the Level 1 urban map.
- Completed Level1, developed the enemy AI including search path, vision cone, and fixed animation bug
- Bulit the Level0 map, with urban and nature mix
- Developed the Shopkeeper, including interactiblity, UI menu and backend to increase speed.
- Developed the ScoreManager, that manages the players XP as they pass levels or kill enemies, bundled with ranks and UI
- Fixed FinishPoint indexing bug, where Levels would not continue.
- Fixed Character Prefabs
- Refactored code to improve readabilty and maintainabilty

Jesse (JLBeitz):
- Built the complete animation system for the main character, covering movement, running, and all directional idle states.
- Implemented startup logic and standardized object tagging to ensure scripts interact correctly with game entities.
- Set up the essential folder architecture, including prefabs and UI elements, while managing the repository's version control settings.
- Created NPCs with dialogue that explains the story, along with an interaction system for possible future use.
- Fixed bugs in code whenever they came up.
- Made all of the weapons the way they currently function
- Made the enemy spawner which spawns them in at regular intervals
- Made level 3 which implements the 2 points above into a challenging new level
- Fixed the eskimo classes to use inheritance so that they have health and can be damaged
- Created a script that can make objects activate and deactivate depending on certain conditions
- Created a weapon switcher that can utilize both the number pad and scroll wheel to change weapons
- Made weapons unlock upon reaching certain levels to balance the player power
  
Victor (victor-liao93):
- Made eskimo classes like attack and follow
- Made inventory slots and inventory script
- Reorganized files
- Some changes to win scene
- Made level 2 map
- Made eskimo animations

Demir (guveld):
- Added camera to follow the character as it moves.
- Added a sample scene transition to demonstrate level progression.
- Added Finish Point and Scene Controller scripts.
- Added Level 4 Map
- Added a functional health bar
- Added functional extra heart option to the shopkeeper
- Implemented a clock on Level 4 as a lose condition
- Added retry option for levels
- Added start new game option for when the game is lost
- Helped with testing
  
Justin (jchiu233): 
- Developed the startup menu with start and exit buttons.
- Developed the tab UI which when opened can look at inventory, settings and map.
- Made potions class.
- Made a new scene where you can choose character.
  


