# PVRU Project

## Abstract

Ever wanted to be a train conductor in real life while also defending against enemies? In our VR game Choo Choo, you can do exactly that!

In Choo Choo, you and your friends can take on the role of either a train conductor or a defender. As the conductor, your job is to keep the train running by fueling it with coal, cooling it down with water, and defending it from enemies with your trusty revolver. As a defender, you’re free to teleport on and off the train, collect resources like water and coal by shooting clouds and coal blocks, and, of course, fend off enemies to protect the train.

The goal is simple: keep the train running as long as possible. Every enemy that hits the train damages it, and when the train’s health reaches zero, the game is over. The train travels through various biomes and along different paths, ensuring that the experience is always fresh and exciting.

One of the cool features of Choo Choo is that players can switch roles anytime, keeping the gameplay dynamic and fun. The game supports two or more players, with Photon Fusion Cloud managing the multiplayer experience.

Choo Choo is designed for hand tracking only, so you’ll use hand gestures to interact with the game. Although hand gestures may take a moment to be recognized, patience will pay off! For example, making a thumbs-up pose and then pressing your thumb down triggers the revolver. Forming a slightly angled L with your thumb and index finger will bring up a teleportation ray, and pressing your thumb on your index finger will teleport you.

But the immersion doesn’t stop there! Choo Choo also integrates external hardware like EMS for damage feedback and a Peltier element that heats up when the locomotive’s kettle reaches a critical temperature, adding a layer of realism. You can cool down the kettle by grabbing the water bucket in the locomotive and pouring it over the fire. The cabin is well-organized, with the water bucket on the left, the kettle in the middle, and a suitcase full of coal on the right for easy access.

For quick updates, the current health of the train is displayed on a handy UI located on the player’s left palm. Just glance at your hand to see the train’s status.

So, what are you waiting for? All aboard the Choo Choo train and let the adventure begin!

## Project Setup

### Unity Version

- [Unity 2022.3.31f1 (LTS)](https://unity.com/releases/2022-lts)

### Installation Requirements

- [Android Build Support](https://docs.unity3d.com/Manual/android-sdksetup.html)

### Device Requirements

- [Quest must be in Developer Mode](https://developer.oculus.com/documentation/native/android/mobile-device-setup/)

### Meta Quest Configuration

1. **XR Plug-in Management**:

   - Navigate to `Project Settings -> XR Plug-in Management`.
   - Enable `OpenXR` in all tabs.

2. **Game Object Settings**:

   - Ensure the game object `XR Device Simulator` is set to disabled when using the Meta Quest.

3. **Build Settings**:
   - Make sure that the `Build Platform` is set to `Android` and that the `Meta Quest 3` shows up under `Run Device`.

## Content

## Implemented Features

- Hand Tracking and Gestures only
- Auto-Scrolling Train
- Dynamic Enemy Pathfinding / Dynamic Environment
- External Hardware Integration: EMS (for damage feedback), Peltier (for kettle heat feedback)
- Photon Fusion 2 Shared Mode Multiplayer
- Weapons
- And more!

## Included Assets

### Multiplayer

- [Photon Fusion 2](https://assetstore.unity.com/packages/tools/network/photon-fusion-267958)
- [Photon Fusion 2 - Physics Addon](https://doc.photonengine.com/fusion/current/addons/physics/overview)
- [Photon Fusion 2 - XR Addons 2.0.0](https://doc.photonengine.com/arvr/current/fusion-xr-addons/fusion-xr-addons-overview)
- [Photon Voice 2 (as some scripts depend on it)](https://assetstore.unity.com/packages/tools/audio/photon-voice-2-130518)

### Paid

- [POLYGON - Western Pack](https://syntystore.com/products/polygon-western-pack)
- [POLYGON - Nature Pack](https://syntystore.com/products/polygon-nature-pack)
- [POLYGON - Icons Pack](https://syntystore.com/products/polygon-icons-pack)
- [POLYGON - Particle FX Pack (Unity only)](https://syntystore.com/products/polygon-particle-fx-pack)

### Free (but mostly used)

- [POLYGON - Bow and Crossbow | FREE](https://syntystore.com/products/polygon-bow-and-crossbow)
- [POLYGON - Pride Crystal Weapons | FREE](https://syntystore.com/products/polygon-pride-crystal-weapons)
- [POLYGON - Pride Plushies | FREE](https://syntystore.com/products/polygon-pride-plushies)
- [POLYGON - Quad Bike | FREE](https://syntystore.com/products/polygon-quad-bike)
- [POLYGON - Starter Pack | FREE](https://syntystore.com/products/polygon-starter-pack)
- [POLYGON - Water Guns | FREE](https://syntystore.com/products/polygon-water-guns)
- [POLYGON - Xmas Pack | FREE](https://syntystore.com/products/polygon-xmas-pack)
- [Simple FX - Cartoon Particles | FREE](https://syntystore.com/products/simple-fx-cartoon-particles)
- [LE Low Poly Cloud Pack](https://assetstore.unity.com/packages/3d/3le-low-poly-cloud-pack-65911)
- [Low Poly Pistol Weapon Pack 1](https://assetstore.unity.com/packages/3d/props/weapons/low-poly-pistol-weapon-pack-1-285693)
- [Low-Poly Medieval Market](https://assetstore.unity.com/packages/3d/environments/low-poly-medieval-market-262473)
- [Tool Set - PolyPack](https://assetstore.unity.com/packages/3d/props/tools/tool-set-polypack-207678)
- [RPG Monster Duo PBR Polyart](https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-duo-pbr-polyart-157762)
- [WorldSkies Free Skybox Pack](https://assetstore.unity.com/packages/2d/textures-materials/sky/worldskies-free-skybox-pack-86517)
- [A\* Pathfinding Project](https://arongranberg.com/astar/)
- [Jammo Character | Mix and Jam](https://assetstore.unity.com/packages/3d/characters/jammo-character-mix-and-jam-158456)
- [Robot Kyle | URP](https://assetstore.unity.com/packages/3d/characters/robots/robot-kyle-urp-4696)

## Included Packages

- [OpenXR Plugin 1.11.0](https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.11/manual/index.html)
- [Splines 2.6.1](https://docs.unity3d.com/Packages/com.unity.splines@2.6/manual/index.html)
- [TextMeshPro 3.0.9](https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/manual/index.html)
- [Universal RP 14.0.11](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/manual/index.html)
- [XR Hands 1.4.1](https://docs.unity3d.com/Packages/com.unity.xr.hands@1.4/manual/index.html)
- [XR Interaction Toolkit 2.5.4](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.5/manual/index.html)
- [XR Plugin Management 4.5.0](https://docs.unity3d.com/Packages/com.unity.xr.management@4.5/manual/index.html)
- [Animation Rigging 1.3.0](https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/index.html)
- [Mono Cecil 1.11.4](https://docs.unity3d.com/Packages/com.unity.nuget.mono-cecil@1.11/manual/index.html)
- [Dreamteck Splines](https://assetstore.unity.com/packages/tools/utilities/dreamteck-splines-61926)

## Additional Assets

- [`VR Development`](https://learn.unity.com/learn/pathway/vr-development) Tutorial (`Create-with-VR_2022LTS`)
- [Pixabay](https://pixabay.com/de/sound-effects/)
