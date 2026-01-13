# Unity Survival Simulation (WIP)

A survival-style simulation project built in **Unity (C#)**, focused on **gameplay systems and architecture** rather than graphics.

This project serves as a **learning and portfolio project**, developed with an emphasis on clean logic, system separation, and emergent gameplay behaviors.

---

## Key Features & Systems

- **Action system**
  - `IAction` interface with `Start / Tick / Cancel / IsFinished`
  - Action queue with interruption and continuation
- **Character needs**
  - Hunger & starvation affecting movement speed and decision-making
- **World simulation**
  - Grid-based world with terrain and elevation
  - Separation of **World (logic)** and **RenderWorld (visuals)**
- **Pathfinding**
  - BFS-based pathfinding
  - Flood fill for area selection
  - Nearest-resource search
- **Resource & inventory system**
  - Harvesting, collecting, resource piles
  - Weight-based inventory constraints
- **Building system**
  - Area-based construction (stockpiles)
  - Construction progress over time
- **Basic UI**
  - Action bar, context actions, progress indicators

---

## Project Structure (simplified)

- `World` – core simulation logic and game state  
- `CharacterActions` – decision-making, action queue, autonomous behavior  
- `IAction` implementations – movement, harvesting, building, eating  
- `Pathfinder` – pathfinding and spatial queries  
- `RenderWorld` – visual representation and animations  
- `UI` – minimal interface for player feedback

---

## Design Goals

- Focus on **systems-driven gameplay**
- Clear separation of responsibilities
- Readable, extensible code suitable for refactoring and iteration
- Minimal graphics – visuals exist only to support system feedback

---

## Status

**Work in progress.**  
Systems are actively developed and refactored as part of continuous learning.

---

## Tech Stack

- Unity
- C#
- Git / GitHub

---

## Author

**Lukas Drewnik**  
Unity / C# developer with a background in visual arts  

- GitHub: https://github.com/Woodwanderer
- LinkedIn: https://www.linkedin.com/in/lukas-drewnik
