# Isaac Modular 3D Approach

*A 3D twin-stick roguelite shooter, built in Unity, inspired by **The Binding of Isaac**.*

The Binding of Isaac is built around upgrade items that stack and combine in unexpected ways. This project recreates that idea in 3D: every upgrade is implemented as an **independent, modular effect** on the player's cannon, so items can be picked up in any order and freely combine — more projectiles *and* bigger *and* homing *and* sticky, all at once.

---

## 1. The idea

In *The Binding of Isaac* each item tweaks one property of how you shoot (tear count, size, homing, etc.), and the power comes from how those tweaks stack. Because each effect is self-contained, adding a new item is just adding one more independent modifier — they don't need to know about each other.

This repo applies that pattern in a 3D Unity game:

- The player controls a **cannon** that lobs/fires cannonballs.
- Each **pickup item** flips a single flag or bumps a single value on the cannon.
- Effects are orthogonal, so collecting several items composes their behaviours with no special-case code.

---

## 2. Modular upgrade items

Pickups are implemented in [`PickUp.cs`](4451_Game/Assets/Scripts/PickUp.cs). On player contact, an item calls one method on the cannon and shows a pickup message. The item prefabs live in [`Assets/Resources/Spawnables`](4451_Game/Assets/Resources/Spawnables).

| Item | Tag | Effect on the cannon |
|---|---|---|
| 🍎 Apple | `health_apple` | `player.AlterHealth(1)` — heals the player |
| 🥊 Boxing glove | `balls_glove` | `increaseNumberofBalls()` — fires one more projectile per shot (`nn`) |
| 🍄 Mushroom | `balls_mushroom` | `increaseBallSize()` — scales projectiles up (`ss *= 1.1`) |
| 🧴 Glue stick | `sticky_glue` | `toggleStickyMode()` — projectiles stick to whatever they hit |
| 🪖 Magneto's helmet | `homing_magneato_helmet` | `toggleHomingMode()` — projectiles steer toward the enemy |

Because each handler touches one parameter on [`Cannon.cs`](4451_Game/Assets/Cannon.cs), the upgrades stack: pick up the glove twice and the mushroom and the helmet, and you fire three large homing balls.

---

## 3. The cannon: composable shot parameters

[`Cannon.cs`](4451_Game/Assets/Cannon.cs) holds the shot state as a set of independent fields, each toggled by an item (or, during development, a hotkey). Any combination is valid simultaneously:

| Field | Meaning |
|---|---|
| `nn` | number of projectiles fired per shot |
| `ss` | projectile size scale |
| `homo_mode` / `homo_strength` | homing — steers live projectiles toward the target |
| `stick_mode` | projectiles stick to surfaces on impact |
| `bounce_mode` | projectiles bounce instead of being destroyed on hit |
| `shooting_mode` | straight-line shot vs. gravity-arc ballistic shot |
| `turret_mode` | fired projectiles act as turrets that fire their own projectiles |
| `float_ball` | projectiles hang in the air with no gravity |
| `angle` | launch angle for the ballistic arc |
| `damage` | damage per projectile |
| `conti_shoot` | continuous automatic fire |

Ballistic shots are aimed with a projectile-physics solver (`BallisticVelocity`) so they land on the point under the cursor. Projectile behaviour (bounce / stick / homing / turret) is handled per-ball in [`cannon_ball.cs`](4451_Game/Assets/cannon_ball.cs).

### Developer hotkeys

The `Update()` loop in `Cannon.cs` maps keys to these flags for testing without picking items up:

| Key | Action |
|---|---|
| Left click | Fire |
| `G` | Toggle continuous fire |
| `U` | Toggle straight-shot mode |
| `B` | Toggle bounce |
| `J` | Toggle sticky |
| `I` | Toggle turret mode |
| `V` | Toggle floating projectiles |
| `N` / `K` | Fewer projectiles / shrink projectiles |
| `Y` / `H` | Raise / lower launch angle |
| `R` / `F` | Stronger / weaker homing |
| `O` / `P` | More / less damage |

---

## 4. Procedural levels

The dungeon is generated rather than hand-built:

- **BSP room generation** — [`Generate_Rooms.cs`](4451_Game/Assets/Scripts/Level/Generate_Rooms.cs) recursively splits the board into rooms (binary space partitioning) and connects them.
- **Wave Function Collapse** — the [`unity-wave-function-collapse`](4451_Game/Assets/unity-wave-function-collapse) module is used for tile-based generation.
- **Floor / spawn population** — [`floorgeneration.cs`](4451_Game/Assets/floorgeneration.cs), [`floor.cs`](4451_Game/Assets/Scripts/Level/floor.cs), [`PopulatePlane.cs`](4451_Game/Assets/Scripts/Level/PopulatePlane.cs) and [`spawner.cs`](4451_Game/Assets/Scripts/Level/spawner.cs) lay out the level and place contents.
- A standalone 2D **room-and-corridor generator** lives in [`map_generator`](map_generator/Assets/2dmap) (`BoardCreator.cs`, `Room.cs`, `Corridor.cs`).

---

## 5. Enemies and the player

- **Enemies** — [`enemy.cs`](4451_Game/Assets/Scripts/Enemy%20Scripts/enemy.cs) and a multi-state **boss** ([`boss_script.cs`](4451_Game/Assets/Scripts/Enemy%20Scripts/boss_script.cs)) with wander / chase / attack behaviour, spawned via [`enemy_spawn.cs`](4451_Game/Assets/Scripts/Enemy%20Scripts/enemy_spawn.cs). Oil droplets act as a hazard ([`oil_interaction.cs`](4451_Game/Assets/Scripts/Enemy%20Scripts/oil_interaction.cs)).
- **Player** — health system, movement, mouse-look and a third-person camera live under [`Scripts/Player Scripts`](4451_Game/Assets/Scripts/Player%20Scripts) and [`ThirdPersonCamera.cs`](4451_Game/Assets/Scripts/ThirdPersonCamera.cs).

---

## 6. Project layout

```
4451_Game/        Main Unity project (the game)
  Assets/
    Cannon.cs            modular shot parameters
    cannon_ball.cs       per-projectile bounce / stick / homing / turret logic
    PickUp.cs            item pickups → cannon upgrades
    Resources/Spawnables item prefabs (apple, glove, mushroom, glue, helmet…)
    Scripts/             player, enemy, level-generation scripts
    unity-wave-function-collapse/   WFC tile generation
map_generator/    Separate 2D room-and-corridor map generator
```

---

## 7. Running it

This is a Unity project. Open `4451_Game` in the Unity Editor and play a scene (e.g. `Assets/first_cannon.unity`). `map_generator` is a separate Unity project for the 2D dungeon-generation experiments.
