# Mathematics 3D Assignment

## Short summary of all exercises
All excercises are in the showcase scene.

# 1. State & Context
For state and context, I did a bouncy mushroom, where if you walk into it would get picked up or trampled (depending on how you look at it :D), and if you jump onto it from above you would bounce. This was initally made with normal collision boxes but was later modified to use intersection logic from exercise number 3.

# 2. Interpolation
For interpolation, I did an example of a box interpolationg forward and backwards, as well as interpolating the color of it. There is also an animation example, where I am interpolating between two animation layers, showing how it could apply to animations as well. You can change the easing function of these interpolation by going into a trigger box, also using logic from exercise number 3.

# 3. Intersection 
For intersection, I made a coin collection example. This is to show a Sphere-AABB intersection working, where coins have a custom sphere collider and the player has a custom box collider.

# 4. Collision
This was a complicated one in my opinion. I made three examples. One of balls bouncing on a box (0.8 bounciness), showing AABB-Sphere collision, one Sphere-Sphere collision where spheres are falling on a bigger sphere, and another one with spheres bouncing on a box with continous bouncing (1 bounciness). The collision and intersection are all calculated in CollisionUtility.cs, as well as in PhysicsWorld.cs. For the collision logic I made a simple collision solver, that is running in PhysicsWorld.cs as well. To activate some of these collision you have to go into a intersection box.

# 5. Noise
For noise I made a few examples. First of in NoiseUtility there are a random noise (System.Random), and a Perlin Noise class. Perlin Noise has two methods for getting 3D Perlin Noise, and getting 2D Perlin noise. I showcase these by first making 2D noise maps in a texture, showing both random and perlin. Then I also showcase perlin in 3D with a big cube grid. Lastly I showcase it for a (poorly optimized) animated waves mesh, by just creating a mesh and adding a scrolloffset. This is to showcase mostly a terrain generator, which since it is position dependant, is tileable and could be used for a procedural terrain generation. To implement noise in a previous exercise, I made a terrain for the mushrooms to stand on, using both noise to generate the terrain on Start() but also setting the mushrooms y position, so it aligns with the terrain.

# Small Custom Math Library
I also made a custom math library mostly for fun and learning. I am using most if not all math calculations from that library for everything except a lot of Vector math in collision. So easing functions as well as interpolating and noise.

