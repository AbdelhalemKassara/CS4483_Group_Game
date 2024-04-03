# CS4483_Individual_Game_Prototype

## Dependencies
- Unity 2022.3.20f1

## Installation/Execution
### Macos
1. download the zip file of the project.
2. unzip.
3. open unity hub and click on "Add" and navigate to the folder location of the file you just unziped.
4. double click on it when it pops up on the unity editor (It should be called CS4483_Individual_Game_Prototype).
5. Click on file on the top bar and then click on Build Settings on the menu that pops up.
6. Click on Build and pick a folder location where you want the .app file to be stored.
7. Navigate back to where .app file is and double click it, it should start the program.

### Windows
Probably the same thing just with an .exe for the last couple of steps

## Features
- Keyboard and controller support (I would add a racing wheel but mine is at my parent's house)
- Basic test Track
- Start screen
- Track Selection Page
- Car Selection Page
- Settings Page (empty)
- Manual Transmission 
- Brake Lights
- Overlay that displays engine rpm and currently selected gear
- Anti-Roll bars simulation with adjustments
- Suspention and Damper simulation with adjustments
- Tire friction with adjustments 
- Basic Engine simulation with adjustments
- Basic Brake simulation with adjustments
- The wheels that are displayed are following the wheel that is used for the physics
- Gear number and ratios selection (in unity editor)
- Differential gear ratio (in unity editor)
- Basic engine simulated sound
- Steering angle adjustment 
- Front wheel drive, rear wheel drive, and all wheel drive are all supported
- Adjustable center of mass 
- two camera modes (dynamic (has a bit of a delay when following you) and fixed) with offset adjustments (anywhere in reference with the car, both inside and outside the car)
- Fov adjustment (in unity editor under camera)
- torque curves
- manual and automatic transmittion switching
- fully adjustable 
- campain where the user can earn money through completing the maps
- leaderboard
- time difference while driving
- Tire smoke 
- Tire slipping sound
- Awd RWd and FWd
- open and LSd differntials
- adjustable camera modes
- camera view to look around the car
- Controls menu to let the user know the controls
- We also support keyboard controls even though this isn't usable for this type of game 
- Pause menu
- volume adjustments
- Toggle to switch between automatic and manual transmission in the setitngs menu
- Free play and campain


## Notes
We ran into many issue with the built in Wheel Colliders as if the torque gets too high some value will wrap around and cause the wheels to spin the other way.


## Controls
### Keyboard
- Camera view mode switch : C
- Handbrake : spacebar
- Steering : A for left, D for right
- Down Shift : I 
- Up Shift : K 
- Brake : S
- Throttle W
- Look Around The Car : Arrow keys (down is default view)

### Controller (any controller should work, I did most testing on the xbox one controller)
- Camera view mode switch : Left shoulder
- Handbrake : South Button (A on an xbox controller)
- Steering : Leftstick
- Down Shift : Button West (X on a xbox controller)
- Up Shift : Button East (B on a xbox controller)
- Brake : Left trigger
- Throttle Right trigger
- Look Around The Car : right joysitc (fully down is the default view)


## How To Play
Click Start, Select, Select and will take you to the game.





