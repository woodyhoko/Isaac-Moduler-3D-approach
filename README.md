# Isaac Modular 3D Approach

*Composable 3D simulation environments for robotics in NVIDIA Isaac Sim / Omniverse.*

A research and experimentation repository exploring **modular scene composition** for robot learning — building reusable, parametric 3D environment components in NVIDIA's Omniverse platform that can be assembled into diverse training scenarios without hand-authoring each one from scratch.

---

## 1. Motivation

Training deep reinforcement learning (RL) policies for robotics requires large numbers of diverse simulation environments. Hand-crafting each environment is labor-intensive and yields brittle, non-generalizing policies (Tobin et al. 2017 — domain randomization). A **modular** approach instead defines a library of parametric building blocks — rooms, shelves, objects, lighting rigs — that can be procedurally assembled and randomized, dramatically expanding the training distribution without proportional authoring effort.

NVIDIA Isaac Sim, built on the Omniverse platform, provides the ideal substrate:

- **USD (Universal Scene Description)** — the de facto standard for composable, reference-based 3D scene graphs (Pixar; Apple visionOS; NVIDIA Omniverse)
- **Python API** — full scripting of scene assembly, randomization, sensor placement, and physics
- **PhysX 5** — GPU-accelerated rigid/articulated body dynamics
- **RTX rendering** — photorealistic images for sim-to-real transfer

---

## 2. USD and modularity

Every component is authored as a **USD Prim** (primitive), grouped under `Xform` nodes that can be referenced (`references` arc) into a stage. Because USD is reference-based (not embed-based), a single asset file is shared across all scenes that use it — changing the asset updates every scene simultaneously.

Key USD concepts used:

| Concept | Role |
|---|---|
| `UsdGeom.Xform` | Transform node — position, rotation, scale |
| `UsdGeom.Mesh` | Renderable geometry |
| References | Non-destructive asset reuse across scenes |
| Variants | Switch between asset configurations (e.g. shelf A vs. B) at load time |
| Payloads | Lazy-load heavy assets to keep scene composition fast |

---

## 3. Modular environment architecture

```
World Stage
  └── /Environment                      (XForm)
        ├── /Room      [reference]       room_A.usd / room_B.usd
        ├── /Furniture [reference ×N]    table.usd, shelf.usd, …
        ├── /Objects   [reference ×M]    mug.usd, box.usd, …
        ├── /Lighting  [reference]       studio_light.usd / window_light.usd
        └── /Robot     [reference]       franka_panda.usd / ur10.usd
```

Each sub-assembly exposes **variant sets** that the Python API switches at episode reset:

```python
stage = omni.usd.get_context().get_stage()
room_prim = stage.GetPrimAtPath("/World/Environment/Room")
vset = room_prim.GetVariantSets().GetVariantSet("layout")
vset.SetVariantSelection(random.choice(["open_plan", "L_shape", "narrow"]))
```

---

## 4. Domain randomization

At each episode reset, a randomization script samples:

- **Geometry variants** — which room, furniture configuration, object set
- **Material properties** — albedo, roughness, metallic (via `UsdShade.Material`)
- **Lighting** — intensity, temperature, position of lights
- **Physics** — object mass, friction coefficients (via `PhysxSchema`)
- **Sensor noise** — Gaussian depth noise on simulated cameras

This implements the *uniform domain randomization* strategy of Tobin et al. (2017) and the *structured domain randomization* of Prakash et al. (2019), empirically shown to close the sim-to-real gap for vision-based manipulation.

---

## 5. Sensor configuration

Isaac Sim provides annotator-based synthetic data generation:

```python
from omni.replicator.core import AnnotatorRegistry
rgb_annot     = AnnotatorRegistry.get_annotator("rgb")
depth_annot   = AnnotatorRegistry.get_annotator("distance_to_image_plane")
normals_annot = AnnotatorRegistry.get_annotator("normals")
```

Camera rigs are themselves USD prims, repositionable via the modular hierarchy.

---

## 6. Requirements

| Dependency | Version |
|---|---|
| NVIDIA Isaac Sim | ≥ 2023.1 |
| Python | 3.10 (bundled with Isaac) |
| CUDA-capable GPU | RTX recommended |
| NVIDIA Omniverse Launcher | For Isaac Sim installation |

---

## 7. References

1. J. Tobin et al. "Domain Randomization for Transferring Deep Neural Networks from Simulation to the Real World." *IROS*, 2017.
2. A. Prakash et al. "Structured Domain Randomization: Bridging the Reality Gap by Context-Aware Synthetic Data." *ICRA*, 2019.
3. Pixar Animation Studios. *Universal Scene Description.* openusd.org, 2016.
4. NVIDIA. *Isaac Sim Documentation.* developer.nvidia.com/isaac-sim.
