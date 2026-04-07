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

# What has been implemented: (2026-03-25)

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
- Implemented the intital tilemap with Japan themed tile palette.
- Built the first playable map (w/ house, cabins, shops) with proper layer setup. (Ground, WalkBehind, WalkInFront and Decor)
- Added tilemap and player collision using RigidBody mechanic.
- Bulit the Level 1 urban map.

Jesse (JLBeitz):
- Built the complete animation system for the main character, covering movement, running, and all directional idle states.
- Implemented startup logic and standardized object tagging to ensure scripts interact correctly with game entities.
- Set up the essential folder architecture, including prefabs and UI elements, while managing the repository's version control settings.
- Created NPCs with dialogue that explains the story, along with an interaction system for possible future use.
- Fixed bugs in code whenever they came up.
  
Victor (victor-liao93):
- Made score manager, enemy and enemy follow class with inheritance.
- Made inventory slots and inventory script
- Reorganized files

Demir (guveld):
- Added camera to follow the character as it moves.
- Added a sample scene transition to demonstrate level progression.
- Added Finish Point and Scene Controller scripts.
  
Justin (jchiu233): 
- Developed the startup menu with start and exit buttons.
- Developed the tab UI which when opened can look at inventory, settings and map.
- Made potions class.
- Made a new scene where you can choose character.
  


