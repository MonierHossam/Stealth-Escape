Game Scenario: "Stealth Escape"  
Task: Develop a basic stealth 3D game where the player must navigate through a guarded area without being detected.  

Gameplay:  
  
The player controls a character that must reach an exit while avoiding patro lling enemies.  
If an enemy sees the player (using a vision cone), they will chase them.  
If the player hides behind an object, the enemy loses sight and returns to patrolling.  
The player wins by reaching the exit undetected.  
  
Requirements:  
  
Implement NavMesh-based enemy patrols with waypoints.  
Use Raycasting or trigger detection for enemy vision.  
Implement a Finite State Machine (FSM) for enemy AI with Patrol, Chase, and Return states.  
Add a UI alert when the player is spotted.  
  
