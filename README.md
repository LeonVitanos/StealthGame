# Stealth game

The game features five levels. A single level of the game presents the player with a simple 2D polygon P with cameras placed inside it. The polygon is allowed to have holes. The cameras are points that have a 2D vision cone which observes part of P.

![alt text](https://github.com/LeonVitanos/StealthGame/blob/main/screenshots/screenshot.png?raw=true)

The player can move their character, a small green square, using the arrow keys or WASD. Their objective is to reach the goal, a small yellow area, without being observed by any of the cameras. The player is allowed to disable x number of cameras at a time by clicking on them with the mouse pointer, where x can vary by level.

We are utilizing the existing code written for the Ruler of the Plane project. We are making use of several of the existing classes to handle some of the lower-level computations and objects, such as Polygon2D for storing 2D polygons and Triangulation for generating meshes from those polygons.