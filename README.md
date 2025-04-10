# Cacophony Playground

A collection of examples for how to use the Cacophony Gesture Library. The examples show how you can use the library for practical applications as well as fun and experimentation. Each example provides engaging feedback to your actions, showing how you can use the various features of the library to build engaging and usable apps.

## Requirements

- Tested with Unity 6000.0 (milage may vary with earlier versions)
- Ultraleap Tracking Camera

## Setup


This Unity Project has a dependency on the cacophony GestureSystem as a submodule. 
Once cloned, go to the command line and set up submodules via:

`git submodule update --init --recursive`

# Ultraleap Hand Tracking Examples

## Light Bulb

A simple example showing how to connect a gesture detection to an action that requires a hand motion. This example shows how you can provide clear feedback at each stage of the interaction.

![Animation of a hand pinching and moving down, turning on a light bulb](/media/Lightbulb.gif "Lightbulb animation")

- Form an OK pose with your hand, then move down to plug in the light bulb.
- Notice how there is visual feedback when the gesture first detects, as you progress and on completion of the movement.

## Handy Menu

A calming example of how several actions can be used alongside the same gesture to control a context menu. 

![Animation showing a hand pinching and moving to control a pie menu](/media/HandyMenu.gif "Handy Menu animation")

- Move your hand to paint with the particles and listen to relaxing music.
- Form a fist and release to summont the pie menu. It should appear in front of your hand
- Form an OK pose and move in a direction to select an audio filter and colour for the particles.
- Form the fist and release again to dismiss the menu.

## Number Selector

An example of how to use several distinct gestures alongside each other, with a common action style. 

![Animation of a hand pinching and moving down, turning on a light bulb](/media/Numbers.gif "Number selector animation")

- Hold out your hand with 1, 2, 3, 4, or 5 fingers extended to change the sound and manipulate the shapes.
- Form a fist with your hand to release control of the shapes
- Control of the shapes is only maintained until a three second timer is completed, then released.

## Cacophonous

An illustration of why the Cacophony library exists. This is primarily a "How not to do it" scene. Enabling too many gestures at once creates a chaotic effect and a difficult to use application!

![Animation of a hand pinching and moving down, turning on a light bulb](/media/Cacophonous.gif "Cacophonous animation")

- Form an OK pose with your hand, then move down to plug in the light bulb.
- Notice how there is visual feedback when the gesture first detects, as you progress and on completion of the movement.