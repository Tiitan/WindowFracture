# Asset Creation Guide

This guide explains how to prepare content for Window Fracture:

- fracture pattern meshes
- glass materials/shaders
- breakable window panel meshes

## Fracture Pattern: Build And Import

### Goal

A fracture pattern is a line mesh projected onto the panel at impact time, then clipped to the panel boundary to produce shard loops.

### Authoring Requirements

- Pattern must be 2D on the XY plane (`Z = 0` for all vertices).
- Pattern lines must extend beyond the largest panel diagonal you expect in game.
  - This avoids under-coverage near borders after projection/rotation.
- Topology must represent connected loops/branches suitable for clipping.
- Vertices should remain in a consistent angular order around loops.
- Do not leave isolated interior vertices disconnected from valid loops.

### Style Recommendations

- Hand-authored patterns are preferred over fully procedural noise.
- A small set of high-quality patterns is enough when combined with random rotation.
- Build multiple pattern densities (small, medium, large shards) for gameplay variety.

### Import Into Unity

- Import the pattern as a mesh asset.
- Ensure the imported mesh is readable at runtime.
- Keep transform scale normalized in source assets to avoid accidental pattern distortion.
- Add imported meshes to `BaseGlass.Patterns` on the target panel/prefab.

### Validation Checklist

- Pattern is visible in XY orientation.
- No unexpected Z offsets.
- Pattern still covers panel after random rotation.
- Fracture does not produce missing shard regions near borders.

## Glass Shader And Material Requirements

### Submesh Material Contract

Runtime mesh generation preserves two submeshes:

- Submesh 0: side faces (edge thickness surfaces)
- Submesh 1: front/back faces

Use two materials on the source panel renderer to match this contract.

### Edge Shader Requirements

- Edge material should not rely on UV layout.
- If rendering double-sided edges (culling off), offset vertices slightly inward along normal in vertex stage to reduce z-fighting.
- Keep edge shading robust to thin geometry and steep view angles.

### Surface Shader Requirements

- Surface shader/material should support front/back usage from generated shard meshes.
- Only UV0 is propagated to generated shards.
- Use textures/materials that remain acceptable with UV0-only propagation.

### Practical Defaults

- Keep side material visually simpler than surface material.
- Avoid expensive per-pixel effects on edge material if many shards are expected.

## Window Panel Mesh Requirements

### Geometry Requirements

- Panel should be oriented on XY, with thickness on Z.
- Mesh origin should be centered along Z.
  - Runtime side selection uses vertex Z sign and impact-side filtering.
- Panel shape should be convex-like for best results.
- Non-uniform XY scale is supported by runtime conversion.

### Topology Requirements

- Keep vertex count reasonable.
  - Current runtime guard rejects meshes with fewer than 3 or more than 100 source vertices.
- Use clean manifold topology with predictable front/back surfaces.
- Avoid unnecessary decorative vertices on fracture-critical surfaces.

### Import Requirements

- Enable mesh Read/Write in Unity import settings.
- Ensure collider setup is present on the breakable object.
- Confirm renderer uses two material slots in the expected order.

### UV Requirements

- UV0 should be valid and regular enough for interpolation.
- Backside UV behavior should be compatible with your front-face texturing needs.
- Remember that shard UV interpolation currently uses a simplified barycentric approach.

## Common Setup Errors

- Empty `Patterns` array on `BaseGlass`.
- Pattern mesh not lying on XY plane.
- Panel mesh too dense or invalid for runtime polygon extraction.
- Missing `MeshFilter`, `MeshRenderer`, or `Collider`.
- Single-material panel where side and face rendering need different shading.

## Quick Preflight Before Shipping

- Trigger 20 to 50 breaks on representative panels.
- Test deterministic breaks (`patternIndex`, `rotation`) and random breaks.
- Verify edge material stability at glancing camera angles.
- Verify shard textures remain acceptable across large and tiny fragments.
