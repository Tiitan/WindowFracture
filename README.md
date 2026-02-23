# Window Fracture

Window Fracture is a runtime package for realistic, pattern-driven window shattering focused on flat glass panels.

On impact, a 2D fracture pattern is projected and clipped on the panel surface, converted into shard polygons, then extruded into runtime shard meshes. This approach is inspired by Receiver 2-style fracture logic and is optimized for annealed glass behavior rather than generic 3D volume destruction.

Compared to heavy physics-fracture workflows, this package keeps full artistic control over break style while remaining lightweight enough for gameplay-heavy and mobile-oriented projects.

## Features

- Hand-crafted fracture patterns with random rotation for high visual variety from a small pattern set.
- Deterministic-friendly fracture API (`patternIndex`, `rotation`) suitable for replay/network synchronization.
- Recursive shattering: spawned shards can break again.
- Supports convex-like panel shapes with arbitrary thickness.
- Supports non-uniform panel scaling on XY.
- UV0 propagation from source panel to generated shards for consistent surface texturing.
- Connectivity-based shard fall behavior:
  - frame-touching shards stay anchored
  - disconnected shard islands fall as cascades
- Lightweight runtime generation compared to full 3D volume fracture systems.
- Includes a ready-to-use sample scene and assets in `Samples~/Glass Demo`.

## Installation

Install with Unity Package Manager as:

- Embedded package: already available when the folder exists under `Packages/com.titan.windowfracture`.
- Git URL: add a Git dependency to your `manifest.json`.

## Package Layout

- `Runtime/` runtime scripts, prefab, shader/material/mesh resources, and MathNet plugins.
- `Samples~/Glass Demo/` removable demo scene and assets importable from Package Manager.
- `Tests/Runtime/` runtime package tests.
- `Documentation/tech_doc.md` internal implementation guide.

## Quick Start

1. Add `WindowFracture.Runtime.GlassPanel` to a GameObject that has `MeshFilter`, `MeshRenderer`, and `Collider`.
2. Assign one or more fracture line meshes to `Patterns`.
3. Ensure shared materials are configured on the panel renderer (the runtime keeps these materials on spawned shards).
4. Trigger fracture from gameplay code:

```csharp
using WindowFracture.Runtime;
using UnityEngine;

public class BreakExample : MonoBehaviour
{
    [SerializeField] private GlassPanel panel;

    public void BreakAt(Vector3 worldPoint, Vector3 worldDirection, float impulse)
    {
        panel.Break(worldPoint, worldDirection.normalized * impulse);
    }
}
```

5. Optional deterministic mode:
- Provide `patternIndex` and `rotation` explicitly in `Break(...)` for replay/network sync.

6. Import the `Glass Demo` sample from Package Manager and open `GlassSampleScene.unity` for a full reference setup.

## Requirements

- Unity `6000.3` or newer in the 6000.3 stream.

## Known Limitations

- Current sample materials target URP assets used by the demo project.
- Runtime fracture is designed for flat, window-like meshes and performs best on simple convex panel shapes.
- `GlassPanel` currently rejects meshes with fewer than 3 or more than 100 source vertices.
- UV interpolation currently relies on the first polygon triangle, which is less accurate on irregular UV layouts.

## Package Contents

| Location | Description |
|---|---|
| `Runtime/Scripts` | Runtime fracture components and helpers. |
| `Runtime/Resources` | Runtime meshes, materials, and shader resources. |
| `Runtime/Prefabs` | Ready-to-use glass prefab. |
| `Runtime/Plugins/MathNet` | Third-party MathNet binaries used by runtime code. |
| `Samples~/Glass Demo` | Removable sample scene, scripts, and materials. |
| `Documentation/tech_doc.md` | Internal architecture and implementation details. |
| `Documentation/Glass system.md` | Documentation index and navigation entrypoint. |

## Internal Documentation

For code architecture, algorithm flow, diagrams, and implementation details, see `Documentation/tech_doc.md`.

## License And Third-Party

This package includes MathNet third-party binaries. See `Third Party Notices.md` for details.
